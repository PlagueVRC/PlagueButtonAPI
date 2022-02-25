using System;
using System.Collections;
using System.Linq;
using MelonLoader;
using PlagueButtonAPI.Misc;
using PlagueButtonAPI.Pages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;
using VRC.UI.Elements;
using Object = UnityEngine.Object;

namespace PlagueButtonAPI.Controls.Grouping
{
    public class CollapsibleButtonGroup
    {
        public readonly ButtonGroup buttonGroup;

        public bool IsOpen;

        public CollapsibleButtonGroup(Transform parent, string text, string tooltip = "deprecated", bool openByDefault = false)
        {
            buttonGroup = new ButtonGroup(parent, text);

            buttonGroup.headerGameObject.transform.Find("Background_Button").gameObject.SetActive(true);
            buttonGroup.headerGameObject.transform.Find("Arrow").gameObject.SetActive(true);

            var foldout = buttonGroup.headerGameObject.GetComponent<QMFoldout>();

            foldout.field_Private_String_0 = "PlagueButtonGroup";

            foldout.field_Private_Action_1_Boolean_0 = new Action<bool>(val =>
            {
                buttonGroup.gameObject.SetActive(val);
                IsOpen = val;
            });

            buttonGroup.headerGameObject.transform.Find("Background_Button").gameObject.GetComponent<Toggle>().isOn = openByDefault;
        }

        public CollapsibleButtonGroup(MenuPage parent, string text, string tooltip = "deprecated", bool openByDefault = false) : this(parent.menuContents, text, tooltip, openByDefault)
        {
        }

        public void Destroy()
        {
            Object.Destroy(buttonGroup.gameObject);
        }

        public void SetActive(bool state)
        {
            buttonGroup?.SetActive(state);
        }
    }
}