using System;
using PlagueButtonAPI.Controls.Base_Classes;
using PlagueButtonAPI.Controls.Grouping;
using PlagueButtonAPI.Pages;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlagueButtonAPI.Controls
{
    public class ToggleButton : Toggle
    {
        public ToggleButton(Transform parent, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite OnImage = null, Sprite OffImage = null)
        {
            gameObject = Object.Instantiate(ButtonAPI.toggleButtonBase, parent);

            this.text.text = text;

            toggle.onValueChanged = new UnityEngine.UI.Toggle.ToggleEvent();
            toggle.Set(false, false);

            if (stateChanged != null)
            {
                toggle.onValueChanged.AddListener(stateChanged);
            }

            if (!string.IsNullOrEmpty(onTooltip) && !string.IsNullOrEmpty(offTooltip))
            {
                tooltip.field_Public_String_0 = onTooltip;
                tooltip.field_Public_String_1 = offTooltip;
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

            var Init = true;

            var Handler = gameObject.AddComponent<ObjectHandler>();

            Handler.OnEnabled += obj =>
            {
                if (Init)
                {
                    SetToggleState(ToggleState);

                    Init = false;
                }
            };
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

        [Obsolete]
        public ToggleButton(MenuPage pge, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite OnImage)
            : this(pge.menuContents, text, offTooltip, onTooltip, stateChanged, OnImage)
        {
        }

        [Obsolete]
        public ToggleButton(ButtonGroup grp, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite OnImage)
            : this(grp.gameObject.transform, text, offTooltip, onTooltip, stateChanged, OnImage)
        {
        }

        [Obsolete]
        public ToggleButton(CollapsibleButtonGroup grp, string text, string offTooltip, string onTooltip, Action<bool> stateChanged, Sprite OnImage)
            : this(grp.buttonGroup, text, offTooltip, onTooltip, stateChanged, OnImage)
        {
        }
    }
}