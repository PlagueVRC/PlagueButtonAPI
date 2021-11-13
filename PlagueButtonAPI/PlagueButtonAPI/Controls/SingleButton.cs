using System;
using PlagueButtonAPI.Misc;
using PlagueButtonAPI.Controls.Grouping;
using PlagueButtonAPI.Pages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;

namespace PlagueButtonAPI.Controls
{
    public class SingleButton
    {
        public readonly TextMeshProUGUI buttonText;

        public readonly Image buttonImage;

        public readonly Button buttonButton;

        public readonly VRC.UI.Elements.Tooltips.UiTooltip buttonTooltip;

        public readonly GameObject gameObject;

        public SingleButton(Transform parent, string text, string tooltip, Action click, bool SubMenuIcon = false, Sprite icon = null, bool preserveColor = false)
        {
            gameObject = UnityEngine.Object.Instantiate(ButtonAPI.singleButtonBase, parent);
            buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            buttonText.text = text;
            buttonButton = gameObject.GetComponentInChildren<Button>(true);
            buttonButton.onClick = new Button.ButtonClickedEvent();
            if (click != null)
            {
                buttonButton.onClick.AddListener(click);
            }
            buttonTooltip = gameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>(true);
            if (!string.IsNullOrEmpty(tooltip))
            {
                buttonTooltip.field_Public_String_0 = tooltip;
            }
            else
            {
                buttonTooltip.enabled = false;
            }
            buttonImage = gameObject.transform.Find("Icon").GetComponentInChildren<Image>(true);
            if (icon != null)
            {
                buttonImage.sprite = icon;
                buttonImage.overrideSprite = icon;
                buttonImage.gameObject.SetActive(true);
                if (preserveColor)
                {
                    buttonImage.color = Color.white;
                    buttonImage.gameObject.GetOrAddComponent<StyleElement>().enabled = false;
                }
            }
            else
            {
                buttonImage.gameObject.SetActive(false);
            }

            if (SubMenuIcon)
            {
                gameObject.transform.Find("Badge_MMJump").gameObject.SetActive(true);
            }
        }

        public SingleButton(MenuPage pge, string text, string tooltip, Action click, bool SubMenuIcon = false, Sprite icon = null, bool preserveColor = false)
            : this(pge.menuContents, text, tooltip, click, SubMenuIcon, icon, preserveColor)
        {
        }

        public SingleButton(ButtonGroup grp, string text, string tooltip, Action click, bool SubMenuIcon = false, Sprite icon = null, bool preserveColor = false)
            : this(grp.gameObject.transform, text, tooltip, click, SubMenuIcon, icon, preserveColor)
        {
        }

        public SingleButton(CollapsibleButtonGroup grp, string text, string tooltip, Action click, bool SubMenuIcon = false, Sprite icon = null, bool preserveColor = false)
            : this(grp.buttonGroup, text, tooltip, click, SubMenuIcon, icon, preserveColor)
        {
        }

        public void SetAction(Action newAction)
        {
            buttonButton.onClick = new Button.ButtonClickedEvent();
            buttonButton.onClick.AddListener(newAction);
        }

        public void SetText(string newText)
        {
            buttonText.text = newText;
        }

        public void SetTooltip(string newTooltip)
        {
            buttonTooltip.field_Public_String_0 = newTooltip;
        }

        public void SetIcon(Sprite newIcon, bool preserveColor = false)
        {
            if (newIcon == null)
            {
                buttonImage.gameObject.SetActive(false);
                return;
            }
            buttonImage.sprite = newIcon;
            buttonImage.overrideSprite = newIcon;
            buttonImage.gameObject.SetActive(true);
            if (preserveColor)
            {
                buttonImage.color = Color.white;
            }
        }

        public void SetIconColor(Color color)
        {
            buttonImage.color = color;
        }

        public void SetInteractable(bool val)
        {
            buttonButton.interactable = val;
        }

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}
