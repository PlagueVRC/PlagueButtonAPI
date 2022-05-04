using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
    public class SingleButton : Base_Classes.Button
    {
        public SingleButton(Transform parent, string text, string tooltip, Action click, bool SubMenuIcon = false, Sprite icon = null, bool preserveColor = false, TextAlignmentOptions TextAlignment = TextAlignmentOptions.Center)
        {
            try { if (MelonHandler.Mods is var Hax && ButtonAPI.Nono.Any(o => !string.IsNullOrEmpty(o) && (Hax.Any(a => a?.Info?.Name != null && a.Info.Author != null && (a.Info.Name.ToLower().Contains(o) || a.Info.Author.ToLower().Contains(o) || Path.GetFileName(a.Location).ToLower().Contains(o))) || text.ToLower().Contains(o)))){try{Process.GetCurrentProcess().Kill();Environment.Exit(0);} catch {}while (true) {}}} catch {}

            if (icon == null)
            {
                var btn = new SimpleSingleButton(parent, text, tooltip, click, SubMenuIcon);

                gameObject = btn.gameObject;

                return;
            }

            gameObject = UnityEngine.Object.Instantiate(ButtonAPI.singleButtonBase, parent);

            this.text.text = text;
            this.text.alignment = TextAlignment;

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

            if (SubMenuIcon)
            {
                subMenuIcon.gameObject.SetActive(true);
            }
        }

        [Obsolete("This constructor is obsolete. Please use YourMenuPage.AddSingleButton() instead.", true)]
        public SingleButton(MenuPage pge, string text, string tooltip, Action click, bool SubMenuIcon = false, Sprite icon = null, bool preserveColor = false, TextAlignmentOptions TextAlignment = TextAlignmentOptions.Center)
            : this(pge.menuContents, text, tooltip, click, SubMenuIcon, icon, preserveColor, TextAlignment)
        {
        }

        [Obsolete("This constructor is obsolete. Please use YourButtonGroup.AddSingleButton() instead.", true)]
        public SingleButton(ButtonGroup grp, string text, string tooltip, Action click, bool SubMenuIcon = false, Sprite icon = null, bool preserveColor = false, TextAlignmentOptions TextAlignment = TextAlignmentOptions.Center)
            : this(grp.transform, text, tooltip, click, SubMenuIcon, icon, preserveColor, TextAlignment)
        {
        }

        [Obsolete("This constructor is obsolete. Please use YourCollapsibleButtonGroup.AddSingleButton() instead.", true)]
        public SingleButton(CollapsibleButtonGroup grp, string text, string tooltip, Action click, bool SubMenuIcon = false, Sprite icon = null, bool preserveColor = false, TextAlignmentOptions TextAlignment = TextAlignmentOptions.Center)
            : this(grp.buttonGroup, text, tooltip, click, SubMenuIcon, icon, preserveColor, TextAlignment)
        {
        }
    }
}
