using Microsoft.VisualStudio.Shell.Events;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using UnityDotsAuthoringGenerator.Classes;

namespace UnityDotsAuthoringGenerator {
/// <summary>
/// Interaction logic for SettingsWindowControl.
/// </summary>
public partial class SettingsWindowControl : UserControl {
    Checkbox chkBx_disableCopyHint;
    Checkbox chkBx_relativeGen;
    Checkbox chkBx_playGenNotification;

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsWindowControl"/> class.
    /// </summary>
    public SettingsWindowControl()
    {
        chkBx_disableCopyHint = new Checkbox(SettingsManager.DISABLE_CLIPBOARD_MESSAGE, false);
        chkBx_relativeGen = new Checkbox(SettingsManager.GENERATE_RELATIVE, true);
        chkBx_playGenNotification = new Checkbox(SettingsManager.PLAY_GENERATED_SOUND, true);
        this.InitializeComponent();

        this.Loaded += (object sender, RoutedEventArgs e) =>
        {
            text_Current.Text = DteHelper.GetSelectedPath();
            textBox_generate.Text = SettingsManager.Instance.TryGet(SettingsManager.GENERATOR_PATH) + " ";
            textBox_snippetsPath.Text = SettingsManager.Instance.TryGet(SettingsManager.SNIPPETS_PATH) + " ";
            textBox_filesPath.Text = SettingsManager.Instance.TryGet(SettingsManager.FILES_PATH) + " ";
            checkbox_surpressCopyMsg.IsChecked = chkBx_disableCopyHint.Checked;
            checkbox_generateRelative.IsChecked = chkBx_relativeGen.Checked;
            checkbox_playGenSound.IsChecked = chkBx_playGenNotification.Checked;

            textBox_generate.IsEnabled = !chkBx_relativeGen.Checked;

            // scroll to end of line
            textBox_generate.Focus();
            textBox_generate.Select(textBox_generate.Text.Length, 0);

            textBox_snippetsPath.Focus();
            textBox_snippetsPath.Select(textBox_snippetsPath.Text.Length, 0);

            textBox_filesPath.Focus();
            textBox_filesPath.Select(textBox_filesPath.Text.Length, 0);
        };
    }

    private void button1_Click(object sender, RoutedEventArgs e)
    {
        SettingsManager.Instance.Set(SettingsManager.GENERATOR_PATH, Utils.GetAsDirectory(textBox_generate.Text.Trim()));
        SettingsManager.Instance.Set(SettingsManager.SNIPPETS_PATH, Utils.GetAsDirectory(textBox_snippetsPath.Text.Trim()));
        SettingsManager.Instance.Set(SettingsManager.FILES_PATH, Utils.GetAsDirectory(textBox_filesPath.Text.Trim()));

        chkBx_disableCopyHint.Checked = (bool)checkbox_surpressCopyMsg.IsChecked;
        chkBx_relativeGen.Checked = (bool)checkbox_generateRelative.IsChecked;
        chkBx_playGenNotification.Checked = (bool)checkbox_playGenSound.IsChecked;

        SettingsManager.Instance.SaveSettings();
        Window.GetWindow(this).Close();
    }

    private void button2_Click(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this).Close();
    }

    private void button_filesBrowse_Click(object sender, RoutedEventArgs e)
    {
        textBox_filesPath.Text = Utils.AskUserForPath("", textBox_filesPath.Text, SettingsManager.FILES_PATH) + " ";
    }

    private void button_snippetsBrowse_Click(object sender, RoutedEventArgs e)
    {
        textBox_snippetsPath.Text = Utils.AskUserForPath("", textBox_snippetsPath.Text, SettingsManager.SNIPPETS_PATH) + " ";
    }

    private void button_generateBrowse_Click(object sender, RoutedEventArgs e)
    {
        textBox_generate.Text = Utils.AskUserForPath("", textBox_generate.Text, SettingsManager.GENERATOR_PATH) + " ";
    }

    private void button_snippetsCreate_Click(object sender, RoutedEventArgs e)
    {
    }

    private void button_filesCreate_Click(object sender, RoutedEventArgs e)
    {
    }

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        textBox_generate.IsEnabled = !(bool)checkbox_generateRelative.IsChecked;
    }

    private void checkbox_playGenSound_Checked(object sender, RoutedEventArgs e)
    {
    }
}
}
