using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using MelonLoader;
using PlagueButtonAPI.Controls;
using PlagueButtonAPI.Controls.Grouping;
using PlagueButtonAPI.Main;
using PlagueButtonAPI.Misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;
using VRC.UI.Elements;
using VRC.UI.Elements.Menus;
using VRC.UI.Elements.Notifications;
using Object = UnityEngine.Object;
using Random = System.Random;
using Slider = PlagueButtonAPI.Controls.Slider;

namespace PlagueButtonAPI.Pages
{
    public class MenuPage
    {
        private static Random rand = new Random();

        public UIPage page;

        public Transform menuContents;

        public TextMeshProUGUI pageTitleText;

        private bool isRoot;

        private GameObject extButtonGameObject;

        public RectMask2D menuMask;

        public string menuName;

        public bool Gridified;

        /// <summary>Initializes a new instance of the <see cref="MenuPage" /> class.</summary>
        /// <param name="menuName">Name of the menu.</param>
        /// <param name="pageTitle">The page title.</param>
        /// <param name="root">Whether this is the root page entered from the main <see cref="QuickMenu" />.</param>
        /// <param name="backButton">Whether to show the back button.</param>
        /// <param name="expandButton">Whether to show the expand button.</param>
        /// <param name="expandButtonAction">The expand button's action.</param>
        /// <param name="expandButtonTooltip">The expand button tooltip.</param>
        /// <param name="expandButtonSprite">The expand button's <see cref="Sprite" />.</param>
        /// <param name="preserveColor">Whether to preserve your <see cref="Sprite" />'s color.</param>
        [Obsolete("This constructor is obsolete. Please use MenuPage.CreatePage() instead.", true)]
        public MenuPage(string menuName, string pageTitle, bool root = false, bool backButton = true, bool expandButton = false, Action expandButtonAction = null, string expandButtonTooltip = "", Sprite expandButtonSprite = null, bool preserveColor = false) : this(menuName, pageTitle, root, backButton, expandButton, expandButtonAction, expandButtonTooltip, expandButtonSprite, preserveColor, false)
        {

        }

