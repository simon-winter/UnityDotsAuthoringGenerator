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
                var fileSnippets = Regex.Matches(fileContent, "\\/\\/ *snippet +(\\w*) +start\\n((?:.*?|\\n)*?).*\\/\\/ *snippet +stop");

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
                var name = Path.GetFileNameWithoutExtension(filePath);
                fileContents.Add((name, content));
            }
            return fileContents;
        }
    }
}
