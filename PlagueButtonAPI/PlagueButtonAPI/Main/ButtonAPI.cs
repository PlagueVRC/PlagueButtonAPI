using System;
using System.Collections;
using System.Linq;
using MelonLoader;
using PlagueButtonAPI.Misc;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;

namespace PlagueButtonAPI
{
    public class ButtonAPI : MelonLoaderEvents
    {
        public static GameObject singleButtonBase;

        public static GameObject toggleButtonBase;

        public static GameObject buttonGroupBase;

        public static GameObject buttonGroupHeaderBase;

        public static GameObject menuPageBase;

        public static GameObject menuTabBase;

        public static GameObject wingSingleButtonBase;

        public static GameObject sliderBase;

        public static Sprite onIconSprite;

        public static Sprite xIconSprite;

        public static bool HasInit = false;

        public static bool PauseInit = false;

        public static VRC.UI.Elements.QuickMenu GetQuickMenuInstance()
        {
            return Resources.FindObjectsOfTypeAll<VRC.UI.Elements.QuickMenu>().FirstOrDefault();
        }

        public static MenuStateController GetMenuStateControllerInstance()
        {
            return Resources.FindObjectsOfTypeAll<VRC.UI.Elements.QuickMenu>().FirstOrDefault()?.gameObject.GetOrAddComponent<MenuStateController>();
        }

        public override void OnUiManagerInit()
        {
            MelonCoroutines.Start(WaitForQMClone());
        }

        private static IEnumerator WaitForQMClone()
        {
            while (GameObject.Find("UserInterface")?.transform?.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/")?.gameObject == null || GameObject.Find("UserInterface")?.transform?.Find("Canvas_QuickMenu(Clone)/Container/Window/Wing_Left/Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup/Button_Explore")?.gameObject == null || GetMenuStateControllerInstance() == null)
            {
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(2f); // Just In Case!

            singleButtonBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/Button_Respawn")?.gameObject;

            if (singleButtonBase == null)
            {
                MelonLogger.Error("singleButtonBase == null!");
            }

            toggleButtonBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Settings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UI_Elements_Row_1/Button_ToggleQMInfo")?.gameObject;

            if (toggleButtonBase == null)
            {
                MelonLogger.Error("toggleButtonBase == null!");
            }

            buttonGroupBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions")?.gameObject;

            if (buttonGroupBase == null)
            {
                MelonLogger.Error("buttonGroupBase == null!");
            }

            buttonGroupHeaderBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Settings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/QM_Foldout_UI_Elements")?.gameObject;

            if (buttonGroupHeaderBase == null)
            {
                MelonLogger.Error("buttonGroupHeaderBase == null!");
            }

            menuPageBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard")?.gameObject;

            if (menuPageBase == null)
            {
                MelonLogger.Error("menuPageBase == null!");
            }

            menuTabBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup/Page_Settings")?.gameObject;

            if (menuTabBase == null)
            {
                MelonLogger.Error("menuTabBase == null!");
            }

            wingSingleButtonBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/Wing_Left/Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup/Button_Explore")?.gameObject;

            if (wingSingleButtonBase == null)
            {
                MelonLogger.Error("wingSingleButtonBase == null!");
            }

            sliderBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_AudioSettings/Content/Audio/VolumeSlider_Master")?.gameObject;

            if (sliderBase == null)
            {
                MelonLogger.Error("sliderBase == null!");
            }

            //For Toggles
            onIconSprite = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Notifications/Panel_NoNotifications_Message/Icon")?.GetComponent<Image>()?.sprite;

            if (onIconSprite == null)
            {
                MelonLogger.Error("onIconSprite == null!");
            }

            xIconSprite = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Here/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_WorldActions/Button_FavoriteWorld/Icon_Secondary")?.GetComponent<Image>()?.sprite;

            if (xIconSprite == null)
            {
                MelonLogger.Error("xIconSprite == null!");
            }

            while (PauseInit)
            {
                yield return new WaitForEndOfFrame();
            }

            OnInit?.Invoke();

            HasInit = true;
        }

        public static event Action OnInit;
    }

    /// <summary>
    ///   A Component For Hooking To Generic Events Such As A Object Becoming Enabled, Disabled, Destroyed And For Events Such
    ///   As Update.
    /// </summary>
    [RegisterTypeInIl2Cpp]
    public class ObjectHandler : MonoBehaviour
    {
        private bool IsEnabled;
        public Action<GameObject> OnDestroyed = null;
        public Action<GameObject> OnDisabled = null;

        public Action<GameObject> OnEnabled = null;
        public Action<GameObject, bool> OnUpdate = null;
        public Action<GameObject, bool> OnUpdateEachSecond = null;
        private float UpdateDelay = 0f;

        public ObjectHandler(IntPtr instance) : base(instance)
        {
        }

        private void OnEnable()
        {
            if (gameObject.active)
            {
                OnEnabled?.Invoke(gameObject);
                IsEnabled = true;
            }
        }

        private void OnDisable()
        {
            if (!gameObject.active)
            {
                OnDisabled?.Invoke(gameObject);
                IsEnabled = false;
            }
        }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke(gameObject);
        }

        private void Update()
        {
            OnUpdate?.Invoke(gameObject, IsEnabled);

            if (UpdateDelay < Time.time)
            {
                UpdateDelay = Time.time + 1f;

                OnUpdateEachSecond?.Invoke(gameObject, IsEnabled);
            }
        }
    }
}