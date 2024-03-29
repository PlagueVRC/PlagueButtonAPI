﻿using System;
using System.Collections;
using MelonLoader;
using PlagueButtonAPI.Controls.Grouping;
using PlagueButtonAPI.Misc;
using PlagueButtonAPI.Pages;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;

namespace PlagueButtonAPI.Controls
{
    public class Label
    {
        public readonly SimpleSingleButton LabelButton;

        public Label(Transform parent, string text, string tooltip, Action onClick = null)
        {
            LabelButton = new SimpleSingleButton(parent, text, tooltip, onClick);

            LabelButton.buttonBackground.color = new Color(0f, 0f, 0f, 0f);

            var Handler = LabelButton.gameObject.AddComponent<ObjectHandler>();

            Handler.OnUpdateEachSecond += (a, b) =>
            {
                LabelButton.text.transform.localPosition = new Vector3(0f, -19f, 0f);
            };

            if (onClick == null)
            {
                LabelButton.gameObject.GetOrAddComponent<Button>().enabled = false;
            }
        }

        [Obsolete("This constructor is obsolete. Please use YourMenuPage.AddLabel() instead.", true)]
        public Label(MenuPage pge, string text, string tooltip, Action onClick = null)
            : this(pge.menuContents, text, tooltip, onClick)
        {
        }

        [Obsolete("This constructor is obsolete. Please use YourButtonGroup.AddLabel() instead.", true)]
        public Label(ButtonGroup grp, string text, string tooltip, Action onClick = null)
            : this(grp.transform, text, tooltip, onClick)
        {
        }

        [Obsolete("This constructor is obsolete. Please use YourCollapsibleButtonGroup.AddLabel() instead.", true)]
        public Label(CollapsibleButtonGroup grp, string text, string tooltip, Action onClick = null)
            : this(grp.buttonGroup, text, tooltip, onClick)
        {
        }
    }
}