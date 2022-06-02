using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using VRC;
using VRC.UI.Elements.Controls;
using Color = UnityEngine.Color;
using Object = UnityEngine.Object;

namespace PlagueButtonAPI.Misc
{
    public static class Extensions
    {
        private static MethodInfo ourShowAlertMethod;

        private static MethodInfo ourShowConfirmDialogMethod;

        private static MethodInfo ourAskConfirmOpenURLMethod;

        public static MethodInfo ShowConfirmDialogMethod
        {
            get
            {
                if (ourShowConfirmDialogMethod != null)
                {
                    return ourShowConfirmDialogMethod;
                }

                ourShowConfirmDialogMethod = typeof(VRC.UI.Elements.QuickMenu).GetMethods().First(it => it.GetParameters().Length == 6 && it.Name.Contains("_String_String_String_String_Action_Action_") && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "ConfirmDialog"));

                return ourShowConfirmDialogMethod;      
            }
        }

        public static MethodInfo AskConfirmOpenURLMethod
        {
            get
            {
                if (ourAskConfirmOpenURLMethod != null)
                {
                    return ourAskConfirmOpenURLMethod;
                }

                ourAskConfirmOpenURLMethod = typeof(VRC.UI.Elements.QuickMenu).GetMethods().First(it => it.GetParameters().Length == 1 && it.GetParameters().First().ParameterType == typeof(string) && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject() != null && jt.ReadAsObject().ToString().Contains("This link will open in your web browser.")));

                return ourAskConfirmOpenURLMethod;
            }
        }

        private static MethodInfo _reloadAvatarMethod;
        private static MethodInfo LoadAvatarMethod
        {
            get
            {
                if (_reloadAvatarMethod == null)
                {
                    _reloadAvatarMethod = typeof(VRCPlayer).GetMethods().First(mi => mi.Name.StartsWith("Method_Private_Void_Boolean_") && mi.Name.Length < 31 && mi.GetParameters().Any(pi => pi.IsOptional) && Utils.CheckUsedBy(mi, "ReloadAvatarNetworkedRPC"));
                }

                return _reloadAvatarMethod;
            }
        }

        public static void ReloadAvatar(this VRCPlayer instance)
        {
            LoadAvatarMethod.Invoke(instance, new object[] { true }); // parameter is forceLoad and has to be true
        }

        public static void ReloadAllAvatars(this VRCPlayer instance, bool ignoreSelf = false)
        {
            foreach (var player in PlayerManager.prop_PlayerManager_0.GetPlayers())
            {
                if (!ignoreSelf || player._vrcplayer != VRCPlayer.field_Internal_Static_VRCPlayer_0)
                    ReloadAvatar(player._vrcplayer);
            }
        }

        private delegate void CloseUiDelegate(VRCUiManager uiManager, bool what, bool what2);
        private static CloseUiDelegate _closeUi;

        public static void CloseUi(this VRCUiManager uiManager)
        {
            if (_closeUi == null)
            {
                _closeUi = (CloseUiDelegate)Delegate.CreateDelegate(typeof(CloseUiDelegate),
                    typeof(VRCUiManager).GetMethods().FirstOrDefault(m => m.Name.StartsWith("Method_Public_Void_Boolean_Boolean") && !m.Name.Contains("PDM") && Utils.CheckUsing(m, "TrimCache")));
            }

            _closeUi(uiManager, true, false);
        }

        private delegate void ShowUiDelegate(VRCUiManager uiManager, bool showDefaultScreen, bool showBackdrop);
        private static ShowUiDelegate _showUi;

        public static void ShowUi(this VRCUiManager uiManager, bool showDefaultScreen = true, bool showBackdrop = true)
        {
            if (_showUi == null)
            {
                _showUi = (ShowUiDelegate)Delegate.CreateDelegate(typeof(ShowUiDelegate), typeof(VRCUiManager).GetMethods()
                    .First(mb => mb.Name.StartsWith("Method_Public_Void_Boolean_Boolean_") && !mb.Name.Contains("_PDM_") && Utils.CheckMethod(mb, "UserInterface/MenuContent/Backdrop/Backdrop")));
            }

            _showUi(uiManager, showDefaultScreen, showBackdrop);
        }

        public static void QueueHudMessage(this VRCUiManager uiManager, string notification, Color color, float duration = 5f,
                                           float delay = 0f)
        {
            uiManager.field_Public_Text_0.color = color;
            uiManager.field_Public_Text_0.text = ""; // Empty This!
            uiManager.field_Private_Single_0 = 0f;
            uiManager.field_Private_Single_1 = duration;
            uiManager.field_Private_Single_2 = delay;

            uiManager.field_Private_List_1_String_0.Add(notification);
        }

        public static Player[] GetPlayers(this PlayerManager playerManager)
        {
            return playerManager.field_Private_List_1_Player_0.ToArray();
        }

        public static Player GetPlayer(this PlayerManager playerManager, string userId)
        {
            foreach (var player in playerManager.GetPlayers())
            {
                if (player == null)
                    continue;

                var apiUser = player.field_Private_APIUser_0;
                if (apiUser == null)
                    continue;

                if (apiUser.id == userId)
                    return player;
            }

            return null;
        }

        public static Player GetPlayer(this PlayerManager playerManager, int actorNr)
        {
            foreach (var player in playerManager.GetPlayers())
            {
                if (player == null)
                    continue;
                if (player.prop_Int32_0 == actorNr)
                    return player;
            }

            return null;
        }

        public static T GetOrAddComponent<T>(this GameObject obj) where T : Behaviour
        {
            T comp;

            try
            {
                comp = obj.GetComponent<T>();

                if (comp == null)
                {
                    comp = obj.AddComponent<T>();
                }
            }
            catch
            {
                comp = obj.AddComponent<T>();
            }

            return comp;
        }

        public static T GetOrAddComponent<T>(this Transform obj) where T : Behaviour
        {
            T comp;

            try
            {
                comp = obj.gameObject.GetComponent<T>();

                if (comp == null)
                {
                    comp = obj.gameObject.AddComponent<T>();
                }
            }
            catch
            {
                comp = obj.gameObject.AddComponent<T>();
            }

            return comp;
        }

        public static GameObject FindObject(this GameObject parent, string name)
        {
            Transform[] array = parent.GetComponentsInChildren<Transform>(true);
            foreach (var transform in array)
            {
                if (transform.name == name)
                {
                    return transform.gameObject;
                }
            }

            return null;
        }

        public static string GetPath(this GameObject gameObject)
        {
            var text = "/" + gameObject.name;
            while (gameObject.transform.parent != null)
            {
                gameObject = gameObject.transform.parent.gameObject;
                text = "/" + gameObject.name + text;
            }

            return text;
        }

        public static void DestroyChildren(this Transform transform, Func<Transform, bool> exclude)
        {
            foreach (var child in transform.GetComponentsInChildren<Transform>(true).Where(o => o != transform))
            {
                if (child != null)
                {
                    if (exclude == null || !exclude(child))
                    {
                        Object.Destroy(child.gameObject);
                    }
                }
            }
        }

        public static void DestroyChildren(this Transform transform)
        {
            transform.DestroyChildren(null);
        }

        public static Vector3 SetX(this Vector3 vector, float x)
        {
            return new Vector3(x, vector.y, vector.z);
        }

        public static Vector3 SetY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }

        public static Vector3 SetZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        public static float RoundAmount(this float i, float nearestFactor)
        {
            return (float)Math.Round(i / nearestFactor) * nearestFactor;
        }

        public static Vector3 RoundAmount(this Vector3 i, float nearestFactor)
        {
            return new Vector3(i.x.RoundAmount(nearestFactor), i.y.RoundAmount(nearestFactor), i.z.RoundAmount(nearestFactor));
        }

        public static Vector2 RoundAmount(this Vector2 i, float nearestFactor)
        {
            return new Vector2(i.x.RoundAmount(nearestFactor), i.y.RoundAmount(nearestFactor));
        }

        public static Sprite ToSprite(this Texture2D tex)
        {
            var rect = new Rect(0f, 0f, tex.width, tex.height);
            var pivot = new Vector2(0.5f, 0.5f);
            var border = Vector4.zero;
            var sprite = Sprite.CreateSprite_Injected(tex, ref rect, ref pivot, 50f, 0u, SpriteMeshType.FullRect, ref border, false);
            sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return sprite;
        }

        public static string ReplaceFirst(this string text, string search, string replace)
        {
            var num = text.IndexOf(search);
            if (num < 0)
            {
                return text;
            }

            return text.Substring(0, num) + replace + text.Substring(num + search.Length);
        }

        public static ColorBlock SetColor(this ColorBlock block, Color color)
        {
            var result = default(ColorBlock);
            result.colorMultiplier = block.colorMultiplier;
            result.disabledColor = Color.grey;
            result.highlightedColor = color;
            result.normalColor = color / 1.5f;
            result.pressedColor = Color.white;
            result.selectedColor = color / 1.5f;
            return result;
        }

        public static void DelegateSafeInvoke(this Delegate @delegate, params object[] args)
        {
            var invocationList = @delegate.GetInvocationList();
            for (var i = 0; i < invocationList.Length; i++)
            {
                try
                {
                    invocationList[i].DynamicInvoke(args);
                }
                catch (Exception ex)
                {
                    MelonLogger.Error("Error while executing delegate:\n" + ex);
                }
            }
        }

        public static string ToEasyString(this TimeSpan timeSpan)
        {
            if (Mathf.FloorToInt(timeSpan.Ticks / 864000000000L) > 0)
            {
                return $"{timeSpan:%d} days";
            }

            if (Mathf.FloorToInt(timeSpan.Ticks / 36000000000L) > 0)
            {
                return $"{timeSpan:%h} hours";
            }

            if (Mathf.FloorToInt(timeSpan.Ticks / 600000000) > 0)
            {
                return $"{timeSpan:%m} minutes";
            }

            return $"{timeSpan:%s} seconds";
        }

        public static void ShowAlert(this VRC.UI.Elements.QuickMenu qm, string message)
        {
            if (ourShowAlertMethod == null)
            {
                var methods = typeof(ModalAlert).GetMethods();

                foreach (var methodInfo in methods)
                {
                    if (methodInfo.Name.Contains("Method_Private_Void_") && !methodInfo.Name.Contains("PDM") && methodInfo.GetParameters().Length == 0)
                    {
                        qm.field_Private_ModalAlert_0.field_Private_String_0 = message;

                        methodInfo.Invoke(qm.field_Private_ModalAlert_0, null);

                        if (qm.transform.Find("Container/Window/QMParent/Modal_Alert/Background_Alert").gameObject.activeSelf)
                        {
                            ourShowAlertMethod = methodInfo;
                            break;
                        }
                    }
                }
            }
            else
            {
                qm.field_Private_ModalAlert_0.field_Private_String_0 = message;
                ourShowAlertMethod.Invoke(qm.field_Private_ModalAlert_0, null);
            }
        }

        public static void ShowOKDialog(this VRC.UI.Elements.QuickMenu qm, string title, string message, Action okButton = null)
        {
            qm.ShowConfirmDialog(title, message, okButton, null, true);
        }

        public static void ShowConfirmDialog(this VRC.UI.Elements.QuickMenu qm, string title, string message, Action yesOkayButton = null, Action noButton = null, bool OneButton = false)
        {
            ShowConfirmDialogMethod.Invoke(qm, new object[]
            {
                title,
                message,
                OneButton ? null : "Yes",
                OneButton ? "Okay" : "No",
                OneButton ? null : DelegateSupport.ConvertDelegate<Il2CppSystem.Action>(yesOkayButton),
                OneButton ? DelegateSupport.ConvertDelegate<Il2CppSystem.Action>(yesOkayButton) : DelegateSupport.ConvertDelegate<Il2CppSystem.Action>(noButton)
            });
        }

        public static void ShowConfirmDialogCustomText(this VRC.UI.Elements.QuickMenu qm, string title, string message, string ConfirmButtonText, string CancelButtonText, Action ConfirmButtonAction = null, Action CancelButtonAction = null)
        {
            ShowConfirmDialogMethod.Invoke(qm, new object[]
            {
                title,
                message,
                ConfirmButtonText,
                CancelButtonText,
                DelegateSupport.ConvertDelegate<Il2CppSystem.Action>(ConfirmButtonAction),
                DelegateSupport.ConvertDelegate<Il2CppSystem.Action>(CancelButtonAction)
            });
        }

        public static void AskConfirmOpenURL(this VRC.UI.Elements.QuickMenu qm, string url)
        {
            AskConfirmOpenURLMethod.Invoke(qm, new object[] { url });
        }

        public static void ShowInputDialog(this VRC.UI.Elements.QuickMenu qm, string title, string Placeholder, Action<string> OnConfirm)
        {
            var InputtedText = "";

            ButtonAPI.GetQuickMenuInstance().ShowConfirmDialogCustomText(title, "", "Confirm", "Cancel", () =>
            {
                // Confirm
                OnConfirm?.Invoke(InputtedText);
            });

            var ModalObj = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Modal_ConfirmDialog");

            var ModalPanel = ModalObj.transform.Find("MenuPanel");

            var InputFieldObj = ModalPanel.Find("InputField(Clone)")?.GetComponent<InputField>() ?? UnityEngine.Object.Instantiate(VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.field_Public_VRCUiPopupInput_0.GetComponentInChildren<InputField>());

            InputFieldObj.transform.SetParent(ModalPanel);
            InputFieldObj.transform.localPosition = new Vector3(0f, 70f, 0f);
            InputFieldObj.transform.localScale = Vector3.one;
            InputFieldObj.transform.localRotation = Quaternion.EulerRotation(0f, 0f, 0f);

            InputFieldObj.transform.Find("Placeholder").GetComponent<Text>().text = Placeholder;

            InputFieldObj.onValueChanged = new InputField.OnChangeEvent();
            InputFieldObj.onValueChanged.AddListener(new Action<string>(s =>
            {
                InputFieldObj.transform.Find("Placeholder").gameObject.SetActive(s.Length == 0);
                InputtedText = s;
            }));

            var FreezeHandler = InputFieldObj.gameObject.GetOrAddComponent<FreezeControls>();
            FreezeHandler.OnEnterKeyPressed = () =>
            {
                ModalPanel.Find("Buttons/Button_Yes").GetComponent<Button>().onClick?.Invoke();
            };
            FreezeHandler.OnExit = () =>
            {
                Object.Destroy(InputFieldObj.gameObject);
            };
        }

        public static Sprite LoadSpriteFromDisk(this string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            byte[] data = File.ReadAllBytes(path);

            if (data == null || data.Length <= 0)
            {
                return null;
            }

            Texture2D tex = new Texture2D(512, 512);

            if (!Il2CppImageConversionManager.LoadImage(tex, data))
            {
                return null;
            }

            Sprite sprite = Sprite.CreateSprite(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f, 0, 0, new Vector4(), false);

            sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            return sprite;
        }

        public static Sprite GetSpriteFromResource(this Bitmap resource)
        {
            var ms = new MemoryStream();
            resource.Save(ms, resource.RawFormat);

            var tex = new Texture2D(resource.Width, resource.Height);
            Il2CppImageConversionManager.LoadImage(tex, ms.ToArray());

            Sprite sprite;
            {
                var size = new Rect(0.0f, 0.0f, tex.width, tex.height);
                var pivot = new Vector2(0.5f, 0.5f);
                var border = Vector4.zero;
                sprite = Sprite.CreateSprite_Injected(tex, ref size, ref pivot, 100.0f, 0u, SpriteMeshType.Tight, ref border, false);
            }

            return sprite;
        }

        public static Quaternion LookAtThisWithoutRef(this Transform transform, Vector3 FromThisPosition)
        {
            var obj = new GameObject("TempObj");
            obj.transform.position = FromThisPosition;

            obj.transform.LookAt(transform);

            var rot = obj.transform.localRotation;

            Object.Destroy(obj);

            return rot;
        }

        public static Quaternion FlipX(this Quaternion rot) => new Quaternion(-rot.x, rot.y, rot.z, rot.w);
        public static Quaternion FlipY(this Quaternion rot) => new Quaternion(rot.x, -rot.y, rot.z, rot.w);
        public static Quaternion FlipZ(this Quaternion rot) => new Quaternion(rot.x, rot.y, -rot.z, rot.w);

        public static Quaternion Combine(this Quaternion rot1, Quaternion rot2) => new Quaternion(rot1.x + rot2.x, rot1.y + rot2.y, rot1.z + rot2.z, rot1.w + rot2.w);

        public static Transform[] GetChildren(this Transform transform)
        {
            var Children = new List<Transform>();

            for (var i = 0; i < transform.childCount; i++)
            {
                Children.Add(transform.GetChild(i));
            }

            return Children.ToArray();
        }

        public static Transform[] GetAllChildren(this Transform transform)
        {
            var Children = new List<Transform>();

            void GetChildrenR(Transform trans)
            {
                for (var i = 0; i < trans.childCount; i++)
                {
                    Children.Add(trans.GetChild(i));

                    GetChildren(trans.GetChild(i));
                }
            }

            GetChildrenR(transform);

            return Children.ToArray();
        }

        public static string GetPath(this Transform transform)
        {
            var path = $"{transform.name}";

            var CurrentObj = transform;

            while (CurrentObj.parent != null)
            {
                CurrentObj = CurrentObj.parent;

                path = $"{CurrentObj.name}/" + path;
            }

            return path;
        }
    }

    [RegisterTypeInIl2Cpp]
    internal class FreezeControls : MonoBehaviour
    {
        public FreezeControls(IntPtr instance) : base(instance) { }

        internal Action OnExit;
        internal Action OnEnterKeyPressed;

        internal InputField inputField;

        void Start()
        {
            inputField = gameObject.GetComponent<InputField>();
        }

        void OnEnable()
        {
            VRCInputManager.Method_Public_Static_Void_Boolean_0(true);
            ButtonAPI.GetQuickMenuInstance().field_Private_Boolean_5 = true;
        }

        void OnDisable()
        {
            VRCInputManager.Method_Public_Static_Void_Boolean_0(false);
            ButtonAPI.GetQuickMenuInstance().field_Private_Boolean_5 = false;

            OnExit?.Invoke();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                VRCInputManager.Method_Public_Static_Void_Boolean_0(false);
                ButtonAPI.GetQuickMenuInstance().field_Private_Boolean_5 = false;
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                OnEnterKeyPressed?.Invoke();
            }
        }
    }
}