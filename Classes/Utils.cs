using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualStudio.Shell;

namespace UnityDotsAuthoringGenerator.Classes {
internal class Utils {
    // returns given path/file path as directory, ensuring it ends on path seperator and there is no file extension in it
    public static string GetAsDirectory(string path)
    {
        if (path == "") {
            return "";
        }
        return Path.GetDirectoryName(path) + Path.DirectorySeparatorChar.ToString();
    }

    public static string AskUserForPath(string description, string openAt, string safeTo)
    {
        ThreadHelper.ThrowIfNotOnUIThread();
        if (openAt == "" || (!Directory.Exists(openAt) && !File.Exists(openAt))) {
            openAt = Path.GetDirectoryName(DteHelper.GetProject().FileName);
        }
        OpenFileDialog dialog = new OpenFileDialog();
        // FolderBrowserDialog dialog = new FolderBrowserDialog();
        dialog.Title = description;

        dialog.InitialDirectory = openAt;
        var result = dialog.ShowDialog();
        if (result != DialogResult.OK) {
            return "";
        }

        if (safeTo != "") {
            SettingsManager.Instance.Set(safeTo, dialog.InitialDirectory + Path.DirectorySeparatorChar);
            SettingsManager.Instance.SaveSettings();
        }

        return dialog.InitialDirectory + Path.DirectorySeparatorChar;
    }

    public static void ShowErrorBox(string error)
    {
        MessageBox.Show(string.Format("An error occured: {0}", error), "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
}
