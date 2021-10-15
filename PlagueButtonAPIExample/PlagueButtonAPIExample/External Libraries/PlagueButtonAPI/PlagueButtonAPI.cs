using IL2CPPAssetBundleAPI;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;
using VRC.UI.Elements;

namespace PlagueButtonAPI
{
    #region PlagueButtonAPI - Created By Plague
    internal class ButtonAPI
    {
        #region Creditation And Disclaimer
#pragma warning disable 414

        private static readonly string Creditation =
        "Plague Button API" +
        "https://VRCAntiCrash.com" +
        "Copyright Reserved" +
        "Use-Only Licensed" +
        "https://github.com/PlagueVRC/PlagueButtonAPI" +
        "Removal Or Modification Of This String Breaches The License." +
        "This String Is To Be Preserved AS IS.";

#pragma warning restore 414
        #endregion

        #region internal Variables

        internal static Transform ShortcutMenuTransform = null;

        internal static Transform NewElementsMenuTransform = null;

        internal static QuickMenu QuickMenuObj = null;

        internal static Transform UserInteractMenuTransform = null;

        internal static Transform CustomTransform = null;

        #endregion

        #region Main Functions

        #region Button Creation

        private static List<Action> QueuedButtons = new List<Action>();

        private static bool HasInit = false;
        private static bool HasRanCoroutine = false;

        internal static void CreateButton(Transform Parent, string ObjectName, string Text, string ToolTip, Action OnClick)
        {
            if (!HasInit)
            {
                if (!HasRanCoroutine)
                {
                    MelonCoroutines.Start(CreateButton());

                    IEnumerator CreateButton()
                    {
                        HasRanCoroutine = true;

                        while (FindOrNull("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/Wing_Left/Container/InnerContainer/Emotes/ScrollRect/Viewport/VerticalLayoutGroup/Emotes_SDK2/Button_Wave")?.transform == null)
                        {
                            yield return new WaitForEndOfFrame();
                        }

                        if (QueuedButtons.Count > 0)
                        {
                            MelonLogger.Warning("Creating " + QueuedButtons.Count + " Queued Buttons..");
                        }

                        foreach (var Button in QueuedButtons)
                        {
                            Button.Invoke();
                        }

                        if (QueuedButtons.Count > 0)
                        {
                            MelonLogger.Warning("Done!");
                        }

                        QueuedButtons.Clear();

                        HasInit = true;

                        yield break;
                    }
                }

                QueuedButtons.Add(new Action(() =>
                {
                    MakeButton();
                }));
            }
            else
            {
                MakeButton();
            }

            void MakeButton()
            {
                var NewButton = UnityEngine.Object.Instantiate(FindOrNull("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/Wing_Left/Container/InnerContainer/Emotes/ScrollRect/Viewport/VerticalLayoutGroup/Emotes_SDK2/Button_Wave").transform);

                NewButton.SetParent(Parent.FindOrNull("ScrollRect/Viewport/VerticalLayoutGroup"));

                NewButton.gameObject.SetActive(true);

                UnityEngine.Object.Destroy(NewButton.GetComponent<VRC.UI.Elements.Controls.EmoteWingButton>());

                RepairShit(FindOrNull("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/Wing_Left/Container/InnerContainer/Emotes/ScrollRect/Viewport/VerticalLayoutGroup/Emotes_SDK2/Button_Wave").transform, NewButton);

                NewButton.name = ObjectName;

                NewButton.FindOrNull("Icon").gameObject.SetActive(false);

                NewButton.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 88f);

                var TextObj = NewButton.FindOrNull("Text_H4");
                TextObj.GetComponent<TextMeshProUGUI>().text = Text;
                TextObj.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 48f);

                TextObj.localPosition = new Vector3(0f, -25f, 0.0004f);

                NewButton.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>().text = (ToolTip);

