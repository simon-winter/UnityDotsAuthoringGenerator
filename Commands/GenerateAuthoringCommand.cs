using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.Settings;
using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnityDotsAuthoringGenerator.Classes;
using Task = System.Threading.Tasks.Task;

namespace UnityDotsAuthoringGenerator {
/// <summary>
/// Command handler
/// </summary>
internal sealed class GenerateAuthoringCommand {
    /// <summary>
    /// Command ID.
    /// </summary>
    public const int CommandId = 0x0100;

    /// <summary>
    /// Command menu group (command set GUID).
    /// </summary>
    public static readonly Guid CommandSet = new Guid("af2be51c-e758-4806-a75f-a34a33403ace");

    /// <summary>
    /// VS Package that provides this command, not null.
    /// </summary>
    private readonly AsyncPackage package;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenerateAuthoringCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    private GenerateAuthoringCommand(AsyncPackage package, OleMenuCommandService commandService)
    {
        this.package = package ?? throw new ArgumentNullException(nameof(package));
        commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

        var menuCommandID = new CommandID(CommandSet, CommandId);
        var menuItem = new MenuCommand(this.Execute, menuCommandID);
        commandService.AddCommand(menuItem);
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static GenerateAuthoringCommand Instance {
        get;
        private set;
    }

    /// <summary>
    /// Gets the service provider from the owner package.
    /// </summary>
    private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
    {
        get {
            return this.package;
        }
    }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package)
    {
        // Switch to the main thread - the call to AddCommand in GenerateAuthoringCommand's constructor requires
        // the UI thread.
        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

        OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
        Instance = new GenerateAuthoringCommand(package, commandService);
    }

    string bakerBlueprint = @"
public class {0}Baker : Baker<{0}Authoring>
{{
    public override void Bake({0}Authoring authoring)
    {{
        var e = GetEntity(authoring, TransformUsageFlags.None);
        {1}(e, new {0}
        {{
{2}        }});
    }}
}}";
    string bakerBufferBlueprint = @"
public class {0}Baker : Baker<{0}Authoring>
{{
    public override void Bake({0}Authoring authoring) 
    {{
        var e = GetEntity(authoring, TransformUsageFlags.None);
        AddBuffer<{0}>(e);
    }}
}}";
    string authoringBlueprint = @"
public class {0}Authoring : MonoBehaviour
{{
{1}}}";

    /// <summary>
    /// This function is the callback used to execute the command when the menu item is clicked.
    /// See the constructor to see how the menu item is associated with this function using
    /// OleMenuCommandService service and MenuCommand class.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    private void Execute(object sender, EventArgs e)
    {
        ThreadHelper.ThrowIfNotOnUIThread();

        var clickedFilePath = DteHelper.GetSelectedFilePath();

        if (!clickedFilePath.EndsWith(".cs")) {
            Utils.ShowErrorBox("Only a .cs file can be used for generation.");
            return;
        }

        // parsing input file and collect data for output file
        var usings = new StringBuilder();
        var authoringValues = new StringBuilder();
        var bakerValues = new StringBuilder();
        string name = "";
        string componentType = "";
        foreach (string line in File.ReadLines(clickedFilePath)) {

            // copy usings one to one
            if (Regex.Match(line, "^\\s*?using .*$").Success) {
                usings.AppendLine(line);
                continue;
            }

            // handling type of component
            var signature = Regex.Match(line, "^.*struct\\s*(.*) \\s*:\\s?(IComponentData|ISharedComponentData|IBufferElementData)\\s*");
            if (signature.Success) {
                if (name != "") {
                    Utils.ShowErrorBox("Having multiple structs in a file is not supported.");
                    return;
                }
                name = signature.Groups[1].Value;
                componentType = signature.Groups[2].Value;
                // buffers don't get any fields and we can early out
                if (componentType == "IBufferElementData") {
                    break;
                }
                continue;
            }

            // copy member fields into ECS format
            var field = Regex.Match(line, "^\\s*public\\s+(.*?) (\\S*);");
            if (field.Success) {
                var vType = field.Groups[1].Value;
                var vName = field.Groups[2].Value;

                // handle copying to new format for default and special cases
                switch (vType) {
                default:
                    authoringValues.AppendLine(string.Format("    public {0} {1};", vType, vName));
                    bakerValues.AppendLine(string.Format("            {0} = authoring.{0},", vName));
                    break;
                case "Entity":
                    authoringValues.AppendLine(string.Format("    public GameObject {0};", vName));
                    bakerValues.AppendLine(string.Format("            {0} = GetEntity(authoring.{0}, TransformUsageFlags.None)", vName));
                    break;
                }
                continue;
            }
        }

        // sticking file parts together
        var fileContent = new StringBuilder();
        fileContent.Append(usings.ToString());
        if (componentType != "IBufferElementData") {
            fileContent.Append(string.Format(authoringBlueprint, name, authoringValues.ToString()));
            fileContent.AppendLine();
            var addCompType = componentType.Contains("Shared") ? "AddSharedComponent" : "AddComponent";
            fileContent.Append(string.Format(bakerBlueprint, name, addCompType, bakerValues.ToString()));
        } else {
            fileContent.Append(string.Format(authoringBlueprint, name));
            fileContent.Append(string.Format(bakerBufferBlueprint, name));
        }

        // write file, add to VS project
        var targetPath = SettingsManager.Instance.TryGet(SettingsManager.GENERATOR_PATH);
        if (targetPath == "") {
            targetPath = Path.GetDirectoryName(clickedFilePath) + Path.DirectorySeparatorChar;
        }
        var targetFile = Path.GetFileName(clickedFilePath.Replace(".cs", "_Authoring.cs"));
        var destination = targetPath + targetFile;
        try {
            File.WriteAllText(destination, fileContent.ToString());
            DteHelper.GetProject().ProjectItems.AddFromFile(destination);
        } catch (Exception ex) {
            Utils.ShowErrorBox(ex.Message);
        }
    }
}
}
