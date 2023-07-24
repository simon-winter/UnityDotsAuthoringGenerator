using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UnityDotsAuthoringGenerator.Classes;

namespace UnityDotsAuthoringGenerator
{
    /// <summary>
    /// Interaction logic for TemplateWindowControl.
    /// </summary>
    public partial class TemplateWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateWindowControl"/> class.
        /// </summary>
        public TemplateWindowControl() {
            this.InitializeComponent();
            var m_templates = new Templates();

            this.Loaded += (object sender, RoutedEventArgs e) => {
                wrapPanel_Files.Children.Clear();
                wrapPanel_Snippets.Children.Clear();
                var filesPath = SettingsManager.Instance.TryGet(SettingsManager.FILES_PATH);
                if (filesPath != "") {
                    foreach (var file in m_templates.LoadFiles(filesPath)) {                        
                        wrapPanel_Files.Children.Add(new Button() {
                            Height = 30,
                            Width = 50,
                            Content = file.name,                           
                            Command = new CreateFile("", file.content)
                        }) ;
                    };
                }



            };
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show(
                string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
                "TemplateWindow");
        }

        private void button2_Click(object sender, RoutedEventArgs e) {

        }
    }
   }