using System;
using System.Collections;
using MelonLoader;
using PlagueButtonAPI.Controls.Base_Classes;
using PlagueButtonAPI.Controls.Grouping;
using PlagueButtonAPI.Pages;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlagueButtonAPI.Controls
{
    public class ToggleButton : Toggle
    {
        public ToggleButton(Transform parent, string text, string tooltipWhileDisabled, string tooltipWhileEnabled, Action<bool> stateChanged, Sprite OnImage = null, Sprite OffImage = null, bool DefaultState = false)
        {
            gameObject = Object.Instantiate(ButtonAPI.toggleButtonBase, parent);

            this.text.text = text;

            toggle.onValueChanged = new UnityEngine.UI.Toggle.ToggleEvent();

            toggle.onValueChanged.AddListener(new Action<bool>(val =>
            {
                if (AllowUserInvoke)
                {
                    stateChanged?.Invoke(val);
                }

                NextState = val;
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
                if (NextState != ToggleState) // The User Set This To Be A Different State While The Object Was Inactive
                {
                    SetToggleState(NextState, NextIsInvoke);
                }
            };
        }

        //Solely Made This For Users Moving From Other API's, As A CTRL H ReplaceAll Would Work.
        public ToggleButton(MenuPage pge, string text, string tooltip, Action<bool> stateChanged, bool DefaultState = false)
            : this(pge.menuContents, text, tooltip, tooltip, stateChanged, null, null, DefaultState)
        {
        }

        public ToggleButton(ButtonGroup pge, string text, string tooltip, Action<bool> stateChanged, bool DefaultState = false)
            : this(pge.gameObject.transform, text, tooltip, tooltip, stateChanged, null, null, DefaultState)
        {
        }

        public ToggleButton(CollapsibleButtonGroup pge, string text, string tooltip, Action<bool> stateChanged, bool DefaultState = false)
            : this(pge.buttonGroup, text, tooltip, tooltip, stateChanged, null, null, DefaultState)
        {
        }

        public ToggleButton(MenuPage pge, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite OnImage = null, Sprite OffImage = null)
            : this(pge.menuContents, text, offTooltip, onTooltip, stateChanged, OnImage, OffImage)
        {
        }

        public ToggleButton(ButtonGroup grp, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite OnImage = null, Sprite OffImage = null)
            : this(grp.gameObject.transform, text, offTooltip, onTooltip, stateChanged, OnImage, OffImage)
        {
        }

        public ToggleButton(CollapsibleButtonGroup grp, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite OnImage = null, Sprite OffImage = null)
            : this(grp.buttonGroup, text, offTooltip, onTooltip, stateChanged, OnImage, OffImage)
        {
        }

        public ToggleButton(MenuPage pge, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite OnImage, Sprite OffImage, bool DefaultState)
            : this(pge.menuContents, text, offTooltip, onTooltip, stateChanged, OnImage, OffImage, DefaultState)
        {
        }

        public ToggleButton(ButtonGroup grp, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite OnImage, Sprite OffImage, bool DefaultState)
            : this(grp.gameObject.transform, text, offTooltip, onTooltip, stateChanged, OnImage, OffImage, DefaultState)
        {
        }

        public ToggleButton(CollapsibleButtonGroup grp, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite OnImage, Sprite OffImage, bool DefaultState)
            : this(grp.buttonGroup, text, offTooltip, onTooltip, stateChanged, OnImage, OffImage, DefaultState)
        {
        }

        [Obsolete]
        public ToggleButton(MenuPage pge, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite OnImage)
            : this(pge.menuContents, text, offTooltip, onTooltip, stateChanged, OnImage, null)
        {
        }

        [Obsolete]
        public ToggleButton(ButtonGroup grp, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite OnImage)
            : this(grp.gameObject.transform, text, offTooltip, onTooltip, stateChanged, OnImage, null)
        {
        }

        [Obsolete]
        public ToggleButton(CollapsibleButtonGroup grp, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite OnImage)
            : this(grp.buttonGroup, text, offTooltip, onTooltip, stateChanged)
        {
        }

        [Obsolete]
        public ToggleButton(MenuPage pge, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite OnImage, bool DefaultState)
            : this(pge.menuContents, text, offTooltip, onTooltip, stateChanged, OnImage, null, DefaultState)
        {
        }

        [Obsolete]
        public ToggleButton(ButtonGroup grp, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite OnImage, bool DefaultState)
            : this(grp.gameObject.transform, text, offTooltip, onTooltip, stateChanged, OnImage, null, DefaultState)
        {
        }

        [Obsolete]
        public ToggleButton(CollapsibleButtonGroup grp, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite OnImage, bool DefaultState)
            : this(grp.buttonGroup, text, offTooltip, onTooltip, stateChanged, OnImage, DefaultState)
        {
        }
    }
}