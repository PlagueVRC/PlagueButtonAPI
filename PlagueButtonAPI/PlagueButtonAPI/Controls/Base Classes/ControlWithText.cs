using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace PlagueButtonAPI.Controls.Base_Classes
{
    public class ControlWithText : GenericControl
    {
        public Image buttonBackground => transform?.Find("Background")?.GetComponentInChildren<Image>(true) ?? transform?.Find("Container/Background")?.GetComponentInChildren<Image>(true);

        public TMPro.TextMeshProUGUI text => gameObject?.GetComponentInChildren<TMPro.TextMeshProUGUI>(true);

        [Obsolete]
        public TMPro.TextMeshProUGUI buttonText => text;

        public void SetText(string newText)
        {
            text.text = newText;
        }
    }
}
