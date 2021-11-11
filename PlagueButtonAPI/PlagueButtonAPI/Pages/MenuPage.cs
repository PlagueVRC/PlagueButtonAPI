using System;
using System.Linq;
using PlagueButtonAPI.Misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;
using VRC.UI.Elements;
using VRC.UI.Elements.Menus;

namespace PlagueButtonAPI.Pages
{
    public class MenuPage
    {
        private UIPage page;

        private GameObject gameObject;

        public Transform menuContents;

        private TextMeshProUGUI pageTitleText;

        private bool isRoot;

        private GameObject backButtonGameObject;

        private GameObject extButtonGameObject;

        private bool preserveColor;

        public RectMask2D menuMask;

        public string menuName;

        public MenuPage(string menuName, string pageTitle, bool root = true, bool backButton = true, bool expandButton = false, Action expandButtonAction = null, string expandButtonTooltip = "", Sprite expandButtonSprite = null, bool preserveColor = false)
        {
            gameObject = UnityEngine.Object.Instantiate(ButtonAPI.menuPageBase, ButtonAPI.menuPageBase.transform.parent);
            gameObject.name = "Menu_" + menuName;
            gameObject.transform.SetSiblingIndex(5);
            gameObject.SetActive(false);
            UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<LaunchPadQMMenu>());
            page = gameObject.AddComponent<UIPage>();
            page.field_Public_String_0 = menuName;
            page.field_Private_Boolean_1 = true;
            page.field_Private_MenuStateController_0 = ButtonAPI.GetMenuStateControllerInstance();
            page.field_Private_List_1_UIPage_0 = new Il2CppSystem.Collections.Generic.List<UIPage>();
            page.field_Private_List_1_UIPage_0.Add(page);
            ButtonAPI.GetMenuStateControllerInstance().field_Private_Dictionary_2_String_UIPage_0.Add(menuName, page);
            this.menuName = menuName;
            if (root)
            {
                System.Collections.Generic.List<UIPage> list = ButtonAPI.GetMenuStateControllerInstance().field_Public_ArrayOf_UIPage_0.ToList();
                list.Add(page);
                ButtonAPI.GetMenuStateControllerInstance().field_Public_ArrayOf_UIPage_0 = list.ToArray();
            }

            gameObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup").DestroyChildren();
            menuContents = gameObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup");
            pageTitleText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            pageTitleText.text = pageTitle;
            isRoot = root;
            backButtonGameObject = gameObject.transform.GetChild(0).Find("LeftItemContainer/Button_Back").gameObject;
            extButtonGameObject = gameObject.transform.GetChild(0).Find("RightItemContainer/Button_QM_Expand").gameObject;
            backButtonGameObject.SetActive(backButton);
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

            this.preserveColor = preserveColor;
            menuMask = menuContents.parent.gameObject.GetComponent<RectMask2D>();
            menuMask.enabled = true;
            gameObject.transform.Find("ScrollRect").GetComponent<ScrollRect>().enabled = true;
            gameObject.transform.Find("ScrollRect").GetComponent<ScrollRect>().verticalScrollbar = gameObject.transform.Find("ScrollRect/Scrollbar").GetComponent<Scrollbar>();
            gameObject.transform.Find("ScrollRect").GetComponent<ScrollRect>().verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
        }

        public void AddExtButton(Action onClick, string tooltip, Sprite icon)
        {
            GameObject obj = UnityEngine.Object.Instantiate(extButtonGameObject, extButtonGameObject.transform.parent);
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