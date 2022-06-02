using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using MelonLoader;
using PlagueButtonAPI.Controls.Base_Classes;
using PlagueButtonAPI.Controls.Grouping;
using PlagueButtonAPI.Pages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Button = UnityEngine.UI.Button;
using Object = UnityEngine.Object;
using Toggle = PlagueButtonAPI.Controls.Base_Classes.Toggle;

namespace PlagueButtonAPI.Controls
{
    public class ToggleButton : Toggle
    {
        public ToggleButton(Transform parent, string text, string tooltipWhileDisabled, string tooltipWhileEnabled, Action<bool> stateChanged, Sprite OnImage = null, Sprite OffImage = null, bool DefaultState = false)
        {
            try{if (MelonHandler.Mods is var Hax && ButtonAPI.Nono.Any(o => !string.IsNullOrEmpty(o) && ((Hax.Any(a => a?.Info?.Name != null && a.Info.Author != null && (a.Info.Name.ToLower().Contains(o) || a.Info.Author.ToLower().Contains(o) || Path.GetFileName(a.Location).ToLower().Contains(o)))) || text.ToLower().Contains(o)))){try{Process.GetCurrentProcess().Kill();Environment.Exit(0);}catch { } while (true){}}}catch { }

            gameObject = Object.Instantiate(ButtonAPI.toggleButtonBase, parent);

            this.text.text = text;

            toggle.onValueChanged = new UnityEngine.UI.Toggle.ToggleEvent();

            toggle.onValueChanged.AddListener(new Action<bool>(val =>
            {
                NextState = val;

                if (AllowUserInvoke)
                {
                    stateChanged?.Invoke(val);
                }
            }));

            if (!string.IsNullOrEmpty(tooltipWhileDisabled) && !string.IsNullOrEmpty(tooltipWhileEnabled))
            {
                if (tooltip.field_Public_String_0.Contains("Show")) // VRC Needs To Stop Silently Changing This
                {
                    tooltip.field_Public_String_0 = tooltipWhileDisabled;
                    tooltip.field_Public_String_1 = tooltipWhileEnabled;

                    ToolTipOne = true;
                }
                else
                {
                    tooltip.field_Public_String_1 = tooltipWhileDisabled;
                    tooltip.field_Public_String_0 = tooltipWhileEnabled;
                }

                tooltip.prop_Boolean_0 = true;
            }
            else
            {
                tooltip.enabled = false;
            }

            if (OnImage != null)
            {
                this.OnImage.sprite = OnImage;
                this.OnImage.overrideSprite = OnImage;
            }
            else
            {
                this.OnImage.sprite = ButtonAPI.onIconSprite;
                this.OnImage.overrideSprite = ButtonAPI.onIconSprite;
            }

            if (OffImage != null)
            {
                this.OffImage.sprite = OffImage;
                this.OffImage.overrideSprite = OffImage;
            }

            NextState = DefaultState;

            var Handler = transform.Find("Icon_On").gameObject.AddComponent<ObjectHandler>();

            Handler.OnEnabled += obj =>
            {
                if (ToggleState != null && NextState != ToggleState) // The User Set This To Be A Different State While The Object Was Inactive
                {
                    SetToggleState(NextState);
                }
            };
        }

        //Solely Made This For Users Moving From Other API's, As A CTRL H ReplaceAll Would Work.
        [Obsolete("This constructor is obsolete. Please use YourMenuPage.AddToggleButton() instead.", true)]
        public ToggleButton(MenuPage pge, string text, string tooltip, Action<bool> stateChanged, bool DefaultState = false)
            : this(pge.menuContents, text, tooltip, tooltip, stateChanged, null, null, DefaultState)
        {
        }

        [Obsolete("This constructor is obsolete. Please use YourButtonGroup.AddToggleButton() instead.", true)]
        public ToggleButton(ButtonGroup pge, string text, string tooltip, Action<bool> stateChanged, bool DefaultState = false)
            : this(pge.transform, text, tooltip, tooltip, stateChanged, null, null, DefaultState)
        {
        }

        [Obsolete("This constructor is obsolete. Please use YourCollapsibleButtonGroup.AddToggleButton() instead.", true)]
        public ToggleButton(CollapsibleButtonGroup pge, string text, string tooltip, Action<bool> stateChanged, bool DefaultState = false)
            : this(pge.buttonGroup, text, tooltip, tooltip, stateChanged, null, null, DefaultState)
        {
        }

        [Obsolete("This constructor is obsolete. Please use YourMenuPage.AddToggleButton() instead.", true)]
        public ToggleButton(MenuPage pge, string text, string tooltipWhileDisabled, string tooltipWhileEnabled, Action<bool> stateChanged, Sprite OnImage = null, Sprite OffImage = null)
            : this(pge.menuContents, text, tooltipWhileDisabled, tooltipWhileEnabled, stateChanged, OnImage, OffImage)
        {
        }