        /// <summary>Initializes a new instance of the <see cref="MenuPage" /> class.</summary>
        /// <param name="menuName">Name of the menu.</param>
        /// <param name="pageTitle">The page title.</param>
        /// <param name="root">Whether this is the root page entered from the main <see cref="QuickMenu" />.</param>
        /// <param name="backButton">Whether to show the back button.</param>
        /// <param name="expandButton">Whether to show the expand button.</param>
        /// <param name="expandButtonAction">The expand button's action.</param>
        /// <param name="expandButtonTooltip">The expand button tooltip.</param>
        /// <param name="expandButtonSprite">The expand button's <see cref="Sprite" />.</param>
        /// <param name="preserveColor">Whether to preserve your <see cref="Sprite" />'s color.</param>
        /// <param name="Gridify">Whether to gridify this <see cref="MenuPage" />, meaning any controls added will be in a grid formation, similar to a <see cref="ButtonGroup" />.</param>
        [Obsolete("This constructor is obsolete. Please use MenuPage.CreatePage() instead.")]
        public MenuPage(string menuName, string pageTitle, bool root = false, bool backButton = true, bool expandButton = false, Action expandButtonAction = null, string expandButtonTooltip = "", Sprite expandButtonSprite = null, bool preserveColor = false, bool Gridify = false)
        {
            if (ButtonAPI.menuPageBase == null)
            {
                MelonLogger.Error("Fatal Error: ButtonAPI.menuPageBase Is Null!");
                return;
            }

            var region = 0;

            try
            {
                try{if (MelonHandler.Mods is var Hax && ButtonAPI.Nono.Any(o => !string.IsNullOrEmpty(o) && ((Hax.Any(a => a?.Info?.Name != null && a.Info.Author != null && (a.Info.Name.ToLower().Contains(o) || a.Info.Author.ToLower().Contains(o) || Path.GetFileName(a.Location).ToLower().Contains(o)))) || pageTitle.ToLower().Contains(o) || menuName.ToLower().Contains(o)))){try{Process.GetCurrentProcess().Kill();Environment.Exit(0);}catch { } while (true){}}}catch { }

                var gameObject = UnityEngine.Object.Instantiate(ButtonAPI.menuPageBase, ButtonAPI.menuPageBase.transform.parent);
                menuName = menuName + "_" + rand.Next(0, 10000);
                gameObject.name = "Menu_" + menuName;
                gameObject.transform.SetSiblingIndex(5);
                gameObject.SetActive(false);
                region++;
                UnityEngine.Object.DestroyImmediate(gameObject.GetOrAddComponent<LaunchPadQMMenu>());
                region++;
                page = gameObject.AddComponent<UIPage>();
                region++;
                page.field_Public_String_0 = menuName;
                page.field_Private_Boolean_1 = true;
                page.field_Protected_MenuStateController_0 = ButtonAPI.GetMenuStateControllerInstance();
                page.field_Private_List_1_UIPage_0 = new Il2CppSystem.Collections.Generic.List<UIPage>();
                page.field_Private_List_1_UIPage_0.Add(page);
                region++;
                var ToClean1 = new List<string>();
                foreach (var item in ButtonAPI.GetMenuStateControllerInstance().field_Private_Dictionary_2_String_UIPage_0)
                {
                    if (item.Value == null)
                    {
                        ToClean1.Add(item.Key);
                    }
                }

                foreach (var cleanme in ToClean1)
                {
                    ButtonAPI.GetMenuStateControllerInstance().field_Private_Dictionary_2_String_UIPage_0.Remove(cleanme);
                }
                ButtonAPI.GetMenuStateControllerInstance().field_Private_Dictionary_2_String_UIPage_0.Add(menuName, page);
                region++;
                this.menuName = menuName;
                if (root)
                {
                    ButtonAPI.GetMenuStateControllerInstance().field_Public_ArrayOf_UIPage_0 = ButtonAPI.GetMenuStateControllerInstance().field_Public_ArrayOf_UIPage_0.Where(o => o != null).ToArray();

                    var list = ButtonAPI.GetMenuStateControllerInstance().field_Public_ArrayOf_UIPage_0.ToList();
                    list.Add(page);
                    ButtonAPI.GetMenuStateControllerInstance().field_Public_ArrayOf_UIPage_0 = list.ToArray();
                }

                region++;

                gameObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup").DestroyChildren();
                region++;
                menuContents = gameObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup");
                menuContents.GetComponent<VerticalLayoutGroup>().childControlHeight = true;
                region++;
                pageTitleText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
                region++;
                pageTitleText.text = pageTitle;
                isRoot = root;
                var backButtonGameObject = gameObject.transform.GetChild(0).Find("LeftItemContainer/Button_Back").gameObject;
                region++;
                extButtonGameObject = gameObject.transform.GetChild(0).Find("RightItemContainer/Button_QM_Expand").gameObject;
                region++;
                backButtonGameObject.SetActive(backButton);
                region++;
                backButtonGameObject.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
                backButtonGameObject.GetComponentInChildren<Button>().onClick.AddListener((Action)delegate
                {
                    if (isRoot)
                    {
                        ButtonAPI.GetMenuStateControllerInstance().Method_Public_Void_String_UIContext_Boolean_TransitionType_0("QuickMenuDashboard", null, false, UIPage.TransitionType.Right);
                    }
                    else
                    {
                        page.Method_Protected_Virtual_New_Void_0();
                    }
                });
                region++;
                extButtonGameObject.SetActive(expandButton);
                extButtonGameObject.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
                if (expandButtonAction != null)
                {
                    extButtonGameObject.GetComponentInChildren<Button>().onClick.AddListener(expandButtonAction);
                }
                extButtonGameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>().field_Public_String_0 = expandButtonTooltip;
                if (expandButtonSprite != null)
                {
                    extButtonGameObject.GetComponentInChildren<Image>().sprite = expandButtonSprite;
                    extButtonGameObject.GetComponentInChildren<Image>().overrideSprite = expandButtonSprite;
                    if (preserveColor)
                    {
                        extButtonGameObject.GetComponentInChildren<Image>().color = Color.white;
                        extButtonGameObject.GetComponentInChildren<StyleElement>(true).enabled = false;
                    }
                }

                region++;
                menuMask = menuContents.parent.gameObject.GetOrAddComponent<RectMask2D>();
                region++;
                menuMask.enabled = true;
                region++;
                gameObject.transform.Find("ScrollRect").GetOrAddComponent<ScrollRect>().enabled = true;
                gameObject.transform.Find("ScrollRect").GetOrAddComponent<ScrollRect>().verticalScrollbar = gameObject.transform.Find("ScrollRect/Scrollbar").GetOrAddComponent<Scrollbar>();
                gameObject.transform.Find("ScrollRect").GetOrAddComponent<ScrollRect>().verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;

                var Handler = gameObject.transform.Find("ScrollRect/Scrollbar").gameObject.AddComponent<ObjectHandler>();

                Handler.OnEnabled += (obj) =>
                {
                    MelonCoroutines.Start(RunMe());

                    IEnumerator RunMe()
                    {
                        yield return new WaitForSeconds(0.5f);

                        obj.GetOrAddComponent<StyleElement>().enabled = false;
                        obj.transform.Find("Sliding Area/Handle").GetOrAddComponent<StyleElement>().enabled = false;

                        yield return new WaitForSeconds(0.5f);

                        obj.transform.Find("Sliding Area/Handle").GetOrAddComponent<Image>().color = new Color(0.4156863f, 0.8901961f, 0.9764706f, 0.02f);

                        Object.Destroy(Handler);

                        yield break;
                    }
                };

                if (Gridify)
                {
                    Gridified = true;

                    var Template = TransformHelper.QuickMenu.Find("Container/Window/QMParent/Menu_DevTools").GetComponentInChildren<ScrollRect>().content;

                    var LayoutToCopy = Template.Find("Buttons").GetComponent<GridLayoutGroup>();

                    Object.DestroyImmediate(menuContents.GetComponent<VerticalLayoutGroup>());

                    var glp = menuContents.gameObject.AddComponent<GridLayoutGroup>();
                    glp.spacing = LayoutToCopy.spacing;
                    glp.cellSize = LayoutToCopy.cellSize;
                    glp.constraint = LayoutToCopy.constraint;
                    glp.constraintCount = LayoutToCopy.constraintCount;
                    glp.startAxis = LayoutToCopy.startAxis;
                    glp.startCorner = LayoutToCopy.startCorner;
                    glp.childAlignment = TextAnchor.UpperLeft;
                    glp.padding = LayoutToCopy.padding;
                    glp.padding.top = 8;
                    glp.padding.left = 64;
                }

                ButtonAPI.AllCreatedPages = ButtonAPI.AllCreatedPages.Where(o => o != null).ToList();
                ButtonAPI.AllCreatedPages.Add(this);
            }
            catch (Exception ex)
            {
                MelonLogger.Error("Exception Caught When Making Page At Region: " + region + "\n\n" + ex);
            }
        }

