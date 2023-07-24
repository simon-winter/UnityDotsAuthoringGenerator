using Microsoft.VisualStudio.Shell.Events;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace UnityDotsAuthoringGenerator
{
    /// <summary>
    /// Interaction logic for SettingsWindowControl.
    /// </summary>
    public partial class SettingsWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsWindowControl"/> class.
        /// </summary>
        public SettingsWindowControl() {
            this.InitializeComponent();

            this.Loaded += (object sender, RoutedEventArgs e) => {

                text_Current.Text = DteHelper.GetSelectedFilePath();
                textBox_Path.Text = SettingsManager.Instance.TryGet(SettingsManager.GENERATOR_PATH);
                text_snippetsPath.Text = SettingsManager.Instance.TryGet(SettingsManager.SNIPPETS_PATH);
                text_filesPath.Text = SettingsManager.Instance.TryGet(SettingsManager.FILES_PATH);

                // scroll to end of line 
                textBox_Path.Focus();
                textBox_Path.Select(textBox_Path.Text.Length, 0);

                text_snippetsPath.Focus();
                text_snippetsPath.Select(textBox_Path.Text.Length, 0);

                text_filesPath.Focus();
                text_filesPath.Select(textBox_Path.Text.Length, 0);
            };
        }


        private void button1_Click(object sender, RoutedEventArgs e) {
            SettingsManager.Instance.Set(SettingsManager.GENERATOR_PATH, getAsDirectory(textBox_Path.Text));
            SettingsManager.Instance.Set(SettingsManager.SNIPPETS_PATH, getAsDirectory(text_snippetsPath.Text));
            SettingsManager.Instance.Set(SettingsManager.FILES_PATH, getAsDirectory(text_filesPath.Text));
            SettingsManager.Instance.SaveSettings();
            Window.GetWindow(this).Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e) {
            Window.GetWindow(this).Close();
        }

        // returns given path/file path as directory, ensuring it ends on path seperator and there is no file extension in it
        private string getAsDirectory(string path) {
            if (path == "") {
                return Path.DirectorySeparatorChar.ToString();
            }
            return Path.GetDirectoryName(path) + Path.DirectorySeparatorChar.ToString();
        }
    }
}