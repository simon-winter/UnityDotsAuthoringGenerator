using Microsoft.VisualStudio.Debugger.Interop;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using UnityDotsAuthoringGenerator.Classes;

namespace UnityDotsAuthoringGenerator {
/// <summary>
/// Interaction logic for TemplateWindowControl.
/// </summary>
public partial class TemplateWindowControl : UserControl {
    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateWindowControl"/> class.
    /// </summary>
    public TemplateWindowControl()
    {
        this.InitializeComponent();
        var m_templates = new Templates();

        this.Loaded += (object sender, RoutedEventArgs e) =>
        {
            wrapPanel_Files.Children.Clear();
            wrapPanel_Snippets.Children.Clear();

            var filesPath = SettingsManager.Instance.TryGet(SettingsManager.FILES_PATH);
            if (filesPath == "") {
                var result = MessageBox.Show("No file template path in settings configured.\nDo you want to generate the default examples?",
                    "Template file path not set", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.OK) {
                    m_templates.GenerateExampleFileTemplates();
                    Window.GetWindow(this).Close();
                }
            }
            if (filesPath != "") {
                foreach (var file in m_templates.LoadFiles(filesPath)) {
                    wrapPanel_Files.Children.Add(new Button() {
                        Height = 30,
                        Width = 120,
                        Margin = new Thickness(2, 2, 2, 2),
                        Content = Path.GetFileNameWithoutExtension(file.name),
                        Command = new CreateFile(DteHelper.GetSelectedFileDirectory(), Path.GetExtension(file.name), file.content)
                    });
                };
            }

            var snippetsPath = SettingsManager.Instance.TryGet(SettingsManager.SNIPPETS_PATH);
            if (snippetsPath == "") {
                var result = MessageBox.Show("No snippets template path in settings configured.\nDo you want to generate the default examples?",
                    "Snippets path not set", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.OK) {
                    m_templates.GenerateExampleSnippets();
                    Window.GetWindow(this).Close();
                }
            }
            if (snippetsPath != "") {
                foreach (var snippet in m_templates.LoadSnippets(snippetsPath)) {
                    wrapPanel_Snippets.Children.Add(new Button() {
                        Height = 30,
                        Width = 120,
                        Margin = new Thickness(2, 2, 2, 2),
                        Content = snippet.name,
                        Command = new Snippet(snippet.name, snippet.content)
                    });
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
    private void button1_Click(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this).Close();
    }

    private void button2_Click(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this).Close();
    }
        private void checkbox_surpressCopyMsg_Changed(object sender, RoutedEventArgs e) {
            SettingsManager.Instance.Set(SettingsManager.DISABLE_CLIPBOARD_MESSAGE,
            (bool)checkbox_surpressCopyMsg.IsChecked ? "true" : "false");
        }
    }
}