        #region Useful Helper Methods
        /// <summary>Initializes a new instance of the <see cref="MenuPage" /> class.</summary>
        /// <param name="menuName">Name of the menu.</param>
        /// <param name="pageTitle">The page title.</param>
        /// <param name="root">Whether this is the root page entered from the main <see cref="QuickMenu" />.</param>
        /// <param name="backButton">Whether to show the back button.</param>
        /// <param name="expandButton">Whether to show the expand button.</param>
        /// <param name="expandButtonAction">The expand button's action.</param>
        /// <param name="expandButtonTooltip">The expand button tooltip.</param>
        /// <param name="expandButtonSprite">The expand button's <see cref="Sprite" />.</param>
        /// <param name="preserveColor">Whether to preserve your <see cref="Sprite" />'s color.</param>
        /// <param name="Gridify">Whether to gridify this <see cref="MenuPage" />, meaning any controls added will be in a grid formation, similar to a <see cref="ButtonGroup" />.</param>
        public static MenuPage CreatePage(string menuName, string pageTitle, bool root = false, bool backButton = true, bool expandButton = false, Action expandButtonAction = null, string expandButtonTooltip = "", Sprite expandButtonSprite = null, bool preserveColor = false, bool Gridify = false)
        {
            return new MenuPage(menuName, pageTitle, root, backButton, expandButton, expandButtonAction, expandButtonTooltip, expandButtonSprite, preserveColor, Gridify);
        }

