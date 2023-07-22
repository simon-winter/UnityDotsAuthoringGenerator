using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDotsAuthoringGenerator
{
    internal class DteHelper
    {
        public static DTE2 GetDte() {
            return AsyncPackage.GetGlobalService(typeof(SDTE)) as DTE2;          
        }

        public static string GetSelectedFilePath() {
            ThreadHelper.ThrowIfNotOnUIThread();

            UIHierarchy uih = GetDte().ToolWindows.SolutionExplorer;
            Array selectedItems = (Array)uih.SelectedItems;
            ProjectItem prjItem = null;
            if (null != selectedItems) {
                foreach (UIHierarchyItem selItem in selectedItems) {
                    prjItem = selItem.Object as ProjectItem;
                    string filePath = prjItem.Properties.Item("FullPath").Value.ToString();
                    //System.Windows.Forms.MessageBox.Show(selItem.Name + filePath);
                    return filePath;
                }
            }
            return string.Empty;
        }

        public static Project GetProject() {
            ThreadHelper.ThrowIfNotOnUIThread();

            UIHierarchy uih = GetDte().ToolWindows.SolutionExplorer;
            Array selectedItems = (Array)uih.SelectedItems;
            ProjectItem prjItem = null;
            if (null != selectedItems) {
                foreach (UIHierarchyItem selItem in selectedItems) {
                    prjItem = selItem.Object as ProjectItem;                   
                    return prjItem.ContainingProject;
                }
            }
            return null;
        }
    }
}
