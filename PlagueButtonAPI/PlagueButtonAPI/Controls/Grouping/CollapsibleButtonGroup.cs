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

        #region Useful Helper Methods
        public static MenuPage CreatePage(string menuName, string pageTitle, bool root = false, bool backButton = true, bool expandButton = false, Action expandButtonAction = null, string expandButtonTooltip = "", Sprite expandButtonSprite = null, bool preserveColor = false, bool Gridify = false)
        {
            return new MenuPage(menuName, pageTitle, root, backButton, expandButton, expandButtonAction, expandButtonTooltip, expandButtonSprite, preserveColor, Gridify);
        }

        public (MenuPage, SimpleSingleButton) AddSubMenu(string menuName, string pageTitle, bool backButton = true, bool expandButton = false, Action expandButtonAction = null, string expandButtonTooltip = "", Sprite expandButtonSprite = null, bool preserveColor = false, bool Gridify = false)
        {
            var NewMenu = new MenuPage(menuName, pageTitle, false, backButton, expandButton, expandButtonAction, expandButtonTooltip, expandButtonSprite, preserveColor, Gridify);

            return (NewMenu, new SimpleSingleButton(this, pageTitle, $"Opens The {pageTitle} SubMenu.", NewMenu.OpenMenu, true));
        }

        public (MenuPage, SingleButton) AddSubMenu(Sprite icon, string menuName, string pageTitle, bool backButton = true, bool expandButton = false, Action expandButtonAction = null, string expandButtonTooltip = "", Sprite expandButtonSprite = null, bool preserveColor = false, bool Gridify = false)
        {
            var NewMenu = new MenuPage(menuName, pageTitle, false, backButton, expandButton, expandButtonAction, expandButtonTooltip, expandButtonSprite, preserveColor, Gridify);

            return (NewMenu, new SingleButton(this, pageTitle, $"Opens The {pageTitle} SubMenu.", NewMenu.OpenMenu, true, icon, preserveColor));
        }

        public SingleButton AddSingleButton(string text, string tooltip, Action click, bool SubMenuIcon = false, Sprite icon = null, bool preserveColor = false, TextAlignmentOptions TextAlignment = TextAlignmentOptions.Center)
        {
            return new SingleButton(this, text, tooltip, click, SubMenuIcon, icon, preserveColor, TextAlignment);
        }

        public SimpleSingleButton AddSimpleSingleButton(string text, string tooltip, Action click, bool SubMenuIcon = false)
        {
            return new SimpleSingleButton(this, text, tooltip, click, SubMenuIcon);
        }

        public Label AddLabel(string text, string tooltip, Action onClick = null)
        {
            return new Label(this, text, tooltip, onClick);
        }

        public ToggleButton AddToggleButton(string text, string tooltipWhileDisabled, string tooltipWhileEnabled, Action<bool> stateChanged, bool DefaultState = false, Sprite OnImage = null, Sprite OffImage = null)
        {
            return new ToggleButton(this, text, tooltipWhileDisabled, tooltipWhileEnabled, stateChanged, OnImage, OffImage, DefaultState);
        }
        #endregion
    }
}