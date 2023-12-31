﻿using Microsoft.VisualStudio.PlatformUI;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace UnityDotsAuthoringGenerator.Classes {
public class CreateFile : ICommand {
    private string m_path;
    private string m_content;
    private string m_extension;

    public CreateFile(string extension, string content)
    {
        m_content = content;
        m_extension = extension;
    }

    public void Execute(object parameter)
    {
        try {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var canceled = !TextInputDialog.Show("Enter name", "Enter the name for the desired file:", "",
                delegate(string obj) { return obj != ""; }, out var name);
            if (canceled) {
                return;
            }
            if (!name.Contains(".")) {
                name += m_extension;
            }

            var path = Utils.AskUserForPath("Choose destination folder", DteHelper.GetSelectedPath(), "");
            if (path == "") {
                return;
            }

            var filePath = Path.Combine(path, name);

            m_content = Regex.Replace(m_content, "TEMPLATENAME_", Path.GetFileNameWithoutExtension(name));
            File.WriteAllText(filePath, m_content);
            DteHelper.GetProject().ProjectItems.AddFromFile(filePath);
        } catch (Exception ex) {
            Utils.ShowErrorBox(ex.Message);
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
