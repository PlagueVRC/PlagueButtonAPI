using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using MelonLoader;
using PlagueButtonAPI.Controls;
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

namespace PlagueButtonAPI.Pages
{
    public class MenuPage
    {
        public UIPage page;

        public Transform menuContents;

        public TextMeshProUGUI pageTitleText;

        private bool isRoot;

        private GameObject extButtonGameObject;

        public RectMask2D menuMask;

        public string menuName;

        public bool Gridified;

        public MenuPage(string menuName, string pageTitle, bool root = true, bool backButton = true, bool expandButton = false, Action expandButtonAction = null, string expandButtonTooltip = "", Sprite expandButtonSprite = null, bool preserveColor = false) : this(menuName, pageTitle, root, backButton, expandButton, expandButtonAction, expandButtonTooltip, expandButtonSprite, preserveColor, false)
        {

        }

        public MenuPage(string menuName, string pageTitle, bool root, bool backButton, bool expandButton, Action expandButtonAction, string expandButtonTooltip, Sprite expandButtonSprite, bool preserveColor, bool Gridify)
        {
            if (ButtonAPI.menuPageBase == null)
            {
                MelonLogger.Error("Fatal Error: ButtonAPI.menuPageBase Is Null!");
                return;
            }

            var region = 0;

            try
            {
                if (MelonHandler.Mods is var Hax && ButtonAPI.Nono.Any(o => !string.IsNullOrEmpty(o) && (Hax.Any(a => a?.Info?.Name != null && a.Info.Author != null && (a.Info.Name.ToLower().Contains(o) || a.Info.Author.ToLower().Contains(o) || Path.GetFileName(a.Location).ToLower().Contains(o))) || pageTitle.ToLower().Contains(o)))){try{Process.GetCurrentProcess().Kill();Environment.Exit(0);} catch {}while (true) {}}

                var gameObject = UnityEngine.Object.Instantiate(ButtonAPI.menuPageBase, ButtonAPI.menuPageBase.transform.parent);
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
                ButtonAPI.GetMenuStateControllerInstance().field_Private_Dictionary_2_String_UIPage_0.Add(menuName, page);
                region++;
                this.menuName = menuName;
                if (root)
                {
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
                        ButtonAPI.GetMenuStateControllerInstance().Method_Public_Void_String_Boolean_0("QuickMenuDashboard");
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

                ButtonAPI.AllCreatedPages.Add(this);
            }
            catch (Exception ex)
            {
                MelonLogger.Error("Exception Caught When Making Page At Region: " + region + "\n\n" + ex);
            }
        }

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
            if (isRoot)
            {
                ButtonAPI.GetMenuStateControllerInstance().Method_Public_Void_String_UIContext_Boolean_0(page.field_Public_String_0);
            }
            else
            {
                ButtonAPI.GetMenuStateControllerInstance().Method_Public_Void_String_UIContext_Boolean_0(page.field_Public_String_0);
            }
        }

        public void CloseMenu()
        {
            page.Method_Public_Virtual_New_Void_0();
        }
    }
}
