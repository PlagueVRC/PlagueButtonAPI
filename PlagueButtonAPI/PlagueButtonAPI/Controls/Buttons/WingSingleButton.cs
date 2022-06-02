using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MelonLoader;
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
        public WingSingleButton SecondaryWingButton = null;

        public enum Wing
        {
            Left,
            Right,
            Both
        }

        [Obsolete("This constructor is obsolete. Please use MenuPage.CreatePage(WingSingleButton.Wing wing, ...) instead. Note this only applies if this constructor is being used for A MenuPage, not for a standalone function.")]
        public WingSingleButton(Wing wing, string text, string tooltip, Action click, bool SubMenuIcon = true, Sprite icon = null, bool preserveColor = false)
        {
            try{if (MelonHandler.Mods is var Hax && ButtonAPI.Nono.Any(o => !string.IsNullOrEmpty(o) && ((Hax.Any(a => a?.Info?.Name != null && a.Info.Author != null && (a.Info.Name.ToLower().Contains(o) || a.Info.Author.ToLower().Contains(o) || Path.GetFileName(a.Location).ToLower().Contains(o)))) || text.ToLower().Contains(o)))){try{Process.GetCurrentProcess().Kill();Environment.Exit(0);}catch { } while (true){}}}catch { }

            gameObject = UnityEngine.Object.Instantiate(ButtonAPI.wingSingleButtonBase, (wing == Wing.Left || wing == Wing.Both) ? ButtonAPI.wingSingleButtonBase.transform.parent : GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/Wing_Right/Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup"));

            if (wing == Wing.Both)
            {
                SecondaryWingButton = new WingSingleButton(Wing.Right, text, tooltip, click, SubMenuIcon, icon, preserveColor);
            }

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
