using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace UnityDotsAuthoringGenerator.Classes
{
    public class CreateFile : ICommand
    {        
        private string m_path;
        private string m_content;

        public CreateFile(string path, string content) {  
            m_path = path;
            m_content = content;
        }

        public void Execute(object parameter) {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.selectedd path //todo here we pick lcoation of right clic kinstead
            m_path = dialog.ShowDialog();            
            File.WriteAllText(m_path, m_content);
        }

        public event EventHandler CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

    }
}
