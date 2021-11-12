using PlagueButtonAPI.Controls.Grouping;
using PlagueButtonAPI.Pages;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlagueButtonAPI.Controls
{
    public class SimpleSingleButton
    {
        public readonly TextMeshProUGUI buttonText;

        public readonly Button buttonButton;

        public readonly VRC.UI.Elements.Tooltips.UiTooltip buttonTooltip;

        public readonly GameObject gameObject;

        public SimpleSingleButton(Transform parent, string text, string tooltip, Action click, bool SubMenuIcon = false)
        {
            gameObject = UnityEngine.Object.Instantiate(ButtonAPI.singleButtonBase, parent);
            buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            buttonText.text = text;
            buttonText.fontSize = 28f;
            buttonText.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, -25f, 0f);
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
            UnityEngine.Object.Destroy(gameObject.transform.Find("Icon").gameObject);
            UnityEngine.Object.Destroy(gameObject.transform.Find("Icon_Secondary").gameObject);
            buttonText.color = new Color(0.9f, 0.9f, 0.9f);

            if (SubMenuIcon)
            {
                gameObject.transform.Find("Badge_MMJump").gameObject.SetActive(true);
            }
        }

        public SimpleSingleButton(MenuPage pge, string text, string tooltip, Action click, bool SubMenuIcon = false)
            : this(pge.menuContents, text, tooltip, click, SubMenuIcon)
        {
        }

        public SimpleSingleButton(ButtonGroup grp, string text, string tooltip, Action click, bool SubMenuIcon = false)
            : this(grp.gameObject.transform, tooltip, text, click, SubMenuIcon)
        {
        }

        public SimpleSingleButton(CollapsibleButtonGroup grp, string text, string tooltip, Action click, bool SubMenuIcon = false)
            : this(grp.buttonGroup, tooltip, text, click, SubMenuIcon)
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
