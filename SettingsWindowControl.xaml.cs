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

                var storedPath = SettingsManager.Instance.TryGet("GenerationPath");
                textBox_Path.Text = storedPath;
                text_Current.Text = DteHelper.GetSelectedFilePath();

                // scroll to end of line
                text_Current.Focus();
                text_Current.Select(text_Current.Text.Length, 0);
                               
                textBox_Path.Focus();
                textBox_Path.Select(textBox_Path.Text.Length, 0);
            };
        }

               
        private void button1_Click(object sender, RoutedEventArgs e) {
            var path = textBox_Path.Text;
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString())) {
                path += Path.DirectorySeparatorChar.ToString();
            }
            SettingsManager.Instance.Set("GenerationPath", path);
            Window.GetWindow(this).Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e) {
            Window.GetWindow(this).Close();
        }     
    }
}