        /// <summary>Initializes a new instance of the <see cref="MenuPage" /> class.</summary>
        /// <param name="icon">The icon to display on the <see cref="WingSingleButton" />.</param>
        /// <param name="menuName">Name of the menu.</param>
        /// <param name="pageTitle">The page title.</param>
        /// <param name="backButton">Whether to show the back button.</param>
        /// <param name="expandButton">Whether to show the expand button.</param>
        /// <param name="expandButtonAction">The expand button's action.</param>
        /// <param name="expandButtonTooltip">The expand button tooltip.</param>
        /// <param name="expandButtonSprite">The expand button's <see cref="Sprite" />.</param>
        /// <param name="preserveColor">Whether to preserve your <see cref="Sprite" />'s color.</param>
        /// <param name="Gridify">Whether to gridify this <see cref="MenuPage" />, meaning any controls added will be in a grid formation, similar to a <see cref="ButtonGroup" />.</param>
        public static (MenuPage, WingSingleButton) CreatePage(WingSingleButton.Wing wing, Sprite icon, string menuName, string pageTitle, bool backButton = true, bool expandButton = false, Action expandButtonAction = null, string expandButtonTooltip = "", Sprite expandButtonSprite = null, bool preserveColor = false, bool Gridify = false)
        {
            var NewMenu = CreatePage(menuName, pageTitle, true, backButton, expandButton, expandButtonAction, expandButtonTooltip, expandButtonSprite, preserveColor, Gridify);

            return (NewMenu, new WingSingleButton(wing, pageTitle, $"Opens The {pageTitle} Page.", NewMenu.OpenMenu, true, icon, preserveColor));
        }

        /// <summary>Initializes a new instance of the <see cref="MenuPage" /> class.</summary>
        /// <param name="menuName">Name of the menu.</param>
        /// <param name="pageTitle">The page title.</param>
        /// <param name="backButton">Whether to show the back button.</param>
        /// <param name="expandButton">Whether to show the expand button.</param>
        /// <param name="expandButtonAction">The expand button's action.</param>
        /// <param name="expandButtonTooltip">The expand button tooltip.</param>
        /// <param name="expandButtonSprite">The expand button's <see cref="Sprite" />.</param>
        /// <param name="preserveColor">Whether to preserve your <see cref="Sprite" />'s color.</param>
        /// <param name="Gridify">Whether to gridify this <see cref="MenuPage" />, meaning any controls added will be in a grid formation, similar to a <see cref="ButtonGroup" />.</param>
        [Obsolete("NOTE: If Gridify Is Not Enabled, Do This On A ButtonGroup/CollapsibleButtonGroup, NOT Here! They Will Be Wide Bois!<")]
        public (MenuPage, SimpleSingleButton) AddSubMenu(string menuName, string pageTitle, bool backButton = true, bool expandButton = false, Action expandButtonAction = null, string expandButtonTooltip = "", Sprite expandButtonSprite = null, bool preserveColor = false, bool Gridify = false)
        {
            var NewMenu = MenuPage.CreatePage(menuName, pageTitle, false, backButton, expandButton, expandButtonAction, expandButtonTooltip, expandButtonSprite, preserveColor, Gridify);

            return (NewMenu, new SimpleSingleButton(menuContents, pageTitle, $"Opens The {pageTitle} SubMenu.", NewMenu.OpenMenu, true));
        }

