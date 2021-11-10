using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlagueButtonAPI.Utils
{
    public class TextButton
    {
        private TextMeshProUGUI buttonText;

        private TextMeshProUGUI buttonTextBig;

        private Button buttonButton;

        private VRC.UI.Elements.Tooltips.UiTooltip buttonTooltip;

        public GameObject gameObject;

        public TextButton(Transform parent, string text, string tooltip, string bigText, Action click)
        {
            gameObject = UnityEngine.Object.Instantiate(ButtonAPI.singleButtonBase, parent);
            buttonText = gameObject.GetComponentsInChildren<TextMeshProUGUI>(true).First();
            buttonText.text = text;
            buttonTextBig = gameObject.GetComponentsInChildren<TextMeshProUGUI>(true).Last();
            buttonTextBig.text = bigText;
            buttonButton = gameObject.GetComponentInChildren<Button>(true);
            buttonButton.onClick = new Button.ButtonClickedEvent();
            if (click != null)
            {
                buttonButton.onClick.AddListener(click);
            }
            buttonTooltip = gameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>(true);
            buttonTooltip.field_Public_String_0 = tooltip;
        }

        public TextButton(MenuPage pge, string text, string tooltip, string bigText, Action click)
            : this(pge.menuContents, text, tooltip, bigText, click)
        {
        }

        public TextButton(ButtonGroup grp, string text, string tooltip, string bigText, Action click)
            : this(grp.gameObject.transform, text, tooltip, bigText, click)
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

        public void SetBigText(string newText)
        {
            buttonTextBig.text = newText;
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
