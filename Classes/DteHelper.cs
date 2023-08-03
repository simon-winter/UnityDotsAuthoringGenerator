using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDotsAuthoringGenerator.Classes;
using System.Windows;

namespace UnityDotsAuthoringGenerator {
internal class DteHelper {
    public static DTE2 GetDte()
    {
        return AsyncPackage.GetGlobalService(typeof(SDTE)) as DTE2;
    }

    public static string GetSelectedPath()
    {
        ThreadHelper.ThrowIfNotOnUIThread();
        if (IsSolutionExplorerFocused()) {
            return GetSelectedSolutionFileDirectory();
        } else if (GetActiveDocument() != null) {
            return GetActiveDocument().Path;
        } else {
            return "";
        }
    }

    public static string GetSelectedFilePath()
    {
        ThreadHelper.ThrowIfNotOnUIThread();
        if (IsSolutionExplorerFocused()) {
            return GetSelectedSolutionFilePath();
        } else {
            return GetActiveDocument().FullName;
        }
    }

    public static string GetSelectedSolutionFileDirectory()
    {
        var filePath = GetSelectedSolutionFilePath();
        return Utils.GetAsDirectory(filePath);
    }

    public static string GetSelectedSolutionFilePath()
    {
        ThreadHelper.ThrowIfNotOnUIThread();

        UIHierarchy uih = GetDte().ToolWindows.SolutionExplorer;
        Array selectedItems = (Array)uih.SelectedItems;
        if (null != selectedItems) {
            foreach (UIHierarchyItem selItem in selectedItems) {
                var prjItem = selItem.Object as ProjectItem;
                string filePath = prjItem.Properties.Item("FullPath").Value.ToString();
                return filePath;
            }
        }
        return string.Empty;
    }

    public static Document GetActiveDocument()
    {
        ThreadHelper.ThrowIfNotOnUIThread();
        try {
            return GetDte().ActiveDocument;
        } catch (Exception ex) {

            MessageBox.Show(string.Format("Failed getting active document: ", ex.Message),
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return null;
        }
    }

    public static bool IsSolutionExplorerFocused()
    {
        ThreadHelper.ThrowIfNotOnUIThread();
        try {
            if (GetDte().ActiveWindow.ObjectKind.Equals(EnvDTE.Constants.vsWindowKindSolutionExplorer,
                    StringComparison.OrdinalIgnoreCase)) {
                return true;
            }
        } catch (Exception ex) {

            MessageBox.Show(string.Format("Failed checking if solution explorer is focused: ", ex.Message),
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        return false;
    }

    public static Project GetProject()
    {
        ThreadHelper.ThrowIfNotOnUIThread();

        UIHierarchy uih = GetDte().ToolWindows.SolutionExplorer;
        Array selectedItems = (Array)uih.SelectedItems;
        if (null != selectedItems) {
            foreach (UIHierarchyItem selItem in selectedItems) {
                if (selItem.Object is ProjectItem prjItem) {
                    return prjItem.ContainingProject;
                }
                if (selItem.Object is Project prj) {
                    return prj;
                }
            }
        }
        return null;
    }
}
}