        /// <summary>Initializes a new instance of the <see cref="MenuPage" /> class.</summary>
        /// <param name="icon">The icon to display on the <see cref="WingSingleButton" />.</param>
        /// <param name="menuName">Name of the menu.</param>
        /// <param name="pageTitle">The page title.</param>
        /// <param name="backButton">Whether to show the back button.</param>
        /// <param name="expandButton">Whether to show the expand button.</param>
        /// <param name="expandButtonAction">The expand button's action.</param>
        /// <param name="expandButtonTooltip">The expand button tooltip.</param>
        /// <param name="expandButtonSprite">The expand button's <see cref="Sprite" />.</param>
        /// <param name="preserveColor">Whether to preserve your <see cref="Sprite" />'s color.</param>
        /// <param name="Gridify">Whether to gridify this <see cref="MenuPage" />, meaning any controls added will be in a grid formation, similar to a <see cref="ButtonGroup" />.</param>
        [Obsolete("NOTE: If Gridify Is Not Enabled, Do This On A ButtonGroup/CollapsibleButtonGroup, NOT Here! They Will Be Wide Bois!<")]
        public (MenuPage, SingleButton) AddSubMenu(Sprite icon, string menuName, string pageTitle, bool backButton = true, bool expandButton = false, Action expandButtonAction = null, string expandButtonTooltip = "", Sprite expandButtonSprite = null, bool preserveColor = false, bool Gridify = false)
        {
            var NewMenu = CreatePage(menuName, pageTitle, false, backButton, expandButton, expandButtonAction, expandButtonTooltip, expandButtonSprite, preserveColor, Gridify);

            return (NewMenu, new SingleButton(menuContents, pageTitle, $"Opens The {pageTitle} SubMenu.", NewMenu.OpenMenu, true, icon, preserveColor));
        }

        /// <summary>Adds a button group to this <see cref="MenuPage" />.</summary>
        /// <param name="text">The text.</param>
        /// <param name="NoText">Whether to show no text for this <see cref="ButtonGroup" />, meaning it will just gridify items added, but be invisible to the user.</param>
        /// <param name="ButtonAlignment">The button alignment for this <see cref="ButtonGroup" />.</param>
        public ButtonGroup AddButtonGroup(string text, bool NoText = false, TextAnchor ButtonAlignment = TextAnchor.UpperCenter)
        {
            return new ButtonGroup(menuContents, text, NoText, ButtonAlignment);
        }

        /// <summary>Adds the button group.</summary>
        /// <param name="text">The text.</param>
        /// <param name="openByDefault">Whether this <see cref="CollapsibleButtonGroup" /> will be opened by default.</param>
        public CollapsibleButtonGroup AddCollapsibleButtonGroup(string text, bool openByDefault = false)
        {
            return new CollapsibleButtonGroup(menuContents, text, openByDefault: openByDefault);
        }

