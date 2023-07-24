using Microsoft.VisualStudio.PlatformUI;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace UnityDotsAuthoringGenerator.Classes {
public class Snippet : ICommand {

    private string m_content;
    private string m_name;

    public Snippet(string name, string content)
    {
        m_content = content;
        m_name = name;
    }

    public void Execute(object parameter)
    {
        Clipboard.SetText(m_content);
        if (SettingsManager.Instance.TryGet(SettingsManager.DISABLE_CLIPBOARD_MESSAGE) != "true") {
            MessageBox.Show(string.Format("Copied snipped {0} to clipboard.", m_name), "Copied to clipboard",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }
}
}
