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
        private Dictionary<string, string> m_settings = new Dictionary<string, string>();

        private static SettingsManager m_instance;
        private SettingsManager() { 
            LoadSettings();
        }

        public static SettingsManager Instance {
            get
            {
                if (m_instance == null) {
                    m_instance = new SettingsManager();
                }
                return m_instance;
            }
        }

        public void Set(string key, string value) {
            m_settings[key]=value;
        }

        public string TryGet(string key) { 
            if( m_settings.TryGetValue(key, out var value)) {
                return value;
            }
            else {
                return "";
            }            
        }

        public void LoadSettings() {
            try {
                using (var isolatedStorage = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null)) {
                    if (isolatedStorage.FileExists(SettingsFileName)) {
                        using (var stream = new IsolatedStorageFileStream(SettingsFileName, FileMode.Open, isolatedStorage)) {
                            using (var reader = new StreamReader(stream)) {
                                var settingsData = reader.ReadToEnd();
                                m_settings = DeserializeSettings(settingsData);
                            }
                        }
                    }
                }
            }
            catch (IOException ex) {
                throw ex;
            }
            return;
        }

        public void SaveSettings() {
            try {
                var settingsData = SerializeSettings(m_settings);

                using (var isolatedStorage = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null)) {
                    using (var stream = new IsolatedStorageFileStream(SettingsFileName, FileMode.Create, isolatedStorage)) {
                        using (var writer = new StreamWriter(stream)) {
                            writer.Write(settingsData);
                        }
                    }
                }
            }
            catch (IOException ex) {
                throw ex;
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