                var ButtonComp = NewButton.GetComponent<Button>();
                ButtonComp.onClick = new Button.ButtonClickedEvent();
                if (OnClick != null)
                {
                    ButtonComp.onClick.AddListener(OnClick);
                }
            }
        }

        private static IL2CPPAssetBundle OurBundle = new IL2CPPAssetBundle();
        private static Sprite Unchecked_Checkbox;
        private static Sprite Checked_Checkbox;
        internal static void CreateToggle(Transform Parent, string ObjectName, string Text, string ToolTip, Action<bool> OnToggle, bool DefaultState, Color? CheckboxColour = null)
        {
            if (!HasInit)
            {
                if (!HasRanCoroutine)
                {
                    MelonCoroutines.Start(CreateButton());

                    IEnumerator CreateButton()
                    {
                        HasRanCoroutine = true;

                        while (FindOrNull("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/Wing_Left/Container/InnerContainer/Emotes/ScrollRect/Viewport/VerticalLayoutGroup/Emotes_SDK2/Button_Wave")?.transform == null)
                        {
                            yield return new WaitForEndOfFrame();
                        }

                        if (QueuedButtons.Count > 0)
                        {
                            MelonLogger.Warning("Creating " + QueuedButtons.Count + " Queued Buttons..");
                        }

                        foreach (var Button in QueuedButtons)
                        {
                            Button.Invoke();
                        }

                        if (QueuedButtons.Count > 0)
                        {
                            MelonLogger.Warning("Done!");
                        }

                        QueuedButtons.Clear();

                        HasInit = true;

                        yield break;
                    }
                }

                QueuedButtons.Add(new Action(() =>
                {
                    MakeButton();
                }));
            }
            else
            {
                MakeButton();
            }

            void MakeButton()
            {
                var NewButton = UnityEngine.Object.Instantiate(FindOrNull("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/Wing_Left/Container/InnerContainer/Emotes/ScrollRect/Viewport/VerticalLayoutGroup/Emotes_SDK2/Button_Wave").transform);

                NewButton.SetParent(Parent.FindOrNull("ScrollRect/Viewport/VerticalLayoutGroup"));

                NewButton.gameObject.SetActive(false);

                UnityEngine.Object.Destroy(NewButton.GetComponent<VRC.UI.Elements.Controls.EmoteWingButton>());

                RepairShit(FindOrNull("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/Wing_Left/Container/InnerContainer/Emotes/ScrollRect/Viewport/VerticalLayoutGroup/Emotes_SDK2/Button_Wave").transform, NewButton);

                NewButton.name = ObjectName;

                NewButton.FindOrNull("Icon").gameObject.SetActive(false);

                NewButton.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 88f);

                var TextObj = NewButton.FindOrNull("Text_H4");
                TextObj.GetComponent<TextMeshProUGUI>().text = Text;
                TextObj.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 48f);

                TextObj.localPosition = new Vector3(0f, -25f, 0.0004f);

                NewButton.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>().text = (ToolTip);

                var IconObj = NewButton.FindOrNull("Icon_Secondary");
                IconObj.localPosition = new Vector3(-130f, 36.3f, 0);
                IconObj.gameObject.SetActive(true);

                if (Unchecked_Checkbox == null || Checked_Checkbox == null)
                {
                    if (OurBundle.LoadBundle(Assembly.GetExecutingAssembly().GetManifestResourceNames().First(o => o.Contains("PlagueButtonAPIBundle"))))
                    {
                        Unchecked_Checkbox = OurBundle.Load<Sprite>("UnChecked");
                        Checked_Checkbox = OurBundle.Load<Sprite>("Checked");

                        if (Unchecked_Checkbox == null || Checked_Checkbox == null)
                        {
                            MelonLogger.Error($"\nFailed To Load A Sprite!\n\nError:\n{(OurBundle.error ?? "None")}\n\nUnChecked: {(Unchecked_Checkbox == null ? "Null" : "Exists")}\nChecked: {(Checked_Checkbox == null ? "Null" : "Exists")}");
                        }
                    }
                    else
                    {
                        MelonLogger.Error($"Failed To Load Asset Bundle!\n\nBundle: {(OurBundle.bundle == null ? "Null" : "Exists")}\n\nError:\n{(OurBundle.error ?? "None")}");
                    }
                }

                var IconImage = IconObj.GetComponent<Image>();
                IconImage.sprite = DefaultState ? Checked_Checkbox : Unchecked_Checkbox;

                if (CheckboxColour != null)
                {
                    IconObj.gameObject.AddComponent<ObjectHandler>().OnUpdate += (obj) =>
                {
                    var image = obj.GetComponent<Image>();

                    if (image.color != CheckboxColour)
                    {
                        image.color = (Color)CheckboxColour;
                    }
                };
                }

                var ButtonComp = NewButton.GetComponent<Button>();
                ButtonComp.onClick = new Button.ButtonClickedEvent();
                if (OnToggle != null)
                {
                    ButtonComp.onClick.AddListener(new Action(() =>
                    {
                        IconImage.sprite = (IconImage.sprite.name == Unchecked_Checkbox.name ? Checked_Checkbox : Unchecked_Checkbox);

                        if (CheckboxColour != null)
                        {
                            IconImage.color = (Color)CheckboxColour;
                        }

                        OnToggle?.Invoke(IconImage.sprite.name == Checked_Checkbox.name);
                    }));
                }

                NewButton.gameObject.SetActive(true);
            }
        }

        #endregion

        #region Sub Menu Creation And Handling

        internal static void InitTransforms()
        {
            if (QuickMenuObj != null)
            {
                return;
            }

            QuickMenuObj = Resources.FindObjectsOfTypeAll<QuickMenu>().FirstOrDefault();

            if (QuickMenuObj != null)
            {
                ShortcutMenuTransform = QuickMenuObj.transform;

                if (CustomTransform == null)
                {
                    CustomTransform = ShortcutMenuTransform;
                }

                if (NewElementsMenuTransform == null)
                {
                    NewElementsMenuTransform = QuickMenuObj.transform.FindOrNull("Container/Window/").transform;
                }

                if (UserInteractMenuTransform == null)
                {
                    UserInteractMenuTransform = QuickMenuObj.transform.FindOrNull("Container/Window/QMParent/Menu_SelectedUser_Local/").transform;
                }
            }
        }

        /// <summary>
        /// Creates A Empty Page For Adding Buttons To, If The Page Already Exists, This Will Return It. | Created By Plague | Discord Server: http://VRCAntiCrash.com
        /// </summary>
        /// <param name="wing">Which Wing To Add This Page To.</param>
        /// <param name="name">The Name You Want To Give The Page/Find Internally.</param>
        /// <param name="PageText">Text To Display At The Top Of The Page And On The Button To Enter It.</param>
        /// <param name="PageTooltip">Text To Display When Hovering Over The Text Defined Just Before This.</param>
        /// <param name="OptionalTitleTextOnColour">Optional Toggled On Colour Of The Text Defined Previous.</param>
        /// <param name="OptionalTitleTextOffColour">Optional Toggled Off Colour Of The Text Defined Previous.</param>
        /// <param name="OptionalTitleTextOnClick">Optional Function To Run On Selecting The Text Defined Previous</param>
        /// <returns></returns>
        internal static GameObject MakeEmptyPage(Wing wing, string name, string PageText, string PageTooltip, Color? OptionalTitleTextOnColour = null, Color? OptionalTitleTextOffColour = null, Action<bool> OptionalTitleTextOnClick = null)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                MelonLogger.Msg("Your Page Internal Name Cannot Be Empty!");
                return null;
            }

            InitTransforms();

            var info = Assembly.GetExecutingAssembly().GetCustomAttribute<MelonInfoAttribute>();
            for (var i = 0; i < SubMenus.Count; i++)
            {
                var menu = SubMenus[i];

                if (menu.name == "PlagueButtonAPI_SubMenu_" + info.Name.Replace(" ", "_") + " By " + info.Author.Replace(" ", "_") + "_" + name + Enum.GetName(typeof(Wing), wing))
                {
                    return menu;
                }
            }

            var LeftWingButtonsArea = QuickMenuObj.transform.FindOrNull("Container/Window/Wing_" + Enum.GetName(typeof(Wing), wing) + "/Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup");

            //Testing Only
            LeftWingButtonsArea.FindOrNull("Button_Explore").gameObject.SetActive(true);

            //Dupe Button
            var transform = UnityEngine.Object.Instantiate(LeftWingButtonsArea.FindOrNull("Button_Explore"));

            //Enable Said Dupe
            transform.gameObject.SetActive(true);

            //Namings
            transform.transform.name = "PlagueButtonAPI_SubMenu_" + info.Name.Replace(" ", "_") + " By " + info.Author.Replace(" ", "_") + "_" + name + "_" + Enum.GetName(typeof(Wing), wing);
            transform.name = "PlagueButtonAPI_SubMenu_" + info.Name.Replace(" ", "_") + " By " + info.Author.Replace(" ", "_") + "_" + name + "_" + Enum.GetName(typeof(Wing), wing);

            //Parenting
            transform.SetParent(LeftWingButtonsArea.transform, true);

            //Set Text
            transform.FindOrNull("Container/Text_QM_H3").GetComponent<TextMeshProUGUI>().text = PageText;

            //Set ToolTip
            var ToolTip = transform.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>();
            ToolTip.text = (PageTooltip);
            ToolTip.alternateText = (PageTooltip);

            //Set ID, Keep Component For Init To Fix localScale Bug
            var StyleElem = transform.GetComponent<StyleElement>();
            StyleElem.id = (name + "_" + Enum.GetName(typeof(Wing), wing));

            transform.localScale = new Vector3(1f, 1f, 1f);
            StyleElem.InitStyles();

            //Dupe Menu
            var MenuTransform = UnityEngine.Object.Instantiate(QuickMenuObj.transform.FindOrNull("Container/Window/Wing_" + Enum.GetName(typeof(Wing), wing) + "/Container/InnerContainer/Explore")).transform;

            //Name Menu Obj
            MenuTransform.name = name + "_" + Enum.GetName(typeof(Wing), wing);

            //Menu Parenting
            MenuTransform.SetParent(QuickMenuObj.transform.FindOrNull("Container/Window/Wing_" + Enum.GetName(typeof(Wing), wing) + "/Container/InnerContainer"), false);

            //Change Menu Internal Name
            MenuTransform.GetComponent<UIPage>().Name = (name + "_" + Enum.GetName(typeof(Wing), wing));

            //localScale Bug Fixing, Also Fix Any localPosition Bugs
            RepairShit(QuickMenuObj.transform.FindOrNull("Container/Window/Wing_" + Enum.GetName(typeof(Wing), wing) + "/Container/InnerContainer/Explore"), MenuTransform);

            //Set Menu Title Text
            MenuTransform.FindOrNull("WngHeader_H1/LeftItemContainer/Text_Title").GetComponent<TextMeshProUGUI>().text = PageText;

            var Layout = MenuTransform.FindOrNull("ScrollRect/Viewport/VerticalLayoutGroup").GetComponent<VerticalLayoutGroup>();
            Layout.childControlWidth = false;
            Layout.childControlHeight = false;

            Layout.childScaleWidth = false;
            Layout.childScaleHeight = false;

            Layout.childForceExpandWidth = false;
            Layout.childForceExpandHeight = false;

            Layout.spacing = 25f;

            Layout.padding.top = 12;

            MenuTransform.FindOrNull("ScrollRect/Viewport").localPosition -= new Vector3(0f, 1f);

            //OnClick
            var button = transform.GetComponent<Button>();
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(new Action(() =>
            {
                MelonLogger.Msg("Button Click!");

                //Cache Our UIPage
                var OurPage = QuickMenuObj.transform.FindOrNull("Container/Window/Wing_" + Enum.GetName(typeof(Wing), wing) + "/Container/InnerContainer/" + name + "_" + Enum.GetName(typeof(Wing), wing)).GetComponent<UIPage>();

                //Cache Whatever Page Is Open That Is NOT OurPage
                var ShownPage = QuickMenuObj.transform.FindOrNull("Container/Window/Wing_" + Enum.GetName(typeof(Wing), wing) + "/Container/InnerContainer").GetComponentsInChildren<UIPage>(false).FirstOrDefault(o => o != null && o != OurPage && o.IsStackShown());

                //In Case
                if (ShownPage != null)
                {
                    MelonLogger.Msg("Hiding ShownPage: " + ShownPage.gameObject.name + "!");
                    ShownPage.Show(false, UIPage.TransitionType.None);
                }

                //Call OnPageShown
                OurPage.OnPageShown();

                //Show Our Page
                OurPage.Show(true, UIPage.TransitionType.Right);

                //In Case
                if (ShownPage != null)
                {
                    MelonLogger.Msg("Pushing!");
                    ShownPage.PushPage(OurPage);
                    ShownPage.OnHideComplete();
                }
                else
                {
                    MelonLogger.Msg("Self Pushing!"); // Likely Doesn't Work; Only Here For Edge Cases Anyways
                    OurPage.PushPage(OurPage);
                }

                OurPage.OnShowComplete();
            }));

            return MenuTransform.gameObject;
        }

        #endregion

        #endregion

        #region Enums

        internal enum Wing
        {
            Left,
            Right
        }

        #endregion Internal Enumerations

        #region Internal Things - Not For The End User

        //Any Created Sub Menus By The User Are Stored Here
        internal static List<GameObject> SubMenus = new List<GameObject>();

        #endregion

        #region Helpers

        internal static GameObject FindOrNull(string path)
        {
            try
            {
                return GameObject.Find(path);
            }
            catch
            {

            }

            return null;
        }

        //This Assumes Exact Hierarchy Identicality
        internal static void RepairShit(Transform Template, Transform Dupe, bool Debug = false, bool Initial = true)
        {
            if (Initial)
            {
                Dupe.position = Template.position;
                Dupe.rotation = Template.rotation;
                Dupe.localScale = Template.localScale;
            }

            for (var j = 0; j < Template.childCount; j++)
            {
                var TemplateChild = Template.GetChild(j);

                for (var i = 0; i < Dupe.childCount; i++)
                {
                    var DupeChild = Dupe.GetChild(i);

                    if (TemplateChild.name == DupeChild.name)
                    {
                        DupeChild.position = TemplateChild.position;
                        DupeChild.rotation = TemplateChild.rotation;
                        DupeChild.localScale = TemplateChild.localScale;

                        if (Debug)
                        {
                            MelonLogger.Msg("Known Object Comparison: " + TemplateChild.name + ", " + DupeChild.name); //Debug
                        }
                    }
                    else if (Debug)
                    {
                        MelonLogger.Warning("Unknown Object Comparison: " + TemplateChild.name + ", " + DupeChild.name); //Debug
                    }

                    RepairShit(TemplateChild, DupeChild, Debug, false);
                }
            }
        }

        #endregion
    }

    #region Extension Methods
    internal static class ButtonAPIExtensions
    {
        internal static Transform FindOrNull(this Transform transform, string path)
        {
            try
            {
                return transform.Find(path);
            }
            catch
            {
            }
            return null;
        }
    }
    #endregion

    #region Custom Components

    [RegisterTypeInIl2Cpp]
    internal class ObjectHandler : MonoBehaviour
    {
        public ObjectHandler(IntPtr instance) : base(instance) { }

        public Action<GameObject> OnEnabled = null;
        public Action<GameObject> OnDisabled = null;
        public Action<GameObject> OnDestroyed = null;
        public Action<GameObject> OnUpdate = null;

        void OnEnable()
        {
            OnEnabled?.Invoke(gameObject);
        }

        void OnDisable()
        {
            OnDisabled?.Invoke(gameObject);
        }

        void OnDestroy()
        {
            OnDestroyed?.Invoke(gameObject);
        }

        void Update()
        {
            OnUpdate?.Invoke(gameObject);
        }
    }

    [RegisterTypeInIl2Cpp]
    internal class FreezeControls : MonoBehaviour
    {
        public FreezeControls(IntPtr instance) : base(instance) { }

        internal Action OnExit;
        internal Action OnEnterKeyPressed;

        void OnEnable()
        {
            VRCInputManager.Method_Public_Static_Void_Boolean_PDM_0(true);
        }

        void OnDisable()
        {
            VRCInputManager.Method_Public_Static_Void_Boolean_PDM_0(false);

            OnExit?.Invoke();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                VRCInputManager.Method_Public_Static_Void_Boolean_PDM_0(false);
                VRCUiManager.prop_VRCUiManager_0.Method_Public_Virtual_New_Void_Boolean_0();
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                OnEnterKeyPressed?.Invoke();
            }
        }
    }

    #endregion
    #endregion
}
