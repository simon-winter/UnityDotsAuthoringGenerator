using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualStudio.Shell;
using System.Media;

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

    public static void PlaySound()
    {
        string soundFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"Media\ding.wav");
        if (File.Exists(soundFilePath)) {
            try {
                using (SoundPlayer player = new SoundPlayer(soundFilePath))
                {
                    player.Play();
                }
            } catch (Exception ex) {
                Utils.ShowErrorBox(string.Format("Can't play sound: ", ex.Message));
            }
        }
    }

    public static string AskUserForPath(string description, string openAt, string safeTo)
    {
        ThreadHelper.ThrowIfNotOnUIThread();
        if (openAt == "" || (!Directory.Exists(openAt) && !File.Exists(openAt))) {
            openAt = Path.GetDirectoryName(DteHelper.GetProject().FileName);
        }

        var dialog = new FolderPicker();
        dialog.InputPath = openAt;
        dialog.Title = description;

        var result = dialog.ShowDialog();
        if (result != true) {
            return "";
        }

        if (safeTo != "") {
            SettingsManager.Instance.Set(safeTo, dialog.ResultName + Path.DirectorySeparatorChar);
            SettingsManager.Instance.SaveSettings();
        }

        return dialog.ResultName + Path.DirectorySeparatorChar;
    }

    public static void ShowErrorBox(string error)
    {
        MessageBox.Show(string.Format(error), "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
}
