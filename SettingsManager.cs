using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDotsAuthoringGenerator
{
    internal class SettingsManager
    {
        private const string SettingsFileName = "DotsGeneratorSettings.txt";
        private Dictionary<string, string> settings = new Dictionary<string, string>();

        private static SettingsManager instance;
        private SettingsManager() { 
            LoadSettings();
        }

        public static SettingsManager Instance {
            get
            {
                if (instance == null) {
                    instance = new SettingsManager();
                }
                return instance;
            }
        }

        public void Set(string key, string value) {
            settings[key]=value;
        }

        public string TryGet(string key) { 
            if( settings.TryGetValue(key, out var value)) {
                return value;
            }
            else {
                return "";
            }            
        }

        public Dictionary<string, string> LoadSettings() {
            try {
                using (var isolatedStorage = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null)) {
                    if (isolatedStorage.FileExists(SettingsFileName)) {
                        using (var stream = new IsolatedStorageFileStream(SettingsFileName, FileMode.Open, isolatedStorage)) {
                            using (var reader = new StreamReader(stream)) {
                                var settingsData = reader.ReadToEnd();
                                return DeserializeSettings(settingsData);
                            }
                        }
                    }
                }
            }
            catch (IOException ex) {
                // Handle IOException if necessary (e.g., log, throw, etc.).
            }

            return new Dictionary<string, string>(); // Return an empty dictionary if settings file doesn't exist or there was an error.
        }

        public void SaveSettings(Dictionary<string, string> settings) {
            try {
                var settingsData = SerializeSettings(settings);

                using (var isolatedStorage = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null)) {
                    using (var stream = new IsolatedStorageFileStream(SettingsFileName, FileMode.Create, isolatedStorage)) {
                        using (var writer = new StreamWriter(stream)) {
                            writer.Write(settingsData);
                        }
                    }
                }
            }
            catch (IOException ex) {
                // Handle IOException if necessary (e.g., log, throw, etc.).
            }
        }

        private string SerializeSettings(Dictionary<string, string> settings) {
            // Convert the dictionary to a serialized string (e.g., JSON).
            // You can use any serialization method of your choice.
            // For simplicity, we'll use a simple custom format.

            var settingsData = string.Empty;

            foreach (var kvp in settings) {
                settingsData += $"{kvp.Key}={kvp.Value}\n";
            }

            return settingsData;
        }

        private Dictionary<string, string> DeserializeSettings(string settingsData) {
            var settings = new Dictionary<string, string>();

            // Convert the serialized string (e.g., JSON) back to the dictionary.
            // For simplicity, we'll use a simple custom format.

            var lines = settingsData.Split('\n');

            foreach (var line in lines) {
                var keyValue = line.Split('=');
                if (keyValue.Length == 2) {
                    settings[keyValue[0]] = keyValue[1];
                }
            }

            return settings;
        }
    }
}