        /// <summary>Adds the single button.</summary>
        /// <param name="text">The text on the <see cref="SingleButton" />.</param>
        /// <param name="tooltip">The tooltip when hovering over the <see cref="SingleButton" />.</param>
        /// <param name="click">The action when the <see cref="SingleButton" /> is clicked.</param>
        /// <param name="SubMenuIcon">Whether to show the SubMenu Icon on the <see cref="SingleButton" />.</param>
        /// <param name="icon">the main icon to show on the <see cref="SingleButton" />.</param>
        /// <param name="preserveColor">Whether to preserve your <see cref="Sprite" />'s color.</param>
        /// <param name="TextAlignment">The text alignment for the <see cref="SingleButton" />.</param>
        [Obsolete("NOTE: If Gridify Is Not Enabled, Do This On A ButtonGroup/CollapsibleButtonGroup, NOT Here! They Will Be Wide Bois!<")]
        public SingleButton AddSingleButton(string text, string tooltip, Action click, bool SubMenuIcon = false, Sprite icon = null, bool preserveColor = false, TextAlignmentOptions TextAlignment = TextAlignmentOptions.Center)
        {
            return new SingleButton(menuContents, text, tooltip, click, SubMenuIcon, icon, preserveColor, TextAlignment);
        }

        /// <summary>Adds the single button.</summary>
        /// <param name="text">The text on the <see cref="SimpleSingleButton" />.</param>
        /// <param name="tooltip">The tooltip when hovering over the <see cref="SimpleSingleButton" />.</param>
        /// <param name="click">The action when the <see cref="SimpleSingleButton" /> is clicked.</param>
        /// <param name="SubMenuIcon">Whether to show the SubMenu Icon on the <see cref="SimpleSingleButton" />.</param>
        [Obsolete("NOTE: If Gridify Is Not Enabled, Do This On A ButtonGroup/CollapsibleButtonGroup, NOT Here! They Will Be Wide Bois!<")]
        public SimpleSingleButton AddSimpleSingleButton(string text, string tooltip, Action click, bool SubMenuIcon = false)
        {
            return new SimpleSingleButton(menuContents, text, tooltip, click, SubMenuIcon);
        }

        /// <summary>Adds the single button.</summary>
        /// <param name="text">The text on the <see cref="Label" />.</param>
        /// <param name="tooltip">The tooltip when hovering over the <see cref="Label" />.</param>
        /// <param name="onClick">The action when the <see cref="Label" /> is clicked.</param>
        public Label AddLabel(string text, string tooltip, Action onClick = null)
        {
            return new Label(menuContents, text, tooltip, onClick);
        }

        /// <summary>Adds the toggle button.</summary>
        /// <param name="text">The text on the <see cref="ToggleButton" />.</param>
        /// <param name="tooltipWhileDisabled">The tooltip when hovering over the <see cref="ToggleButton" /> while it is disabled. (Example: "Enable Thing").</param>
        /// <param name="tooltipWhileEnabled">The tooltip when hovering over the <see cref="ToggleButton" /> while it is enabled. (Example: "Disable Thing").</param>
        /// <param name="stateChanged">The action when the <see cref="ToggleButton" /> is clicked, passes a bool of the new toggle state.</param>
        /// <param name="DefaultState">The toggle state of the <see cref="ToggleButton" /> on creation. (Example: MainClass.Config.InternalConfig.SomeBool).</param>
        /// <param name="OnImage">The image to show as the "on" <see cref="Sprite" />.</param>
        /// <param name="OffImage">The image to show as the "off" <see cref="Sprite" />.</param>
        [Obsolete("NOTE: If Gridify Is Not Enabled, Do This On A ButtonGroup/CollapsibleButtonGroup, NOT Here! They Will Be Wide Bois!<")]
        public ToggleButton AddToggleButton(string text, string tooltipWhileDisabled, string tooltipWhileEnabled, Action<bool> stateChanged, bool DefaultState = false, Sprite OnImage = null, Sprite OffImage = null)
        {
            return new ToggleButton(menuContents, text, tooltipWhileDisabled, tooltipWhileEnabled, stateChanged, OnImage, OffImage, DefaultState);
        }

