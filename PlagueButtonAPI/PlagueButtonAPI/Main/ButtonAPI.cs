using MelonLoader;
using PlagueButtonAPI.Misc;
using System;
using System.Collections;
using System.Linq;
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

        public static GameObject sliderBase;

        public static Sprite onIconSprite;

        public static Sprite xIconSprite;

        public static QuickMenu GetQuickMenuInstance()
        {
            return Resources.FindObjectsOfTypeAll<QuickMenu>().FirstOrDefault();
        }

        public static MenuStateController GetMenuStateControllerInstance()
        {
            return Resources.FindObjectsOfTypeAll<QuickMenu>().FirstOrDefault()?.gameObject.GetOrAddComponent<MenuStateController>();
        }

        public override void OnUiManagerInit()
        {
            MelonCoroutines.Start(WaitForQMClone());
        }

        public static bool HasInit = false;

        private static IEnumerator WaitForQMClone()
        {
            while (GameObject.Find("UserInterface")?.transform?.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/") == null)
            {
                yield return new WaitForEndOfFrame();
            }

            singleButtonBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/Button_Respawn").gameObject;

            toggleButtonBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Settings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UI_Elements_Row_1/Button_ToggleQMInfo").gameObject;

            buttonGroupBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions").gameObject;

            buttonGroupHeaderBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Header_QuickActions").gameObject;

            menuPageBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard").gameObject;

            menuTabBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup/Page_Settings").gameObject;

            sliderBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_AudioSettings/Content/Audio/VolumeSlider_Master").gameObject;

            //For Toggles
            onIconSprite = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Notifications/Panel_NoNotifications_Message/Icon").GetOrAddComponent<Image>().sprite;

            xIconSprite = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Here/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_WorldActions/Button_FavoriteWorld/Icon_Secondary").GetOrAddComponent<Image>().sprite;

            OnInit?.Invoke();

            HasInit = true;
        }

        public static event Action OnInit;
    }

    /// <summary>
    /// A Component For Hooking To Generic Events Such As A Object Becoming Enabled, Disabled, Destroyed And For Events Such As Update.
    /// </summary>
    [RegisterTypeInIl2Cpp]
    public class ObjectHandler : MonoBehaviour
    {
        public ObjectHandler(IntPtr instance) : base(instance) { }

        public Action<GameObject> OnEnabled = null;
        public Action<GameObject> OnDisabled = null;
        public Action<GameObject> OnDestroyed = null;
        public Action<GameObject, bool> OnUpdate = null;
        public Action<GameObject, bool> OnUpdateEachSecond = null;

        private bool IsEnabled;
        private float UpdateDelay = 0f;

        void OnEnable()
        {
            OnEnabled?.Invoke(gameObject);
            IsEnabled = true;
        }

        void OnDisable()
        {
            OnDisabled?.Invoke(gameObject);
            IsEnabled = false;
        }

        void OnDestroy()
        {
            OnDestroyed?.Invoke(gameObject);
        }

        void Update()
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
