using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.PlatformUI;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace UnityDotsAuthoringGenerator.Classes
{
    internal class Templates
    {
        public Templates() {
        }
        public void GenerateDefaultInstallation() {
            ThreadHelper.ThrowIfNotOnUIThread();
            var basePath = Path.Combine(Path.GetDirectoryName(DteHelper.GetProject().FileName), "CodeTemplates");

            var filePath = Path.Combine(basePath, "Files");
            filePath += Path.DirectorySeparatorChar;
            GenerateExampleFileTemplates(filePath);
            SettingsManager.Instance.Set(SettingsManager.FILES_PATH, filePath);

            var snippetsPath = Path.Combine(basePath, "Snippets");
            snippetsPath += Path.DirectorySeparatorChar;
            GenerateExampleSnippets(snippetsPath);
            SettingsManager.Instance.Set(SettingsManager.SNIPPETS_PATH, snippetsPath);          
            SettingsManager.Instance.SaveSettings();
        }

        public void GenerateExampleFileTemplates(string path = "") {
            if (path == "") {
                path = Utils.AskUserForPath("Provide a location for your template files. Note: All files in and below given folder" +
                    " will be made available as pastable template!", "", SettingsManager.FILES_PATH);
            }
            if (path != "") {
                generateExampleFiles("Templates.Files", path);
            }
        }

        public void GenerateExampleSnippets(string path = "") {
            if (path == "") {
                path = Utils.AskUserForPath("Provide a location for your template snippets. Note: All files in and below given folder" +
                    " will be processed and made be available as pastable snippets!", "", SettingsManager.SNIPPETS_PATH);
            }
            if (path != "") {
                generateExampleFiles("Templates.Snippets", path);
            }
        }

        private void generateExampleFiles(string resourceFolder, string generationPath) {
            ThreadHelper.ThrowIfNotOnUIThread();
            var fileTemplates = ResourceHelper.GetResourcesFromFolder(resourceFolder);
            foreach ((string fileName, string content) in fileTemplates) {
                if(!Directory.Exists(generationPath)) {
                    Directory.CreateDirectory(generationPath);
                }
                var fullFilePath = generationPath + fileName;
                if (!File.Exists(fullFilePath)) {
                    File.WriteAllText(fullFilePath, content);
                    DteHelper.GetProject().ProjectItems.AddFromFile(fullFilePath);
                }
                else {
                    MessageBox.Show(string.Format("File with name '{0}' already exists, skipped generating!", fileName), "Generation skipped",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public List<(string name, string content)> LoadFiles(string path) {
            if (path == "") {
                return null;
            }
            return loadFiles(path);
        }

        public List<(string name, string content)> LoadSnippets(string path) {
            if (path == "") {
                return null;
            }
            var snippets = new List<(string name, string content)>();
            var files = loadFiles(path);

            foreach ((string fileName, string fileContent) in files) {
                var fileSnippets = Regex.Matches(fileContent, "\\/\\/ *snippet +(\\w*) +start\\r\\n((?:.*?|\\r\\n)*?).*\\/\\/ *snippet +stop", RegexOptions.IgnoreCase);

                foreach (Match snippet in fileSnippets) {
                    var snippetName = snippet.Groups[1].Value;
                    var snippetContent = snippet.Groups[2].Value;
                    snippets.Add((snippetName, snippetContent));
                }
            }
            return snippets;
        }

        private List<(string name, string content)> loadFiles(string path) {
            var fileContents = new List<(string name, string content)>();
            var filePaths = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);
            foreach (var filePath in filePaths) {
                string content;
                try {
                    content = File.ReadAllText(filePath);
                }
                catch (Exception ex) {
                    MessageBox.Show(string.Format("Failed reading file: {0}", ex.Message), "Failed reading file");
                    continue;
                }
                var name = Path.GetFileName(filePath);
                fileContents.Add((name, content));
            }
            return fileContents;
        }
    }
}
