using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using UnityDotsAuthoringGenerator.Classes;

namespace UnityDotsAuthoringGenerator
{
    /// <summary>
    /// Interaction logic for InstantiateWindowControl.
    /// </summary>
    public partial class InstantiateWindowControl : UserControl
    {
        //Templates m_templates;
        /// <summary>
        /// Initializes a new instance of the <see cref="InstantiateWindowControl"/> class.
        /// </summary>
        public InstantiateWindowControl() {
            this.InitializeComponent();
            var button = new Button() {
                Width = 50,
                Height = 30,
            };
           
            this.Loaded += (object sender, RoutedEventArgs e) => {
                wrapPanel_Files.Children.Clear();
                wrapPanel_Snippets.Children.Clear();
                //m_templates = new Templates();
                wrapPanel_Files.Children.Add(button);
                wrapPanel_Files.Children.Add(button);
                wrapPanel_Files.Children.Add(button);
                wrapPanel_Files.Children.Add(button);
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
           
        }

        private void button2_Click(object sender, RoutedEventArgs e) {

        }
    }
}