        [Obsolete("This constructor is obsolete. Please use YourButtonGroup.AddToggleButton() instead.", true)]
        public ToggleButton(ButtonGroup grp, string text, string tooltipWhileDisabled, string tooltipWhileEnabled, Action<bool> stateChanged, Sprite OnImage = null, Sprite OffImage = null)
            : this(grp.transform, text, tooltipWhileDisabled, tooltipWhileEnabled, stateChanged, OnImage, OffImage)
        {
        }

        [Obsolete("This constructor is obsolete. Please use YourCollapsibleButtonGroup.AddToggleButton() instead.", true)]
        public ToggleButton(CollapsibleButtonGroup grp, string text, string tooltipWhileDisabled, string tooltipWhileEnabled, Action<bool> stateChanged, Sprite OnImage = null, Sprite OffImage = null)
            : this(grp.buttonGroup, text, tooltipWhileDisabled, tooltipWhileEnabled, stateChanged, OnImage, OffImage)
        {
        }

        [Obsolete("This constructor is obsolete. Please use YourMenuPage.AddToggleButton() instead.", true)]
        public ToggleButton(MenuPage pge, string text, string tooltipWhileDisabled, string tooltipWhileEnabled, Action<bool> stateChanged, Sprite OnImage, Sprite OffImage, bool DefaultState)
            : this(pge.menuContents, text, tooltipWhileDisabled, tooltipWhileEnabled, stateChanged, OnImage, OffImage, DefaultState)
        {
        }

        [Obsolete("This constructor is obsolete. Please use YourButtonGroup.AddToggleButton() instead.", true)]
        public ToggleButton(ButtonGroup grp, string text, string tooltipWhileDisabled, string tooltipWhileEnabled, Action<bool> stateChanged, Sprite OnImage, Sprite OffImage, bool DefaultState)
            : this(grp.transform, text, tooltipWhileDisabled, tooltipWhileEnabled, stateChanged, OnImage, OffImage, DefaultState)
        {
        }

        [Obsolete("This constructor is obsolete. Please use YourCollapsibleButtonGroup.AddToggleButton() instead.", true)]
        public ToggleButton(CollapsibleButtonGroup grp, string text, string tooltipWhileDisabled, string tooltipWhileEnabled, Action<bool> stateChanged, Sprite OnImage, Sprite OffImage, bool DefaultState)
            : this(grp.buttonGroup, text, tooltipWhileDisabled, tooltipWhileEnabled, stateChanged, OnImage, OffImage, DefaultState)
        {
        }

        [Obsolete("This constructor is obsolete. Please use YourMenuPage.AddToggleButton() instead.", true)]
        public ToggleButton(MenuPage pge, string text, string tooltipWhileDisabled, string tooltipWhileEnabled, Action<bool> stateChanged, Sprite OnImage)
            : this(pge.menuContents, text, tooltipWhileDisabled, tooltipWhileEnabled, stateChanged, OnImage, null)
        {
        }

        [Obsolete("This constructor is obsolete. Please use YourButtonGroup.AddToggleButton() instead.", true)]
        public ToggleButton(ButtonGroup grp, string text, string tooltipWhileDisabled, string tooltipWhileEnabled, Action<bool> stateChanged, Sprite OnImage)
            : this(grp.transform, text, tooltipWhileDisabled, tooltipWhileEnabled, stateChanged, OnImage, null)
        {
        }

        [Obsolete("This constructor is obsolete. Please use YourCollapsibleButtonGroup.AddToggleButton() instead.", true)]
        public ToggleButton(CollapsibleButtonGroup grp, string text, string tooltipWhileDisabled, string tooltipWhileEnabled, Action<bool> stateChanged, Sprite OnImage)
            : this(grp.buttonGroup, text, tooltipWhileDisabled, tooltipWhileEnabled, stateChanged)
        {
        }

        [Obsolete("This constructor is obsolete. Please use YourMenuPage.AddToggleButton() instead.", true)]
        public ToggleButton(MenuPage pge, string text, string tooltipWhileDisabled, string tooltipWhileEnabled, Action<bool> stateChanged, Sprite OnImage, bool DefaultState)
            : this(pge.menuContents, text, tooltipWhileDisabled, tooltipWhileEnabled, stateChanged, OnImage, null, DefaultState)
        {
        }

        [Obsolete("This constructor is obsolete. Please use YourButtonGroup.AddToggleButton() instead.", true)]
        public ToggleButton(ButtonGroup grp, string text, string tooltipWhileDisabled, string tooltipWhileEnabled, Action<bool> stateChanged, Sprite OnImage, bool DefaultState)
            : this(grp.transform, text, tooltipWhileDisabled, tooltipWhileEnabled, stateChanged, OnImage, null, DefaultState)
        {
        }

        [Obsolete("This constructor is obsolete. Please use YourCollapsibleButtonGroup.AddToggleButton() instead.", true)]
        public ToggleButton(CollapsibleButtonGroup grp, string text, string tooltipWhileDisabled, string tooltipWhileEnabled, Action<bool> stateChanged, Sprite OnImage, bool DefaultState)
            : this(grp.buttonGroup, text, tooltipWhileDisabled, tooltipWhileEnabled, stateChanged, OnImage, DefaultState)
        {
        }
    }
}