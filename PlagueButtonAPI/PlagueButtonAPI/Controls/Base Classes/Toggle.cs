using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements.Tooltips;

namespace PlagueButtonAPI.Controls.Base_Classes
{
    public class Toggle : ControlWithText
    {
        public UnityEngine.UI.Toggle toggle => gameObject?.GetComponentInChildren<UnityEngine.UI.Toggle>(true);

        [Obsolete]
        public UnityEngine.UI.Toggle buttontoggle => toggle;

        public UiToggleTooltip tooltip => gameObject?.GetComponentInChildren<UiToggleTooltip>(true);

        public Image OnImage => transform?.Find("Icon_On")?.GetComponentInChildren<Image>(true);
        public Image OffImage => transform?.Find("Icon_Off")?.GetComponentInChildren<Image>(true);

        public bool ToggleState => toggle?.isOn ?? false;

        public void SetAction(Action<bool> newAction)
        {
            toggle.onValueChanged = new UnityEngine.UI.Toggle.ToggleEvent();
            toggle.onValueChanged.AddListener((Action<bool>)delegate (bool val)
            {
                newAction?.Invoke(val);
            });
        }

        public void SetInteractable(bool val)
        {
            toggle.interactable = val;
        }

        public void SetToggleState(bool newState, bool invoke = false)
        {
            var onValueChanged = toggle.onValueChanged;
            toggle.onValueChanged = new UnityEngine.UI.Toggle.ToggleEvent();
            toggle.isOn = newState;
            toggle.onValueChanged = onValueChanged;

            toggle.GetComponent<ToggleIcon>().Method_Private_Void_Boolean_PDM_0(newState);
            toggle.GetComponent<ToggleIcon>().Method_Private_Void_Boolean_PDM_1(newState);
            toggle.GetComponent<ToggleIcon>().Method_Private_Void_Boolean_PDM_2(newState);
            toggle.GetComponent<ToggleIcon>().Method_Private_Void_Boolean_PDM_3(newState);
            toggle.GetComponent<ToggleIcon>().Method_Private_Void_Boolean_PDM_4(newState);
            toggle.GetComponent<ToggleIcon>().Method_Private_Void_Boolean_PDM_5(newState);
            toggle.GetComponent<ToggleIcon>().Method_Private_Void_Boolean_PDM_6(newState);
            toggle.GetComponent<ToggleIcon>().Method_Private_Void_Boolean_PDM_7(newState);
            toggle.GetComponent<ToggleIcon>().Method_Private_Void_Boolean_PDM_8(newState);
            toggle.GetComponent<ToggleIcon>().Method_Private_Void_Boolean_PDM_9(newState);
            toggle.GetComponent<ToggleIcon>().Method_Private_Void_Boolean_PDM_10(newState);

            if (tooltip != null)
            {
                tooltip.field_Private_Boolean_1 = !newState;
            }

            if (invoke)
            {
                toggle.onValueChanged.Invoke(newState);
            }
        }

        public void SetTooltip(string newOffTooltip, string newOnTooltip)
        {
            tooltip.field_Public_String_0 = newOnTooltip;
            tooltip.field_Public_String_1 = newOffTooltip;
        }

        public void SetOnIcon(Sprite newIcon)
        {
            if (newIcon == null)
            {
                OnImage.gameObject.SetActive(false);
                return;
            }

            OnImage.sprite = newIcon;
            OnImage.overrideSprite = newIcon;
            OnImage.gameObject.SetActive(true);
        }

        public void SetOffIcon(Sprite newIcon)
        {
            if (newIcon == null)
            {
                OffImage.gameObject.SetActive(false);
                return;
            }

            OffImage.sprite = newIcon;
            OffImage.overrideSprite = newIcon;
            OffImage.gameObject.SetActive(true);
        }
    }
}
