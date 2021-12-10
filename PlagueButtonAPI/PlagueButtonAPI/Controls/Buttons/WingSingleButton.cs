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
    public class WingSingleButton : Base_Classes.Button
    {
        public enum Wing
        {
            Left,
            Right
        }

        public WingSingleButton(Wing wing, string text, string tooltip, Action click, bool SubMenuIcon = true, Sprite icon = null, bool preserveColor = false)
        {
            gameObject = UnityEngine.Object.Instantiate(ButtonAPI.wingSingleButtonBase, wing == Wing.Left ? ButtonAPI.wingSingleButtonBase.transform.parent : GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/Wing_Right/Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup"));

            this.text.text = text;

            button.onClick = new Button.ButtonClickedEvent();

            if (click != null)
            {
                button.onClick.AddListener(click);
            }

            if (!string.IsNullOrEmpty(tooltip))
            {
                this.tooltip.field_Public_String_0 = tooltip;
            }
            else
            {
                this.tooltip.enabled = false;
            }

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

            subMenuIcon.gameObject.SetActive(SubMenuIcon);

            gameObject.SetActive(true);
        }
    }
}
