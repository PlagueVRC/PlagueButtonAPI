using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlagueButtonAPI.Misc;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;

namespace PlagueButtonAPI
{
    public class ButtonAPI : MelonLoaderEvents
    {
        public static GameObject singleButtonBase;

        public static GameObject textButtonBase;

        public static GameObject toggleButtonBase;

        public static GameObject buttonGroupBase;

        public static GameObject buttonGroupHeaderBase;

        public static GameObject collapsibleButtonGroupFoldoutButtonBase;

        public static GameObject collapsibleButtonGroupGridBase;

        public static GameObject menuPageBase;

        public static GameObject menuTabBase;

        public static GameObject sliderBase;

        public static Sprite onIconSprite;

        public static Sprite plusIconSprite;

        public static Sprite xIconSprite;

        public static QuickMenu GetQuickMenuInstance()
        {
            return Resources.FindObjectsOfTypeAll<QuickMenu>().FirstOrDefault();
        }

        public static MenuStateController GetMenuStateControllerInstance()
        {
            return Resources.FindObjectsOfTypeAll<QuickMenu>().FirstOrDefault()?.gameObject.GetComponent<MenuStateController>();
        }

        public override void OnUiManagerInit()
        {
            MelonCoroutines.Start(WaitForQMClone());
        }

        public static bool HasInit = false;

        private static IEnumerator WaitForQMClone()
        {
            while (GameObject.Find("UserInterface")?.transform?.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/") == null || GameObject.Find("UserInterface")?.transform?.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_QM_Emojis/ScrollRect/Viewport/VerticalLayoutGroup/QM_EmojiGrid(Clone)") == null)
            {
                yield return new WaitForEndOfFrame();
            }

            singleButtonBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/Button_Respawn").gameObject;
            textButtonBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Settings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/Buttons_Debug/Button_Build").gameObject;
            toggleButtonBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Settings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UI_Elements_Row_1/Button_ToggleQMInfo").gameObject;
            buttonGroupBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions").gameObject;
            buttonGroupHeaderBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Header_QuickActions").gameObject;
            collapsibleButtonGroupFoldoutButtonBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_QM_Emojis/ScrollRect/Viewport/VerticalLayoutGroup/QM_GridFoldout(Clone)").gameObject;
            collapsibleButtonGroupGridBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_QM_Emojis/ScrollRect/Viewport/VerticalLayoutGroup/QM_EmojiGrid(Clone)").gameObject;
            menuPageBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard").gameObject;
            menuTabBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup/Page_Settings").gameObject;
            sliderBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_AudioSettings/Content/Audio/VolumeSlider_Master").gameObject;
            onIconSprite = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Notifications/Panel_NoNotifications_Message/Icon").GetComponent<Image>().sprite;
            plusIconSprite = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Here/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_WorldActions/Button_FavoriteWorld/Icon").GetComponent<Image>().sprite;
            xIconSprite = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Here/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_WorldActions/Button_FavoriteWorld/Icon_Secondary").GetComponent<Image>().sprite;

            OnInit?.Invoke();

            HasInit = true;
        }

        public static event Action OnInit;
    }
}
