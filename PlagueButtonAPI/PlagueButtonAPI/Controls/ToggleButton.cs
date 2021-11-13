using PlagueButtonAPI.Misc;
using PlagueButtonAPI.Controls.Grouping;
using PlagueButtonAPI.Pages;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements.Tooltips;
using Object = UnityEngine.Object;

namespace PlagueButtonAPI.Controls
{
    public class ToggleButton
    {
        public readonly TextMeshProUGUI buttonText;

        public readonly Image buttonImage;

        public readonly Toggle buttonToggle;

        public readonly UiToggleTooltip toggleTooltip;

        public readonly GameObject gameObject;

        public bool ToggleState;

        public ToggleButton(Transform parent, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite icon = null)
        {
            gameObject = UnityEngine.Object.Instantiate(ButtonAPI.toggleButtonBase, parent);
            buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            buttonText.text = text;
            buttonToggle = gameObject.GetComponentInChildren<Toggle>(true);
            buttonToggle.onValueChanged = new Toggle.ToggleEvent();
            buttonToggle.isOn = false;

            if (stateChanged != null)
            {
                buttonToggle.onValueChanged.AddListener(stateChanged);
            }

            toggleTooltip = gameObject.GetComponentInChildren<UiToggleTooltip>(true);

            if (!string.IsNullOrEmpty(onTooltip) && !string.IsNullOrEmpty(offTooltip))
            {
                toggleTooltip.field_Public_String_0 = onTooltip;
                toggleTooltip.field_Public_String_1 = offTooltip;
                toggleTooltip.prop_Boolean_0 = true;
            }
            else
            {
                toggleTooltip.enabled = false;
            }

            buttonImage = gameObject.transform.Find("Icon_On").GetComponentInChildren<Image>(true);

            if (icon != null)
            {
                buttonImage.sprite = icon;
                buttonImage.overrideSprite = icon;
            }
            else
            {
                buttonImage.sprite = ButtonAPI.onIconSprite;
                buttonImage.overrideSprite = ButtonAPI.onIconSprite;
            }

            var Handler = buttonToggle.gameObject.AddComponent<ObjectHandler>();

            Handler.OnEnabled += (obj) =>
            {
                SetToggleState(ToggleState, true);

                Object.Destroy(Handler);
            };
        }

        public ToggleButton(MenuPage pge, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite icon = null)
            : this(pge.menuContents, text, offTooltip, onTooltip, stateChanged, icon)
        {
        }

        public ToggleButton(ButtonGroup grp, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite icon = null)
            : this(grp.gameObject.transform, text, offTooltip, onTooltip, stateChanged, icon)
        {
        }

        public ToggleButton(CollapsibleButtonGroup grp, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite icon = null)
            : this(grp.buttonGroup, text, offTooltip, onTooltip, stateChanged, icon)
        {
        }

        public void SetAction(Action<bool> newAction)
        {
            buttonToggle.onValueChanged = new Toggle.ToggleEvent();
            buttonToggle.onValueChanged.AddListener((Action<bool>)delegate (bool val)
            {
                newAction?.Invoke(val);
            });
        }

        public void SetText(string newText)
        {
            buttonText.text = newText;
        }

        public void SetTooltip(string newOffTooltip, string newOnTooltip)
        {
            toggleTooltip.field_Public_String_0 = newOnTooltip;
            toggleTooltip.field_Public_String_1 = newOffTooltip;
        }

        public void SetIcon(Sprite newIcon)
        {
            if (newIcon == null)
            {
                buttonImage.gameObject.SetActive(false);
                return;
            }
            buttonImage.sprite = newIcon;
            buttonImage.overrideSprite = newIcon;
            buttonImage.gameObject.SetActive(true);
        }

        public void SetToggleState(bool newState, bool invoke = false)
        {
            Toggle.ToggleEvent onValueChanged = buttonToggle.onValueChanged;
            buttonToggle.onValueChanged = new Toggle.ToggleEvent();
            buttonToggle.isOn = newState;
            buttonToggle.onValueChanged = onValueChanged;
            buttonToggle.gameObject.GetOrAddComponent<ToggleIcon>().Method_Private_Void_Boolean_PDM_0(newState);
            buttonToggle.gameObject.GetOrAddComponent<ToggleIcon>().Method_Private_Void_Boolean_PDM_1(newState);
            buttonToggle.gameObject.GetOrAddComponent<ToggleIcon>().Method_Private_Void_Boolean_PDM_2(newState);
            buttonToggle.gameObject.GetOrAddComponent<ToggleIcon>().Method_Private_Void_Boolean_PDM_3(newState);
            if (invoke)
            {
                buttonToggle.onValueChanged.Invoke(newState);
            }

            ToggleState = newState;
        }

        public void SetInteractable(bool val)
        {
            buttonToggle.interactable = val;
        }

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}
