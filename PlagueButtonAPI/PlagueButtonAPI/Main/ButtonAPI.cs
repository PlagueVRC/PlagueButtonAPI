using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Harmony;
using MelonLoader;
using PlagueButtonAPI.Main;
using PlagueButtonAPI.Misc;
using PlagueButtonAPI.Pages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;

namespace PlagueButtonAPI
{
    public class ButtonAPI : MelonLoaderEvents
    {
        public static List<MenuPage> AllCreatedPages = new List<MenuPage>();

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

        public static bool HasInit;

        public static bool PauseInit;

        public static VRC.UI.Elements.QuickMenu GetQuickMenuInstance()
        {
            return Resources.FindObjectsOfTypeAll<VRC.UI.Elements.QuickMenu>().FirstOrDefault();
        }

        public static MenuStateController GetMenuStateControllerInstance()
        {
            return Resources.FindObjectsOfTypeAll<VRC.UI.Elements.QuickMenu>().FirstOrDefault()?.gameObject.GetOrAddComponent<MenuStateController>();
        }

        public static GameObject GetEyeCamera()
        {
            return GameObject.Find("Camera (eye)");
        }

        public override void OnUiManagerInit()
        {
            MelonCoroutines.Start(WaitForQMClone());
        }

        internal static string[] Nono;

        internal static GameObject QuickMenuObj;

        [HarmonyShield]
        [PatchShield]
        private static IEnumerator WaitForQMClone()
        {
            var IsDebug = Environment.CommandLine.ToLower().Contains("debug");
            var Paranoid = Environment.CommandLine.ToLower().Contains("paranoid");

            #if !Mods
            if (!Paranoid && !File.Exists(Environment.CurrentDirectory + "\\Plugins\\GithubModUpdater.dll"))
            {
                using var client = new WebClient();
                client.DownloadFile("https://github.com/PlagueVRC/GithubModUpdater/releases/latest/download/GithubModUpdater.dll", Environment.CurrentDirectory + "\\Plugins\\GithubModUpdater.dll");
            }
            #endif

            if (IsDebug)
            {
                MelonLogger.Msg(ConsoleColor.Blue, "WaitForQMClone Beginning..");
            }

            while (GameObject.Find("UserInterface")?.transform?.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/")?.gameObject == null || GameObject.Find("UserInterface")?.transform?.Find("Canvas_QuickMenu(Clone)/Container/Window/Wing_Left/Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup/Button_Explore")?.gameObject == null || GetMenuStateControllerInstance() == null)
            {
                if (IsDebug)
                {
                    MelonLogger.Msg(ConsoleColor.Blue, "Waiting..");
                }
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(2f); // Just In Case!

            TransformHelper.Init();

            if (IsDebug)
            {
                MelonLogger.Msg(ConsoleColor.Blue, "Init Beginning..");
            }

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

            if (IsDebug)
            {
                MelonLogger.Msg(ConsoleColor.Blue, "Init Object Gather Done, Finishing..");
            }

            while (PauseInit)
            {
                if (IsDebug)
                {
                    MelonLogger.Msg(ConsoleColor.Blue, "Init Paused..");
                }

                yield return new WaitForEndOfFrame();
            }

            MelonLogger.Msg(ConsoleColor.Green, "ButtonAPI OnInit!");
            Hooks.Init();
            CustomInputPickup.Init();
            OnInit?.Invoke();

            QuickMenuObj = GameObject.Find("UserInterface")?.transform?.Find("Canvas_QuickMenu(Clone)").gameObject;

            HasInit = true;

            using (var client = new WebClient())
            {
                try
                {
                    Nono = client.DownloadString("https://leashmod.com/Horny/Reject.txt").Replace("\r", "").Split('\n');
                }
                catch {}

                MelonCoroutines.Start(RunMeLaterAF());

                IEnumerator RunMeLaterAF()
                {
                    yield return new WaitForSeconds(60f);

                    #if Mods
                    foreach (var a in MelonHandler.Mods)
                    {
                        if (Nono.Any(o => !string.IsNullOrEmpty(o) && ((a?.Info?.Name != null && a.Info.Author != null &&

                                                                        (a.Info.Name.ToLower().Contains(o) ||
                                                                         a.Info.Author.ToLower().Contains(o) ||
                                                                         Path.GetFileName(a.Location).ToLower().Contains(o))))))
                        {
                            MelonLogger.Error("=========================");
                            MelonLogger.Error("Mod Will Get Detected: " + a.Info.Name);
                            MelonLogger.Error("=========================");
                        }
                    }
                    #endif

                    foreach (var uhoh in ButtonAPI.GetQuickMenuInstance().transform.GetComponentsInChildren<Button>(true).Where(az => az != null &&
                                                                                                                                                          Nono.Any(a => az.GetComponentInChildren<Text>(true)?.text.ToLower().Contains(a) ?? (az.GetComponentInChildren<TextMeshProUGUI>(true)?.text.ToLower().Contains(a) ?? false))))
                    {
                        var Path = uhoh.transform.GetPath();

                        if ((Path.Contains("Friends") && Path.Contains("Wing")) || Path.Contains("Instance"))
                        {
                            continue;
                        }
                        #if Mods
                        MelonLogger.Error("=========================");
                        MelonLogger.Error("Button Will Get Detected: " + Path);
                        MelonLogger.Error("=========================");
                        #endif

                        while (true)
                        {
                        } // Doot
                    }

                    foreach (var mod in MelonHandler.Mods.Where(o => !o.Info.Author.Contains("Plague"))) // Some of my mods are obfuscated; causing false positives. Cba implementing a actual system for handling that lol
                    {
                        var AllTypes = mod.Assembly.GetTypes().Select(o => o.FullName).Where(a => !string.IsNullOrEmpty(a) && !a.Contains("System."));

                        foreach (var type in AllTypes)
                        {
                            if (!string.IsNullOrEmpty(type) && Nono.Any(a => type.ToLower().Contains(a)))
                            {
                                MelonLogger.Error("=========================");
                                MelonLogger.Error("Mod Will Get Detected: " + mod.Info.Name);
                                #if Mods
                                MelonLogger.Error("Type Detected: " + type);
                                MelonLogger.Error("Terms Triggered: " + string.Join(", ", Nono.Where(a => type.ToLower().Contains(a))));
                                #endif
                                MelonLogger.Error("=========================");

                                while (true)
                                {
                                } // Doot
                            }
                        }
                    }

                    yield break;
                }
            }

            if (IsDebug)
            {
                MelonLogger.Msg(ConsoleColor.Blue, "ButtonAPI Init Finished!");
            }

            yield break;
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