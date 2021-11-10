using System;
using PlagueButtonAPI.Controls.Grouping;
using PlagueButtonAPI.Pages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlagueButtonAPI.Controls
{
    public class SimpleSingleButton
    {
        private TextMeshProUGUI buttonText;

        private Button buttonButton;

        private VRC.UI.Elements.Tooltips.UiTooltip buttonTooltip;

        public GameObject gameObject;

        public SimpleSingleButton(Transform parent, string text, string tooltip, Action click)
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
            buttonTooltip.field_Public_String_0 = tooltip;
            UnityEngine.Object.Destroy(gameObject.transform.Find("Icon").gameObject);
            UnityEngine.Object.Destroy(gameObject.transform.Find("Icon_Secondary").gameObject);
            buttonText.color = new Color(0.9f, 0.9f, 0.9f);
        }

        public SimpleSingleButton(MenuPage pge, string text, string tooltip, Action click)
            : this(pge.menuContents, text, tooltip, click)
        {
        }

        public SimpleSingleButton(ButtonGroup grp, string text, string tooltip, Action click)
            : this(grp.gameObject.transform, tooltip, text, click)
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
