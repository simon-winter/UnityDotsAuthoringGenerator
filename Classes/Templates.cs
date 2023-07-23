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

namespace UnityDotsAuthoringGenerator.Classes
{
    internal class Templates
    {
        public Templates() {
            MessageBox.Show(
             string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
             "TemplateWindow");
        }

        public List<(string name, string content)> LoadFiles(AsyncPackage pkg) {
            var path = SettingsManager.Instance.TryGet(SettingsManager.FILES_PATH);
            return loadFiles(pkg, path);
        }

        public List<(string name, string content)> LoadSnippets(AsyncPackage pkg) {
            var snippets = new List<(string name, string content)>();
            var path = SettingsManager.Instance.TryGet(SettingsManager.SNIPPETS_PATH);
            var files = loadFiles(pkg, path);

            foreach ((string fileName, string fileContent) in files) {
                var fileSnippets = Regex.Matches(fileContent, "\\/\\/ *snippet +(\\w*) +start\\n((?:.*?|\\n)*?).*\\/\\/ *snippet +stop");

                foreach (Match snippet in fileSnippets) {
                    var snippetName = snippet.Groups[1].Value;
                    var snippetContent = snippet.Groups[2].Value;
                    snippets.Add((snippetName, snippetContent));
                }
            }
            return snippets;
        }

        private List<(string name, string content)> loadFiles(AsyncPackage pkg, string path) {
            var fileContents = new List<(string name, string content)>();
            var filePaths = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);
            foreach (var filePath in filePaths) {
                string content;
                try {
                    content = File.ReadAllText(filePath);
                }
                catch (Exception ex) {
                    VsShellUtilities.ShowMessageBox(pkg,
                     string.Format("Failed reading file: {0}", ex.Message),
                     "Failed reading file",
                     OLEMSGICON.OLEMSGICON_WARNING,
                     OLEMSGBUTTON.OLEMSGBUTTON_OK,
                     OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                    continue;
                }
                var name = Path.GetFileNameWithoutExtension(filePath);
                fileContents.Add((name, content));
            }
            return fileContents;
        }
    }
}