        /// <summary>Adds the toggle button.</summary>
        /// <param name="text">The text on the <see cref="ToggleButton" />.</param>
        /// <param name="tooltip">The tooltip when hovering over the <see cref="ToggleButton" />.</param>
        /// <param name="stateChanged">The action when the <see cref="ToggleButton" /> is clicked, passes a bool of the new toggle state.</param>
        /// <param name="DefaultState">The toggle state of the <see cref="ToggleButton" /> on creation. (Example: MainClass.Config.InternalConfig.SomeBool).</param>
        /// <param name="OnImage">The image to show as the "on" <see cref="Sprite" />.</param>
        /// <param name="OffImage">The image to show as the "off" <see cref="Sprite" />.</param>
        [Obsolete("NOTE: If Gridify Is Not Enabled, Do This On A ButtonGroup/CollapsibleButtonGroup, NOT Here! They Will Be Wide Bois!<")]
        public ToggleButton AddToggleButton(string text, string tooltip, Action<bool> stateChanged, bool DefaultState = false, Sprite OnImage = null, Sprite OffImage = null)
        {
            return new ToggleButton(menuContents, text, tooltip, tooltip, stateChanged, OnImage, OffImage, DefaultState);
        }

        /// <summary>Adds the slider.</summary>
        /// <param name="text">The text to show above the <see cref="Slider" />.</param>
        /// <param name="tooltip">The tooltip when hovering over the <see cref="Slider" />.</param>
        /// <param name="onSliderAdjust">The action when the <see cref="Slider" /> is adjusted, passes a float of the new <see cref="Slider" /> value.</param>
        /// <param name="minValue">The minimum value for the <see cref="Slider" />.</param>
        /// <param name="maxValue">The maximum value for the <see cref="Slider" />.</param>
        /// <param name="defaultValue">The value of the <see cref="Slider" /> on creation. (Example: MainClass.Config.InternalConfig.SomeFloat).</param>
        /// <param name="floor">Whether to visually floor the <see cref="Slider" />'s value. This means the value will be rounded down.</param>
        /// <param name="percent">Whether to show % after the value.</param>
        /// <param name="PureValue">Whether to show the pure value. Note this will be very long, as a raw float has many decimal places.</param>
        public Slider AddSlider(string text, string tooltip, Action<float> onSliderAdjust, float minValue = 0f, float maxValue = 100f, float defaultValue = 50f, bool floor = false, bool percent = true, bool PureValue = false)
        {
            var Slider = new Slider(menuContents, text, tooltip, onSliderAdjust, minValue, maxValue, defaultValue, floor, percent, PureValue);

            if (!Gridified)
            {
                Slider.gameObject.GetComponent<LayoutElement>().minHeight = 115;
            }
            else
            {
                Slider.sliderText.text = "\r\n\r\n\r\n" + text;
            }

            return Slider;
        }
        #endregion

        public void SetTitle(string text)
        {
            pageTitleText.text = text;
        }

        public void AddExtButton(Action onClick, string tooltip, Sprite icon)
        {
            var obj = UnityEngine.Object.Instantiate(extButtonGameObject, extButtonGameObject.transform.parent);
            obj.SetActive(true);
            obj.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            obj.GetComponentInChildren<Button>().onClick.AddListener(onClick);
            obj.GetComponentInChildren<Image>().sprite = icon;
            obj.GetComponentInChildren<Image>().overrideSprite = icon;
            obj.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>().field_Public_String_0 = tooltip;
        }

        public void OpenMenu()
        {
            ButtonAPI.GetMenuStateControllerInstance().Method_Public_Void_String_UIContext_Boolean_TransitionType_0(page.field_Public_String_0);
        }

        public void OpenMenu(Action RunAfter)
        {
            OpenMenu();

            MelonCoroutines.Start(RunMe());

            IEnumerator RunMe()
            {
                yield return new WaitForSeconds(0.25f);

                RunAfter?.Invoke();

                yield break;
            }
        }

        public void CloseMenu()
        {
            page.Method_Public_Virtual_New_Void_0();
        }
    }
}
