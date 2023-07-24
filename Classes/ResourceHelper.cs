using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

public class ResourceHelper {
    public static string ReadResourceFile(string resourceName)
    {
        if (!resourceName.StartsWith("UnityDotsAuthoringGenerator")) {
            resourceName = "UnityDotsAuthoringGenerator." + resourceName;
        }
        Assembly assembly = Assembly.GetExecutingAssembly();
        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null) {
                throw new ArgumentException($"Resource '{resourceName}' not found in the assembly.");
            }

            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }

    public static List<(string name, string content)> GetResourcesFromFolder(string folderPath)
    {
        var contents = new List<(string name, string content)>();

        if (!folderPath.StartsWith("UnityDotsAuthoringGenerator")) {
            folderPath = "UnityDotsAuthoringGenerator." + folderPath;
        }
        Assembly assembly = Assembly.GetExecutingAssembly();
        string[] allResourceNames = assembly.GetManifestResourceNames();

        // Filter resources that belong to the specified folder
        List<string> folderResources = allResourceNames
                                           .Where(resourceName => resourceName.StartsWith(folderPath))
                                           .ToList();

        foreach (string resourcePath in folderResources) {
            var resourceName = Path.GetFileName(resourcePath);
            var fileName = resourceName.Replace(folderPath + ".", "");
            contents.Add((fileName, ReadResourceFile(resourcePath)));
        }

        return contents;
    }
}
