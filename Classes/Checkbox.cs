using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDotsAuthoringGenerator.Classes {

internal class Checkbox {
    public string Name { get; set; }

    public bool Checked
    {
        get {
            if (SettingsManager.Instance.TryGet(Name) != "True") {
                return false;
            }
            return true;
        }
        set {
            SettingsManager.Instance.Set(Name, value.ToString());
        }
    }

    public Checkbox(string name, bool defaultChecked)
    {
        Name = name;
        var isChecked = SettingsManager.Instance.TryGet(name);

        if (isChecked == "") {
            SettingsManager.Instance.Set(Name, defaultChecked.ToString());
            SettingsManager.Instance.SaveSettings();
        } else {
            Checked = bool.Parse(isChecked);
        }
    }
}
}
