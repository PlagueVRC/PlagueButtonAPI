﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace PlagueButtonAPI.Controls.Base_Classes
{
    public class Button : ControlWithText
    {
        public Image subMenuIcon => transform?.Find("Badge_MMJump")?.GetComponentInChildren<Image>(true) ?? transform?.Find("Container/Icon_Arrow")?.GetComponentInChildren<Image>(true);

        public Image buttonImage => transform?.Find("Icon")?.GetComponentInChildren<Image>(true) ?? transform?.Find("Container/Icon")?.GetComponentInChildren<Image>(true);

        public UnityEngine.UI.Button button => gameObject?.GetComponentInChildren<UnityEngine.UI.Button>(true);

        public VRC.UI.Elements.Tooltips.UiTooltip tooltip => gameObject?.GetComponentsInChildren<VRC.UI.Elements.Tooltips.UiTooltip>(true).FirstOrDefault(o => !string.IsNullOrEmpty(o.field_Public_String_0) || !string.IsNullOrEmpty(o.field_Public_String_1));

        public void SetAction(Action newAction)
        {
            button.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            button.onClick.AddListener(newAction);
        }

        public void SetInteractable(bool val)
        {
            button.interactable = val;
        }

        public void SetIcon(Sprite newIcon, bool preserveColor = false)
        {
            if (newIcon == null)
            {
                buttonImage.gameObject.SetActive(false);
                return;
            }
            buttonImage.sprite = newIcon;
            buttonImage.overrideSprite = newIcon;
            buttonImage.gameObject.SetActive(true);
            if (preserveColor)
            {
                buttonImage.color = Color.white;
            }
        }

        public void SetIconColor(Color color)
        {
            buttonImage.color = color;
        }

        public void SetTooltip(string newTooltip)
        {
            if (tooltip == null)
            {
                return;
            }

            tooltip.field_Public_String_0 = newTooltip;
        }
    }
}