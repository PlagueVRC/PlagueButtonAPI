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
using Random = System.Random;

namespace PlagueButtonAPI
{
    #region PlagueButtonAPI - Created By Plague
    public class ButtonAPI : MelonMod
    {
        #region Overrides

        private static bool HasPageInit;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "ui")
            {
                MelonCoroutines.Start(RunMe());

                IEnumerator RunMe()
                {
                    yield return new WaitForSeconds(15f); // I Cba Checking Objects.

                    HasPageInit = true;

                    if (QueuedPages.Count > 0)
                    {
                        MelonLogger.Warning("Creating " + QueuedPages.Count + " Queued Pages..");
                    }

                    foreach (var page in QueuedPages)
                    {
                        page?.Invoke();
                    }

                    if (QueuedPages.Count > 0)
                    {
                        MelonLogger.Warning("Done!");
                    }

                    QueuedPages.Clear();

                    yield break;
                }
            }
        }

        #endregion

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

        #region Public Variables

        public static Transform ShortcutMenuTransform = null;

        public static Transform NewElementsMenuTransform = null;

        public static QuickMenu QuickMenuObj = null;

        public static Transform UserInteractMenuTransform = null;

        public static Transform CustomTransform = null;

        #endregion

        #region Instance Classes

        public class PlagueBase
        {
            public GameObject gameObject;
            public VRC.UI.Elements.Tooltips.UiTooltip tooltip;
        }

        public class PlagueButton : PlagueBase
        {
            public Button button;

            public TextMeshProUGUI mainButtonText;
            
            public Image buttonBackground;
            public Image Icon_Secondary;
            public Image SubMenu_Arrow;
            public Image Close_Badge;
            public Image CancelGif;
        }

        public class PlagueToggle : PlagueButton
        {
            public bool ToggleState => Icon_Secondary.sprite.name == Checked_Checkbox.name;
        }

        public class PlagueSlider : PlagueBase
        {
            public Slider slider;

            public TextMeshProUGUI mainButtonText;

            public Image sliderBackground;
            public Image sliderFill;

            public TextMeshProUGUI SliderPercentage;
            public Image sliderPercentageBackground;
        }

        public class PlaguePage
        {
            public GameObject gameObject;
            public UIPage page;
            public VerticalLayoutGroup layout;
            public Button pageEntryButton;
            public Button pageBackButton;
        }

        #endregion

        #region Main Functions

        #region Control Creation

        private static List<Action> QueuedButtons = new List<Action>();
        private static List<Action> QueuedPages = new List<Action>();

        private static bool HasInit = false;
        private static bool HasRanCoroutine = false;

        /// <summary>
        /// Creates A Button In The Specified Parent Page.
        /// </summary>
        /// <param name="Parent">The Transform Of The Page To Put This In. Use MakeEmptyPage To Obtain This.</param>
        /// <param name="Text">The Main Text On The Button.</param>
        /// <param name="ToolTip">The Text To Display When You Hover Over The Button.</param>
        /// <param name="OnClick">A Method To Be Called On Button-Press. Use () => { } To Create This.</param>
        /// <param name="OnCreation">A Method To Obtain The GameObject Of Your Button Once Made If Needed. This Is A Method Due To PlagueButtonAPI Supporting Queued Creation.</param>
        public static void CreateButton(Transform Parent, string Text, string ToolTip, Action OnClick, Action<PlagueButton> OnCreation = null)
        {
            InitTransforms();

            if (!HasInit)
            {
                if (!HasRanCoroutine)
                {
                    MelonCoroutines.Start(CreateButton());

                    IEnumerator CreateButton()
                    {
                        HasRanCoroutine = true;

                        while (QuickMenuObj.transform.FindOrNull("Container/Window/Wing_Left/Container/InnerContainer/Emotes/ScrollRect/Viewport/VerticalLayoutGroup/Emotes_SDK2/Button_Wave")?.transform == null)
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
                var NewButton = UnityEngine.Object.Instantiate(QuickMenuObj.transform.FindOrNull("Container/Window/Wing_Left/Container/InnerContainer/Emotes/ScrollRect/Viewport/VerticalLayoutGroup/Emotes_SDK2/Button_Wave").transform);

                NewButton.SetParent(Parent.FindOrNull("ScrollRect/Viewport/VerticalLayoutGroup"));

                NewButton.gameObject.SetActive(true);

                UnityEngine.Object.Destroy(NewButton.GetComponent<VRC.UI.Elements.Controls.EmoteWingButton>());

                RepairShit(QuickMenuObj.transform.FindOrNull("Container/Window/Wing_Left/Container/InnerContainer/Emotes/ScrollRect/Viewport/VerticalLayoutGroup/Emotes_SDK2/Button_Wave").transform, NewButton);

                var info = Assembly.GetExecutingAssembly().GetCustomAttribute<MelonInfoAttribute>();

                NewButton.name = "PlagueButtonAPI_Button_" + info.Name.Replace(" ", "_") + " By " + info.Author.Replace(" ", "_") + "_" + Text + "_" + ToolTip + "_" + new Random().Next(0, 999999);

                NewButton.FindOrNull("Icon").gameObject.SetActive(false);

                NewButton.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 88f);

                var TextObj = NewButton.FindOrNull("Text_H4");
                TextObj.GetComponent<TextMeshProUGUI>().text = Text;
                TextObj.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 48f);

                TextObj.localPosition = new Vector3(0f, -24f, 0.0004f);

                NewButton.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>().text = (ToolTip);

                var ButtonComp = NewButton.GetComponent<Button>();
                ButtonComp.onClick = new Button.ButtonClickedEvent();
                if (OnClick != null)
                {
                    ButtonComp.onClick.AddListener(OnClick);
                }

                OnCreation?.Invoke(new PlagueButton
                {
                    button = ButtonComp,
                    buttonBackground = NewButton.FindOrNull("Background").GetComponent<Image>(),
                    CancelGif = NewButton.FindOrNull("Cancel").GetComponent<Image>(),
                    Close_Badge = NewButton.FindOrNull("Badge_Close").GetComponent<Image>(),
                    gameObject = NewButton.gameObject,
                    Icon_Secondary = NewButton.FindOrNull("Icon_Secondary").GetComponent<Image>(),
                    mainButtonText = TextObj.GetComponent<TextMeshProUGUI>(),
                    SubMenu_Arrow = NewButton.FindOrNull("Badge_MMJump").GetComponent<Image>(),
                    tooltip = NewButton.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>()
                });
            }
        }

        private static IL2CPPAssetBundle OurBundle = new IL2CPPAssetBundle();
        public static Sprite Unchecked_Checkbox;
        public static Sprite Checked_Checkbox;
        /// <summary>
        /// Creates A Toggle In The Specified Parent Page.
        /// </summary>
        /// <param name="Parent">The Transform Of The Page To Put This In. Use MakeEmptyPage To Obtain This.</param>
        /// <param name="Text">The Main Text On The Toggle.</param>
        /// <param name="ToolTip">The Text To Display When You Hover Over The Toggle.</param>
        /// <param name="OnToggle">A Method To Be Called On Toggling. Use (val) => { } To Create This.</param>
        /// <param name="DefaultState">The Default Toggle State Of The Toggle.</param>
        /// <param name="RemoveButtonBackground">Whether Or Not To Remove The Blue Button Background And Only Leave A Checkbox And Text.</param>
        /// <param name="CheckboxColour">Change The Colour Of The CheckBox Image; If You Insist..</param>
        /// <param name="OnCreation">A Method To Obtain The GameObject Of Your Toggle Once Made If Needed. This Is A Method Due To PlagueButtonAPI Supporting Queued Creation.</param>
        public static void CreateToggle(Transform Parent, string Text, string ToolTip, Action<bool> OnToggle, bool DefaultState, bool RemoveButtonBackground, Color? CheckboxColour = null, Action<PlagueToggle> OnCreation = null)
        {
            InitTransforms();

            if (!HasInit)
            {
                if (!HasRanCoroutine)
                {
                    MelonCoroutines.Start(CreateButton());

                    IEnumerator CreateButton()
                    {
                        HasRanCoroutine = true;

                        while (QuickMenuObj.transform.FindOrNull("Container/Window/Wing_Left/Container/InnerContainer/Emotes/ScrollRect/Viewport/VerticalLayoutGroup/Emotes_SDK2/Button_Wave")?.transform == null)
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
                var NewButton = UnityEngine.Object.Instantiate(QuickMenuObj.transform.FindOrNull("Container/Window/Wing_Left/Container/InnerContainer/Emotes/ScrollRect/Viewport/VerticalLayoutGroup/Emotes_SDK2/Button_Wave").transform);

                NewButton.SetParent(Parent.FindOrNull("ScrollRect/Viewport/VerticalLayoutGroup"));

                NewButton.gameObject.SetActive(false);

                UnityEngine.Object.Destroy(NewButton.GetComponent<VRC.UI.Elements.Controls.EmoteWingButton>());

                RepairShit(QuickMenuObj.transform.FindOrNull("Container/Window/Wing_Left/Container/InnerContainer/Emotes/ScrollRect/Viewport/VerticalLayoutGroup/Emotes_SDK2/Button_Wave").transform, NewButton);

                var info = Assembly.GetExecutingAssembly().GetCustomAttribute<MelonInfoAttribute>();

                NewButton.name = "PlagueButtonAPI_Toggle_" + info.Name.Replace(" ", "_") + " By " + info.Author.Replace(" ", "_") + "_" + Text + "_" + ToolTip + "_" + new Random().Next(0, 999999);

                NewButton.FindOrNull("Icon").gameObject.SetActive(false);

                NewButton.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 88f);

                var TextObj = NewButton.FindOrNull("Text_H4");
                TextObj.GetComponent<TextMeshProUGUI>().text = Text;
                TextObj.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 48f);

                TextObj.localPosition = new Vector3(0f, -24f, 0.0004f);

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

                if (RemoveButtonBackground)
                {
                    NewButton.FindOrNull("Background").GetComponent<Image>().color = new Color(0, 0, 0, 0);
                }

                NewButton.gameObject.SetActive(true);

                OnCreation?.Invoke(new PlagueToggle
                {
                    button = ButtonComp,
                    buttonBackground = NewButton.FindOrNull("Background").GetComponent<Image>(),
                    CancelGif = NewButton.FindOrNull("Cancel").GetComponent<Image>(),
                    Close_Badge = NewButton.FindOrNull("Badge_Close").GetComponent<Image>(),
                    gameObject = NewButton.gameObject,
                    Icon_Secondary = NewButton.FindOrNull("Icon_Secondary").GetComponent<Image>(),
                    mainButtonText = TextObj.GetComponent<TextMeshProUGUI>(),
                    SubMenu_Arrow = NewButton.FindOrNull("Badge_MMJump").GetComponent<Image>(),
                    tooltip = NewButton.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>()
                });
            }
        }

        /// <summary>
        /// Creates A Slider In The Specified Parent Page.
        /// </summary>
        /// <param name="Parent">The Transform Of The Page To Put This In. Use MakeEmptyPage To Obtain This.</param>
        /// <param name="Text">The Main Text On The Slider.</param>
        /// <param name="ToolTip">The Text To Display When You Hover Over The Slider.</param>
        /// <param name="OnValueChanged">A Method To Be Called On Value Change. Use (val) => { } To Create This.</param>
        /// <param name="DefaultValue">The Default Slider Value.</param>
        /// <param name="MinPossibleValue">What Is The Lowest The Slider Can Go?</param>
        /// <param name="MaxPossibleValue">What Is The Highest The Slider Can Go?</param>
        /// <param name="OnCreation">A Method To Obtain The GameObject Of Your Slider Once Made If Needed. This Is A Method Due To PlagueButtonAPI Supporting Queued Creation.</param>
        public static void CreateSlider(Transform Parent, string Text, string ToolTip, Action<float> OnValueChanged, float DefaultValue, float MinPossibleValue, float MaxPossibleValue, Action<PlagueSlider> OnCreation = null)
        {
            InitTransforms();

            if (!HasInit)
            {
                if (!HasRanCoroutine)
                {
                    MelonCoroutines.Start(CreateButton());

                    IEnumerator CreateButton()
                    {
                        HasRanCoroutine = true;

                        while (QuickMenuObj.transform.FindOrNull("Container/Window/Wing_Left/Container/InnerContainer/Emotes/ScrollRect/Viewport/VerticalLayoutGroup/Emotes_SDK2/Button_Wave")?.transform == null)
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
                var NewSlider = UnityEngine.Object.Instantiate(QuickMenuObj.transform.FindOrNull("Container/Window/Wing_Left/Container/InnerContainer/Emotes/Wing_Menu_RadialPuppet"));

                NewSlider.transform.SetParent(Parent.FindOrNull("ScrollRect/Viewport/VerticalLayoutGroup"));

                var info = Assembly.GetExecutingAssembly().GetCustomAttribute<MelonInfoAttribute>();

                NewSlider.name = "PlagueButtonAPI_Slider_" + info.Name.Replace(" ", "_") + " By " + info.Author.Replace(" ", "_") + "_" + Text + "_" + ToolTip + "_" + new Random().Next(0, 999999);

                RepairShit(QuickMenuObj.transform.FindOrNull("Container/Window/Wing_Left/Container/InnerContainer/Emotes/Wing_Menu_RadialPuppet").transform, NewSlider.transform);

                NewSlider.transform.FindOrNull("Scrim").gameObject.SetActive(false);
                NewSlider.transform.FindOrNull("Container/Icon").gameObject.SetActive(false);

                var ToolTipHandler = NewSlider.gameObject.AddComponent<VRC.UI.Elements.Tooltips.UiTooltip>();
                ToolTipHandler.text = ToolTip;
                ToolTipHandler.alternateText = ToolTip;

                NewSlider.transform.FindOrNull("Container/Title").GetComponent<TMPro.TextMeshProUGUI>().text = Text;

                var SliderComp = NewSlider.transform.FindOrNull("Container/Slider").GetComponent<Slider>();

                NewSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(360f, 100f);
                SliderComp.GetComponent<RectTransform>().sizeDelta = new Vector2(355f, 75f);
                SliderComp.transform.localPosition = new Vector3(0f, -100f, 0.0094f);

                SliderComp.minValue = MinPossibleValue;
                SliderComp.maxValue = MaxPossibleValue;

                var SliderText = NewSlider.transform.FindOrNull("Container/Slider/Text/Value").GetComponent<TMPro.TextMeshProUGUI>();
                SliderComp.onValueChanged = new Slider.SliderEvent();
                SliderComp.onValueChanged.AddListener(new Action<float>((val) =>
                {
                    SliderText.text = RangeConv(val, MinPossibleValue, MaxPossibleValue, 0f, 100f) + "%";
                }));

                SliderComp.Set(DefaultValue);

                SliderComp.onValueChanged.AddListener(OnValueChanged);

                NewSlider.gameObject.SetActive(true);

                OnCreation?.Invoke(new PlagueSlider
                {
                    gameObject = NewSlider.gameObject,
                    mainButtonText = NewSlider.FindOrNull("Container/Title").GetComponent<TMPro.TextMeshProUGUI>(),
                    slider = SliderComp,
                    sliderBackground = SliderComp.transform.FindOrNull("Background").GetComponent<Image>(),
                    sliderFill = SliderComp.transform.FindOrNull("Fill Area/Fill").GetComponent<Image>(),
                    SliderPercentage = SliderComp.transform.FindOrNull("Text/Value").GetComponent<TextMeshProUGUI>(),
                    sliderPercentageBackground = SliderComp.transform.FindOrNull("Text/Image").GetComponent<Image>(),
                    tooltip = NewSlider.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>()
                });
            }
        }

        #endregion

        #region Sub Menu Creation

        public static void InitTransforms()
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
        /// <param name="name">The Name You Want To Give The Page/Find publicly.</param>
        /// <param name="PageText">Text To Display At The Top Of The Page And On The Button To Enter It.</param>
        /// <param name="PageTooltip">Text To Display When Hovering Over The Text Defined Just Before This.</param>
        /// <param name="OptionalButtonParent">Optional Parent Page To Put The Button To Enter This SubMenu In.</param>
        /// <returns></returns>
        public static void MakeEmptyPage(Wing wing, string name, string PageText, string PageTooltip, UIPage OptionalButtonParent = null, Action<PlaguePage> OnCreation = null)
        {
            InitTransforms();

            if (!HasPageInit)
            {
                QueuedPages.Add(new Action(() =>
                {
                    MakePage();
                }));

                return;
            }

            MakePage();

            void MakePage()
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                {
                    MelonLogger.Msg("Your Page public Name Cannot Be Empty!");
                    return;
                }

                var info = Assembly.GetExecutingAssembly().GetCustomAttribute<MelonInfoAttribute>();
                for (var i = 0; i < SubMenus.Count; i++)
                {
                    var menu = SubMenus[i];

                    if (menu.gameObject.name == "PlagueButtonAPI_SubMenu_" + info.Name.Replace(" ", "_") + " By " + info.Author.Replace(" ", "_") + "_" + name + Enum.GetName(typeof(Wing), wing))
                    {
                        OnCreation?.Invoke(menu);
                        return;
                    }
                }

                var LeftWingButtonsArea = QuickMenuObj.transform.FindOrNull("Container/Window/Wing_" + Enum.GetName(typeof(Wing), wing) + "/Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup");

                #region Menu Creation
                //Dupe Menu
                var MenuTransform = UnityEngine.Object.Instantiate(QuickMenuObj.transform.FindOrNull("Container/Window/Wing_" + Enum.GetName(typeof(Wing), wing) + "/Container/InnerContainer/Explore")).transform;

                //Name Menu Obj
                MenuTransform.name = "PlagueButtonAPI_SubMenu_" + info.Name.Replace(" ", "_") + " By " + info.Author.Replace(" ", "_") + "_" + name + Enum.GetName(typeof(Wing), wing);

                //Menu Parenting
                MenuTransform.SetParent(QuickMenuObj.transform.FindOrNull("Container/Window/Wing_" + Enum.GetName(typeof(Wing), wing) + "/Container/InnerContainer"), false);

                //Change Menu public Name
                MenuTransform.GetComponent<UIPage>().Name = MenuTransform.name;

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

                if (OptionalButtonParent != null)
                {
                    var BackButton = MenuTransform.FindOrNull("WngHeader_H1/LeftItemContainer/Button_Back").GetComponent<Button>();
                    BackButton.onClick = new Button.ButtonClickedEvent();
                    BackButton.onClick.AddListener(new Action(() =>
                    {
                        //Cache Our UIPage
                        var OurPage = QuickMenuObj.transform.FindOrNull("Container/Window/Wing_" + Enum.GetName(typeof(Wing), wing) + "/Container/InnerContainer/" + MenuTransform.name).GetComponent<UIPage>();

                        OurPage.Show(false, UIPage.TransitionType.Right);

                        //Call OnPageShown
                        OptionalButtonParent.OnPageShown();

                        //Show PArent
                        OptionalButtonParent.Show(true, UIPage.TransitionType.None);

                        OurPage.ClosePage();

                        OurPage.OnHideComplete();

                        OptionalButtonParent.OnShowComplete();

                        OptionalButtonParent.PopPage();
                    }));
                }
                #endregion

                //Testing Only
                //LeftWingButtonsArea.FindOrNull("Button_Explore").gameObject.SetActive(true);

                Button EntryButton = null;

                if (OptionalButtonParent == null)
                {
                    //Dupe Button
                    var transform = UnityEngine.Object.Instantiate(LeftWingButtonsArea.FindOrNull("Button_Explore"));

                    //Enable Said Dupe
                    transform.gameObject.SetActive(true);

                    //Namings
                    transform.transform.name = "PlagueButtonAPI_SubMenu_" + info.Name.Replace(" ", "_") + " By " +
                                               info.Author.Replace(" ", "_") + "_" + name + "_" +
                                               Enum.GetName(typeof(Wing), wing);
                    transform.name = "PlagueButtonAPI_SubMenu_" + info.Name.Replace(" ", "_") + " By " +
                                     info.Author.Replace(" ", "_") + "_" + name + "_" + Enum.GetName(typeof(Wing), wing);

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
                    StyleElem.id = transform.name;

                    transform.localScale = new Vector3(1f, 1f, 1f);
                    StyleElem.InitStyles();

                    //OnClick
                    EntryButton = transform.GetComponent<Button>();
                    EntryButton.onClick = new Button.ButtonClickedEvent();
                    EntryButton.onClick.AddListener(new Action(() =>
                    {
                        //Cache Our UIPage
                        var OurPage = QuickMenuObj.transform.FindOrNull("Container/Window/Wing_" + Enum.GetName(typeof(Wing), wing) + "/Container/InnerContainer/" + MenuTransform.name).GetComponent<UIPage>();

                        //Cache Whatever Page Is Open That Is NOT OurPage
                        var ShownPage = QuickMenuObj.transform.FindOrNull("Container/Window/Wing_" + Enum.GetName(typeof(Wing), wing) + "/Container/InnerContainer").GetComponentsInChildren<UIPage>(false).FirstOrDefault(o => o != null && o != OurPage && o.IsStackShown());

                        //In Case
                        if (ShownPage != null)
                        {
                            ShownPage.Show(false, UIPage.TransitionType.None);
                        }

                        //Call OnPageShown
                        OurPage.OnPageShown();

                        //Show Our Page
                        OurPage.Show(true, UIPage.TransitionType.Right);

                        //In Case
                        if (ShownPage != null)
                        {
                            ShownPage.PushPage(OurPage);
                            ShownPage.OnHideComplete();
                        }
                        else
                        {
                            OurPage.PushPage(OurPage);
                        }

                        OurPage.OnShowComplete();
                    }));
                }
                else
                {
                    CreateButton(OptionalButtonParent.transform, PageText, PageTooltip, () =>
                    {
                        //Cache Our UIPage
                        var OurPage = QuickMenuObj.transform.FindOrNull("Container/Window/Wing_" + Enum.GetName(typeof(Wing), wing) + "/Container/InnerContainer/" + MenuTransform.name).GetComponent<UIPage>();

                        //Cache Whatever Page Is Open That Is NOT OurPage
                        var ShownPage = QuickMenuObj.transform.FindOrNull("Container/Window/Wing_" + Enum.GetName(typeof(Wing), wing) + "/Container/InnerContainer").GetComponentsInChildren<UIPage>(false).FirstOrDefault(o => o != null && o != OurPage && o.IsStackShown());

                        //In Case
                        if (ShownPage != null)
                        {
                            ShownPage.Show(false, UIPage.TransitionType.None);
                        }

                        //Call OnPageShown
                        OurPage.OnPageShown();

                        //Show Our Page
                        OurPage.Show(true, UIPage.TransitionType.Right);

                        //In Case
                        if (ShownPage != null)
                        {
                            ShownPage.PushPage(OurPage);
                            ShownPage.OnHideComplete();
                        }
                        else
                        {
                            OurPage.PushPage(OurPage);
                        }

                        OurPage.OnShowComplete();
                    }, (obj) =>
                    {
                        obj.SubMenu_Arrow.gameObject.SetActive(true);
                        EntryButton = obj.button;
                    });
                }

                var Page = new PlaguePage
                {
                    gameObject = MenuTransform.gameObject,
                    layout = MenuTransform.FindOrNull("ScrollRect/Viewport/VerticalLayoutGroup").GetComponent<VerticalLayoutGroup>(),
                    page = MenuTransform.GetComponent<UIPage>(),
                    pageBackButton = MenuTransform.FindOrNull("WngHeader_H1/LeftItemContainer/Button_Back").GetComponent<Button>(),
                    pageEntryButton = EntryButton
                };

                SubMenus.Add(Page);

                OnCreation?.Invoke(Page);
            }
        }

        #endregion

        #endregion

        #region Enums

        public enum Wing
        {
            Left,
            Right
        }

        #endregion public Enumerations

        #region Public Things - Not For The End User

        //Any Created Sub Menus By The User Are Stored Here
        public static List<PlaguePage> SubMenus = new List<PlaguePage>();

        #endregion

        #region Helpers

        public static int RangeConv(float input, float MinPossibleInput, float MaxPossibleInput, float MinConv, float MaxConv)
        {
            return (int)((((input - MinPossibleInput) * (MaxConv - MinConv)) / (MaxPossibleInput - MinPossibleInput)) + MinConv);
        }

        public static GameObject FindOrNull(string path)
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
        public static void RepairShit(Transform Template, Transform Dupe, bool Debug = false, bool Initial = true)
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
    public static class ButtonAPIExtensions
    {
        public static Transform FindOrNull(this Transform transform, string path)
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

        public static void SetActive(this ButtonAPI.PlagueBase plagueBase, bool state)
        {
            plagueBase.gameObject?.SetActive(state);
        }

        public static void SetInteractable(this ButtonAPI.PlagueButton plagueButton, bool interactable)
        {
            plagueButton.button.interactable = interactable;
            plagueButton.CancelGif.gameObject.SetActive(!interactable);
        }

        public static void SetToggleState(this ButtonAPI.PlagueToggle plagueToggle, bool state)
        {
            plagueToggle.Icon_Secondary.sprite = state ? ButtonAPI.Checked_Checkbox : ButtonAPI.Unchecked_Checkbox;
        }
    }
    #endregion

    #region Custom Components

    [RegisterTypeInIl2Cpp]
    public class ObjectHandler : MonoBehaviour
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

    #endregion
    #endregion
}
