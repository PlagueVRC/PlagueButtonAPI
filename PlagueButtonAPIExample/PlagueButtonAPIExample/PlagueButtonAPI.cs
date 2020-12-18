using MelonLoader;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlagueButtonAPI
{
    internal class ButtonAPI
    {
#pragma warning disable 414

        #region Creditation And Disclaimer

        private static readonly string Creditation =
        "Plague Button API" +
        "http://Krewella.co.uk/Discord" +
        "Copyright Reserved" +
        "Use-Only Licensed" +
        "https://github.com/OFWModz/PlagueButtonAPI" +
        "Removal Or Modification Of This String Breaches The License." +
        "This String Is To Be Preserved AS IS.";

        #endregion Creditation And Disclaimer

#pragma warning restore 414

        #region Button Class Type

        internal class PlagueButton
        {
            #region Constructor
            public PlagueButton(GameObject gameObject, Button button, Text text, UiTooltip tooltip, Image image, RectTransform rect, Color OffColour, Color OnColour, Color? BorderColour, bool ToggleState, float xPos, float yPos)
            {
                if (gameObject != null)
                {
                    this.gameObject = gameObject;
                }

                if (button != null)
                {
                    this.button = button;
                }

                if (text != null)
                {
                    this.text = text;
                }

                if (tooltip != null)
                {
                    this.tooltip = tooltip;
                }

                if (image != null)
                {
                    this.image = image;
                }

                if (rect != null)
                {
                    this.rect = rect;
                }

                this.OffColour = OffColour;
                this.OnColour = OnColour;
                BorderColor = BorderColour; // Only One Can Be Set, Thus No this. Is Needed.
                this.ToggleState = ToggleState;
                this.xPos = xPos;
                this.yPos = yPos;
            }
            #endregion

            #region Reference Objects
            internal GameObject gameObject;
            internal Button button;
            internal Text text;
            internal UiTooltip tooltip;
            internal Image image;
            internal RectTransform rect;
            #endregion

            #region Read Only Objects

            internal readonly Color OffColour;

            internal readonly Color OnColour;

            internal readonly Color? BorderColor;

            internal readonly bool ToggleState;

            internal readonly float xPos;
            internal readonly float yPos;

            #endregion
        }

        #endregion

        #region internal Variables

        internal static Transform ShortcutMenuTransform =>
            GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu").transform;

        internal static QuickMenu QuickMenuObj =>
            ShortcutMenuTransform.parent.GetComponent<QuickMenu>();

        internal static Transform UserInteractMenuTransform =>
            GameObject.Find("/UserInterface/QuickMenu/UserInteractMenu").transform;

        internal static List<PlagueButton> ButtonsFromThisMod = new List<PlagueButton>();

        #endregion internal Variables

        #region Main Functions

        #region Button Creation

        /// <summary>
        /// Creates A Input Field And Returns The Object Of The Input Field Made. | Created By Plague | Discord Server: http://Krewella.co.uk/Discord
        /// </summary>
        ///     <para>
        ///     As You Type Arguments Within This Method You Will See What Each Argument Does Here.
        ///     </para>
        /// <param name="PlaceHolderText">The Text To Show Before Anything Is Typed By The User.</param>
        /// <param name="Y">The Vertical Position Of The InputField</param>
        /// <param name="Parent">The Parent To Place This InputField In, You Can Use ButtonAPI.ShortcutMenuTransform As A Example Transform.</param>
        /// <param name="TextChanged">The Delegate To Call Upon Text Being Changed, This Is Used As: delegate(string text) { }</param>
        /// <returns>UnityEngine.UI.InputField</returns>
        internal static InputField CreateInputField(string PlaceHolderText, VerticalPosition Y, Transform Parent, Action<string> TextChanged)
        {
            //FreezeControls
            //VRCInputManager.Method_internal_Static_Void_Boolean_0(true);

            //Prevent Weird Bugs Due To A Invalid Parent - Set It To The Main QuickMenu
            if (Parent == null)
            {
                Parent = QuickMenu.prop_QuickMenu_0.transform;
            }

            //Get The Transform Of InputField Of The Input Popup - Which We Are Going To Use As Our Template
            InputField inputfield = UnityEngine.Object.Instantiate(VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.inputPopup.GetComponentInChildren<InputField>());

            inputfield.placeholder.GetComponent<Text>().text = PlaceHolderText;

            inputfield.placeholder.GetComponent<Text>().alignment = TextAnchor.UpperLeft;

            inputfield.textComponent.alignment = TextAnchor.UpperLeft;

            ColorBlock tempcolorblock = inputfield.colors;

            tempcolorblock.normalColor = Color.magenta;
            tempcolorblock.highlightedColor = Color.magenta;
            tempcolorblock.pressedColor = Color.magenta;

            inputfield.colors = tempcolorblock;

            //InputField Position Calculation
            float num =
                (GameObject.Find("/UserInterface/QuickMenu").GetComponent<QuickMenu>().transform.Find("UserInteractMenu/ForceLogoutButton").localPosition.x -
                 GameObject.Find("/UserInterface/QuickMenu").GetComponent<QuickMenu>().transform.Find("UserInteractMenu/BanButton").localPosition.x) / 3.9f;

            //Define Position To Place This InputField In The Parent, Appended To By The TopOrBottom Switch
            inputfield.transform.localPosition = new Vector3(-1185f, inputfield.transform.localPosition.y + num * ((float)Y - 1.57f), inputfield.transform.localPosition.z);

            //Alignment Due To Scaling
            inputfield.GetComponent<RectTransform>().sizeDelta = new Vector2(1675f, 410f);

            inputfield.transform.localPosition -= new Vector3(-1185f, 130f, 0);

            //Define Where To Put This InputField
            inputfield.transform.SetParent(Parent, worldPositionStays: false);

            inputfield.onValueChanged = new InputField.OnChangeEvent();
            inputfield.onValueChanged.AddListener(TextChanged);

            return inputfield;
        }

        /// <summary>
        /// Creates A Button With A Lot Of Customization And Returns The GameObject Of The Button Made. | Created By Plague | Discord Server: http://Krewella.co.uk/Discord
        ///     <para>
        ///     As You Type Arguments Within This Method You Will See What Each Argument Does Here.
        ///     </para>
        ///
        ///     <example>
        ///     Here Is An Example Of How To Use This:
        ///         <code>
        ///         ButtonAPI.CreateButton(ButtonAPI.ButtonType.Toggle, "Toggle Pickups", "Toggles All Pickups In The Current Instance.", ButtonAPI.HorizontalPosition.FirstButtonPos,      ButtonAPI.VerticalPosition.TopButton, null, delegate (bool a)
        ///            {
        ///                //Do Something Here
        ///            }, Color.white, Color.magenta, null, false, false, true);
        ///         </code>
        ///     </example>
        /// </summary>
        /// <param name="ButtonType">
        /// The Type Of Button You Wish To Create.
        /// </param>
        /// <param name="Text">
        /// The Main Text In The Button
        /// </param>
        /// <param name="ToolTip">
        /// The Text That Appears At The Top Of The Menu When You Hover Over The Button.
        /// </param>
        /// <param name="X">
        /// The Horizontal Position Of The Button.
        /// </param>
        /// <param name="Y">
        /// The Vertical Position Of The Button.
        /// </param>
        /// <param name="Parent">
        /// The Transform Of The GameObject You Wish To Put Your Button In (You Can Set This As Just "null" For The Main ShortcutMenu).
        /// </param>
        /// <param name="ButtonListener">
        /// What You Want The Button To Do When You Click It - Must Be delegate(bool nameofboolhere) {  }.
        /// </param>
        /// <param name="ToggledOffTextColour">
        /// The Colour You Want The Main Text Of The Button You Defined Earlier To Change Into If This Button Is Toggled Off.
        /// </param>
        /// <param name="ToggledOnTextColour">
        /// The Colour You Want The Main Text Of The Button You Defined Earlier To Change Into If This Button Is Toggled On.
        /// </param>
        /// <param name="BorderColour">
        /// The Colour You Want The Border Of The Button To Be (You Can Set This As Just "null" For The Default Colour That The ShortcutMenu Currently Is!).
        /// </param>
        /// <param name="FullSizeButton">
        /// If You Want This Button To Be A Full Size Normal Button, Or Half Sized (False) - Default Is Half Sized.
        /// </param>
        /// <param name="BottomHalf">
        /// If You Want This Button To Be On The Bottom Half Of The VericalPosition You Chose Or The Top - Default Is Bottom Half.
        /// </param>
        /// <param name="HalfHorizontally">
        /// If You Want This Button To Have It's Size Cut In Half Horizontally.
        /// </param>
        /// <param name="CurrentToggleState">
        /// The Toggle State You Want The Button To Be On Creation.
        /// </param>
        /// <param name="SpriteForButton">
        /// The Image Sprite You Want To Apply To The Button.
        /// </param>
        /// <param name="ChangeColourOnClick">
        /// Only Set This To False If You Are Setting The Button's Text Colour In The ButtonListener - Or The Toggling Will Break!
        /// </param>
        internal static PlagueButton CreateButton(ButtonType ButtonType, string Text, string ToolTip, HorizontalPosition X,
            VerticalPosition Y, Transform Parent, Action<bool> ButtonListener, Color ToggledOffTextColour,
            Color ToggledOnTextColour, Color? BorderColour, bool FullSizeButton = false, bool BottomHalf = true,
            bool HalfHorizontally = false, bool CurrentToggleState = false, Sprite SpriteForButton = null,
            bool ChangeColourOnClick = true)
        {
            //Prevent Weird Bugs Due To A Invalid Parent - Set It To The Main QuickMenu
            if (Parent == null)
            {
                Parent = ShortcutMenuTransform;
            }

            //Get The Transform Of The Settings Button - Which We Are Going To Use As Our Template
            Transform transform = UnityEngine.Object
                .Instantiate(GameObject.Find("/UserInterface/QuickMenu").GetComponent<QuickMenu>().transform.Find("ShortcutMenu/SettingsButton").gameObject)
                .transform;

            PlagueButton plagueButton = new PlagueButton(transform.gameObject,
                transform.GetComponent<Button>(),
                transform.GetComponentInChildren<Text>(),
                transform.GetComponentInChildren<UiTooltip>(),
                transform.GetComponentInChildren<Image>(),
                transform.GetComponent<RectTransform>(),
                ToggledOffTextColour,
                ToggledOnTextColour,
                BorderColour,
                CurrentToggleState, (float)X, (float)Y);

            //Button Position Calculation
            float num =
                (GameObject.Find("/UserInterface/QuickMenu").GetComponent<QuickMenu>().transform.Find("UserInteractMenu/ForceLogoutButton").localPosition.x -
                GameObject.Find("/UserInterface/QuickMenu").GetComponent<QuickMenu>().transform.Find("UserInteractMenu/BanButton").localPosition.x) / 3.9f;

            //Change Internal Names & Sanitize Them
            transform.name = "PlagueButton_" + Text.Replace(" ", "_".Replace(",", "_").Replace(":", "_"));
            transform.transform.name = "PlagueButton_" + Text.Replace(" ", "_".Replace(",", "_").Replace(":", "_"));

            //Define Position To Place This Button In The Parent, Appended To Later
            if (BottomHalf || FullSizeButton)
            {
                if (Parent == UserInteractMenuTransform)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x + num * (float)X,
                        transform.localPosition.y + num * ((float)Y - 2.95f), transform.localPosition.z);
                }
                else
                {
                    transform.localPosition = new Vector3(transform.localPosition.x + num * (float)X,
                        transform.localPosition.y + num * ((float)Y - 1.95f), transform.localPosition.z);
                }
            }
            else
            {
                if (Parent == UserInteractMenuTransform)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x + num * (float)X,
                    transform.localPosition.y + num * ((float)Y - 2.45f), transform.localPosition.z);
                }
                else
                {
                    transform.localPosition = new Vector3(transform.localPosition.x + num * (float)X,
                        transform.localPosition.y + num * ((float)Y - 1.45f), transform.localPosition.z);
                }
            }

            //Define Where To Put This Button
            transform.SetParent(Parent, worldPositionStays: false);

            //Set Text, Tooltip & Colours
            transform.GetComponentInChildren<Text>().supportRichText = true;
            transform.GetComponentInChildren<Text>().text = Text;
            transform.GetComponentInChildren<UiTooltip>().text = ToolTip;
            transform.GetComponentInChildren<UiTooltip>().alternateText = ToolTip;

            if (CurrentToggleState && ButtonType != ButtonAPI.ButtonType.Default)
            {
                transform.GetComponentInChildren<Text>().color = ToggledOnTextColour;
            }
            else
            {
                transform.GetComponentInChildren<Text>().color = ToggledOffTextColour;
            }

            //Set The Button's Border Colour
            if (BorderColour != null)
            {
                transform.GetComponentInChildren<Image>().color = (Color)BorderColour;
            }

            //Size Scaling & Repositioning
            if (!FullSizeButton)
            {
                transform.localPosition +=
                    new Vector3(0f, transform.GetComponent<RectTransform>().sizeDelta.y / 5f, 0f);
                transform.localPosition -=
                    new Vector3(0f, transform.GetComponent<RectTransform>().sizeDelta.y / 2f, 0f);
                transform.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    transform.GetComponent<RectTransform>().sizeDelta.x,
                    transform.GetComponent<RectTransform>().sizeDelta.y / 2f);
            }
            else
            {
                transform.localPosition -= new Vector3(0f, 20f, 0f);
            }

            if (HalfHorizontally)
            {
                transform.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    transform.GetComponent<RectTransform>().sizeDelta.x / 2f,
                    transform.GetComponent<RectTransform>().sizeDelta.y);
            }

            if (SpriteForButton != null)
            {
                transform.GetComponentInChildren<Image>().sprite = SpriteForButton;
            }

            //Remove Any Previous Events
            transform.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();

            //Listener Redirection - To Get Around AddListener Not Passing A State Bool Due To Being A onClick Event
            transform.GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                if (ButtonType == ButtonType.Toggle)
                {
                    ButtonListener?.Invoke(transform.GetComponentInChildren<Text>().color != plagueButton.OnColour);
                }
                else
                {
                    ButtonListener?.Invoke(true);
                }
            }));

            if (ButtonType == ButtonType.Toggle)
            {
                //Set The Text Colour To The Toggle State, ToggledOnTextColour Being Toggled On
                transform.GetComponent<Button>().onClick.AddListener(new Action(() =>
                {
                    if (transform.GetComponentInChildren<Text>().color == plagueButton.OnColour)
                    {
                        transform.GetComponentInChildren<Text>().color = plagueButton.OffColour;
                    }
                    else
                    {
                        transform.GetComponentInChildren<Text>().color = plagueButton.OnColour;
                    }
                }));
            }

            //Update
            plagueButton.gameObject = transform.gameObject;
            plagueButton.button = transform.GetComponent<Button>();
            plagueButton.text = transform.GetComponentInChildren<Text>();
            plagueButton.tooltip = transform.GetComponentInChildren<UiTooltip>();
            plagueButton.image = transform.GetComponentInChildren<Image>();
            plagueButton.rect = transform.GetComponent<RectTransform>();

            ButtonsFromThisMod.Add(plagueButton);

            //Return The GameObject For Handling It Elsewhere
            return plagueButton;
        }

        /// <summary>
        /// Creates A Button With A Lot Of Customization And Returns The GameObject Of The Button Made. | Created By Plague | Discord Server: http://Krewella.co.uk/Discord
        ///     <para>
        ///     As You Type Arguments Within This Method You Will See What Each Argument Does Here.
        ///     </para>
        ///
        ///     <example>
        ///     Here Is An Example Of How To Use This:
        ///         <code>
        ///         ButtonAPI.CreateButton(ButtonAPI.ButtonType.Toggle, "Toggle Pickups", "Toggles All Pickups In The Current Instance.", ButtonAPI.HorizontalPosition.FirstButtonPos,      ButtonAPI.VerticalPosition.TopButton, null, delegate (bool a)
        ///            {
        ///                //Do Something Here
        ///            }, Color.white, Color.magenta, null, false, false, true);
        ///         </code>
        ///     </example>
        /// </summary>
        /// <param name="ButtonType">
        /// The Type Of Button You Wish To Create.
        /// </param>
        /// <param name="Text">
        /// The Main Text In The Button
        /// </param>
        /// <param name="ToolTip">
        /// The Text That Appears At The Top Of The Menu When You Hover Over The Button.
        /// </param>
        /// <param name="X">
        /// The Horizontal Position Of The Button.
        /// </param>
        /// <param name="Y">
        /// The Vertical Position Of The Button.
        /// </param>
        /// <param name="Parent">
        /// The Transform Of The GameObject You Wish To Put Your Button In (You Can Set This As Just "null" For The Main ShortcutMenu).
        /// </param>
        /// <param name="ButtonListener">
        /// What You Want The Button To Do When You Click It - Must Be delegate(bool nameofboolhere) {  }.
        /// </param>
        /// <param name="ToggledOffTextColour">
        /// The Colour You Want The Main Text Of The Button You Defined Earlier To Change Into If This Button Is Toggled Off.
        /// </param>
        /// <param name="ToggledOnTextColour">
        /// The Colour You Want The Main Text Of The Button You Defined Earlier To Change Into If This Button Is Toggled On.
        /// </param>
        /// <param name="BorderColour">
        /// The Colour You Want The Border Of The Button To Be (You Can Set This As Just "null" For The Default Colour That The ShortcutMenu Currently Is!).
        /// </param>
        /// <param name="FullSizeButton">
        /// If You Want This Button To Be A Full Size Normal Button, Or Half Sized (False) - Default Is Half Sized.
        /// </param>
        /// <param name="BottomHalf">
        /// If You Want This Button To Be On The Bottom Half Of The VericalPosition You Chose Or The Top - Default Is Bottom Half.
        /// </param>
        /// <param name="HalfHorizontally">
        /// If You Want This Button To Have It's Size Cut In Half Horizontally.
        /// </param>
        /// <param name="CurrentToggleState">
        /// The Toggle State You Want The Button To Be On Creation.
        /// </param>
        /// <param name="SpriteForButton">
        /// The Image Sprite You Want To Apply To The Button.
        /// </param>
        /// <param name="ChangeColourOnClick">
        /// Only Set This To False If You Are Setting The Button's Text Colour In The ButtonListener - Or The Toggling Will Break!
        /// </param>
        internal static PlagueButton CreateButton(ButtonType ButtonType, string Text, string ToolTip, float X, float Y,
            Transform Parent, Action<bool> ButtonListener, Color ToggledOffTextColour, Color ToggledOnTextColour,
            Color? BorderColour, bool FullSizeButton = false, bool BottomHalf = true, bool HalfHorizontally = false,
            bool CurrentToggleState = false, Sprite SpriteForButton = null, bool ChangeColourOnClick = true)
        {
            //Prevent Weird Bugs Due To A Invalid Parent - Set It To The Main QuickMenu
            if (Parent == null)
            {
                Parent = ShortcutMenuTransform;
            }

            //Get The Transform Of The Settings Button - Which We Are Going To Use As Our Template
            Transform transform = UnityEngine.Object
                .Instantiate(GameObject.Find("/UserInterface/QuickMenu").GetComponent<QuickMenu>().transform.Find("ShortcutMenu/SettingsButton").gameObject)
                .transform;

            PlagueButton plagueButton = new PlagueButton(transform.gameObject,
                transform.GetComponent<Button>(),
                transform.GetComponentInChildren<Text>(),
                transform.GetComponentInChildren<UiTooltip>(),
                transform.GetComponentInChildren<Image>(),
                transform.GetComponent<RectTransform>(),
                ToggledOffTextColour,
                ToggledOnTextColour,
                BorderColour,
                CurrentToggleState, X, Y);

            //Button Position Calculation
            float num =
                (GameObject.Find("/UserInterface/QuickMenu").GetComponent<QuickMenu>().transform.Find("UserInteractMenu/ForceLogoutButton").localPosition.x -
                GameObject.Find("/UserInterface/QuickMenu").GetComponent<QuickMenu>().transform.Find("UserInteractMenu/BanButton").localPosition.x) / 3.9f;

            //Change Internal Names & Sanitize Them
            transform.name = "PlagueButton_" + Text.Replace(" ", "_".Replace(",", "_").Replace(":", "_"));
            transform.transform.name = "PlagueButton_" + Text.Replace(" ", "_".Replace(",", "_").Replace(":", "_"));

            //Define Position To Place This Button In The Parent, Appended To Later
            if (BottomHalf || FullSizeButton)
            {
                if (Parent == UserInteractMenuTransform)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x + num * X,
                        transform.localPosition.y + num * (Y - 2.95f), transform.localPosition.z);
                }
                else
                {
                    transform.localPosition = new Vector3(transform.localPosition.x + num * X,
                        transform.localPosition.y + num * (Y - 1.95f), transform.localPosition.z);
                }
            }
            else
            {
                if (Parent == UserInteractMenuTransform)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x + num * X,
                        transform.localPosition.y + num * (Y - 2.45f), transform.localPosition.z);
                }
                else
                {
                    transform.localPosition = new Vector3(transform.localPosition.x + num * X,
                        transform.localPosition.y + num * (Y - 1.45f), transform.localPosition.z);
                }
            }

            //Define Where To Put This Button
            transform.SetParent(Parent, worldPositionStays: false);

            //Set Text, Tooltip & Colours
            transform.GetComponentInChildren<Text>().supportRichText = true;
            transform.GetComponentInChildren<Text>().text = Text;
            transform.GetComponentInChildren<UiTooltip>().text = ToolTip;
            transform.GetComponentInChildren<UiTooltip>().alternateText = ToolTip;

            if (CurrentToggleState && ButtonType != ButtonAPI.ButtonType.Default)
            {
                transform.GetComponentInChildren<Text>().color = ToggledOnTextColour;
            }
            else
            {
                transform.GetComponentInChildren<Text>().color = ToggledOffTextColour;
            }

            //Set The Button's Border Colour
            if (BorderColour != null)
            {
                transform.GetComponentInChildren<Image>().color = (Color)BorderColour;
            }

            //Size Scaling & Repositioning
            if (!FullSizeButton)
            {
                transform.localPosition +=
                    new Vector3(0f, transform.GetComponent<RectTransform>().sizeDelta.y / 5f, 0f);
                transform.localPosition -=
                    new Vector3(0f, transform.GetComponent<RectTransform>().sizeDelta.y / 2f, 0f);
                transform.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    transform.GetComponent<RectTransform>().sizeDelta.x,
                    transform.GetComponent<RectTransform>().sizeDelta.y / 2f);
            }
            else
            {
                transform.localPosition -= new Vector3(0f, 20f, 0f);
            }

            if (HalfHorizontally)
            {
                transform.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    transform.GetComponent<RectTransform>().sizeDelta.x / 2f,
                    transform.GetComponent<RectTransform>().sizeDelta.y);
            }

            if (SpriteForButton != null)
            {
                transform.GetComponentInChildren<Image>().sprite = SpriteForButton;
            }

            //Remove Any Previous Events
            transform.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();

            //Listener Redirection - To Get Around AddListener Not Passing A State Bool Due To Being A onClick Event
            transform.GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                if (ButtonType == ButtonType.Toggle)
                {
                    ButtonListener?.Invoke(transform.GetComponentInChildren<Text>().color != ToggledOnTextColour);
                }
                else
                {
                    ButtonListener?.Invoke(true);
                }
            }));

            if (ButtonType == ButtonType.Toggle)
            {
                //Set The Text Colour To The Toggle State, ToggledOnTextColour Being Toggled On
                transform.GetComponent<Button>().onClick.AddListener(new Action(() =>
                {
                    if (transform.GetComponentInChildren<Text>().color == ToggledOnTextColour)
                    {
                        transform.GetComponentInChildren<Text>().color = ToggledOffTextColour;
                    }
                    else
                    {
                        transform.GetComponentInChildren<Text>().color = ToggledOnTextColour;
                    }
                }));
            }

            //Update
            plagueButton.gameObject = transform.gameObject;
            plagueButton.button = transform.GetComponent<Button>();
            plagueButton.text = transform.GetComponentInChildren<Text>();
            plagueButton.tooltip = transform.GetComponentInChildren<UiTooltip>();
            plagueButton.image = transform.GetComponentInChildren<Image>();
            plagueButton.rect = transform.GetComponent<RectTransform>();

            ButtonsFromThisMod.Add(plagueButton);

            //Return The GameObject For Handling It Elsewhere
            return plagueButton;
        }

        #endregion Button Creation

        #region Sub Menu Creation And Handling

        /// <summary>
        /// Creates A Empty Page For Adding Buttons To, If The Page Already Exists, This Will Return It. | Created By Plague | Discord Server: http://Krewella.co.uk/Discord
        /// </summary>
        /// <param name="name">
        /// The Name You Want To Give The Page/Find Internally.
        /// </param>
        internal static GameObject MakeEmptyPage(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                MelonLogger.Log("Your Empty Page Name Cannot Be Empty!");
                return null;
            }

            //If This Page Already Exists, Return It
            for (int i = 0; i < SubMenus.Count; i++)
            {
                GameObject menu = SubMenus[i];
                if (menu.name == "PlagueButtonAPI_" + name)
                {
                    return menu;
                }
            }

            //Clone The ShortcutMenu
            Transform transform = UnityEngine.Object.Instantiate(ShortcutMenuTransform.gameObject).transform;

            //Change Internal Names
            transform.transform.name = "PlagueButtonAPI_" + name;
            transform.name = "PlagueButtonAPI_" + name;

            //Remove All Buttons
            for (int i = 0; i < transform.childCount; i++)
            {
                UnityEngine.Object.Destroy(transform.GetChild(i).gameObject);
            }

            //Make This Page We Cloned A Child Of The QuickMenu
            transform.SetParent(GameObject.Find("/UserInterface/QuickMenu").GetComponent<QuickMenu>().transform, worldPositionStays: false);

            //Make This Page We Cloned Inactive By Default
            transform.gameObject.SetActive(value: false);

            //Add It To The Handler
            SubMenus.Add(transform.gameObject);

            //Return The GameObject For Handling It Elsewhere
            return transform.gameObject;
        }

        /// <summary>
        /// Finds A SubMenu Inside Said Transform Created By My Button API. This Method Will Not Create One Under This Name If Not Found. | Created By Plague | Discord Server: http://Krewella.co.uk/Discord
        /// </summary>
        /// <param name="name">
        /// The Name OF The SubMenu To Find.
        /// </param>
        /// <param name="WhereTheSubMenuIsInside">
        /// Where You Placed The SubMenu, Such As The ShortcutMenu Or UserInteractMenu.
        /// </param>
        internal static GameObject FindSubMenu(string name, Transform WhereTheSubMenuIsInside)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                MelonLogger.Log("Your SubMenu Name Cannot Be Empty!");
                return null;
            }

            //Find The SubMenu And Return It
            return WhereTheSubMenuIsInside.Find("PlagueButtonAPI_" + name).gameObject;
        }

        /// <summary>
        /// Enters The Submenu. | Created By Plague | Discord Server: http://Krewella.co.uk/Discord
        /// </summary>
        /// <param name="name">
        /// The GameObject Of The SubMenu You Want To Enter.
        /// </param>
        internal static void EnterSubMenu(GameObject menu)
        {
            if (ShortcutMenuTransform.gameObject.active)
            {
                ShortcutMenuTransform.gameObject.SetActive(false);
            }

            if (UserInteractMenuTransform.gameObject.active)
            {
                UserInteractMenuTransform.gameObject.SetActive(false);
            }

            for (int i = 0; i < SubMenus.Count; i++)
            {
                GameObject Menu = SubMenus[i];
                Menu.SetActive(false);
            }

            if (menu != null)
            {
                menu.SetActive(true);
            }
        }

        /// <summary>
        /// Closes All SubMenus Created
        /// </summary>
        internal static void CloseAllSubMenus()
        {
            ShortcutMenuTransform.gameObject.SetActive(false);
            UserInteractMenuTransform.gameObject.SetActive(false);

            for (int i = 0; i < SubMenus.Count; i++)
            {
                GameObject Menu = SubMenus[i];
                Menu.SetActive(false);
            }
        }

        #endregion Sub Menu Creation And Handling

        #endregion Main Functions

        #region Internal Enumerations

        /// <summary>
        /// The Horizontal Position Of The Button You Are Creating.
        /// </summary>
        internal enum HorizontalPosition
        {
            TwoLeftOfMenu = -5,

            LeftOfMenu = -4,

            FirstButtonPos = -3,

            SecondButtonPos = -2,

            ThirdButtonPos = -1,

            FourthButtonPos = 0,

            RightOfMenu = 1
        }

        /// <summary>
        /// The Vertical Position Of The Button You Are Creating.
        /// </summary>
        internal enum VerticalPosition
        {
            TwoAboveMenu = 6,

            AboveMenu = 5,

            AboveTopButton = 4,

            TopButton = 3,

            SecondButton = 2,

            BottomButton = 1,

            BelowBottomButton = 0
        }

        /// <summary>
        /// The Type Of Button You Are Creating.
        /// </summary>
        internal enum ButtonType
        {
            Default,
            Toggle
        }

        #endregion Internal Enumerations

        #region Internal Functions - Not For The End User

        //Any Created Sub Menus By The User Are Stored Here
        internal static List<GameObject> SubMenus = new List<GameObject>();

        private static float HandlerRoutineDelay = 0f;

        internal static void SubMenuHandler()
        {
            if (SubMenus != null && SubMenus.Count > 0 && QuickMenuObj != null && Time.time > HandlerRoutineDelay)
            {
                HandlerRoutineDelay = Time.time + 0.2f;

                //If User Has Loaded A World
                if (RoomManager.prop_Boolean_3)
                {
                    for (int i = 0; i < SubMenus.Count; i++)
                    {
                        GameObject Menu = SubMenus[i];

                        if (Menu.active) // Is In This SubMenu
                        {
                            //If QuickMenu Was Closed
                            if (!QuickMenuObj.prop_Boolean_0)
                            {
                                //Hide SubMenu
                                Menu.SetActive(false);
                            }

                            //If QuickMenu Is Open Normally When In A SubMenu (Aka When It Shouldn't Be) - This Fixes The Menu Breaking When A Player Joins
                            else if (ShortcutMenuTransform.gameObject.active || UserInteractMenuTransform.gameObject.active)
                            {
                                ShortcutMenuTransform.gameObject.SetActive(false);
                                UserInteractMenuTransform.gameObject.SetActive(false);
                            }

                            break;
                        }
                    }
                }
            }
        }

        #endregion Internal Functions - Not For The End User
    }

    #region Extension Methods
    internal static class ButtonAPIExtensions
    {
        /// <summary>
        /// Sets The Buttons Toggle State. | Created By Plague | Discord Server: http://Krewella.co.uk/Discord
        /// </summary>
        /// <param name="button">
        /// The GameObject Of The Button You Wish To Set The Toggle State Of.
        /// </param>
        /// <param name="StateToSetTo">
        /// The Toggle State You Wish To Set This Button To.
        /// </param>
        internal static void SetToggleState(this ButtonAPI.PlagueButton button, bool StateToSetTo)
        {
            if (button.text != null && button.OnColour != null && button.OffColour != null)
            {
                button.text.color = button.text.color == button.OnColour ? button.OffColour : button.OnColour;
            }
        }

        /// <summary>
        /// Sets The Buttons Text. | Created By Plague | Discord Server: http://Krewella.co.uk/Discord
        /// </summary>
        /// <param name="button">
        /// The GameObject Of The Button You Wish To Set The Toggle State Of.
        /// </param>
        /// <param name="text">
        /// The Text You Want To Place On The Button.
        /// </param>
        internal static void SetText(this ButtonAPI.PlagueButton button, string text)
        {
            if (button.text != null)
            {
                button.text.text = text;
            }
        }

        /// <summary>
        /// Sets The Buttons Text. | Created By Plague | Discord Server: http://Krewella.co.uk/Discord
        /// </summary>
        /// <param name="button">
        /// The GameObject Of The Button You Wish To Set The Toggle State Of.
        /// </param>
        /// <param name="text">
        /// The Text You Want To Place On The Button.
        /// </param>
        internal static void SetTooltip(this ButtonAPI.PlagueButton button, string text)
        {
            if (button.tooltip != null)
            {
                button.tooltip.text = text;
                button.tooltip.alternateText = text;
            }
        }

        /// <summary>
        /// Sets A Button To Be Interactable Or Not. | Created By Plague | Discord Server: http://Krewella.co.uk/Discord
        /// </summary>
        /// <param name="button">
        /// The GameObject Of The Button To Set The Interactivity Of.
        /// </param>
        /// <param name="state">
        /// If You Want The Button To Be Interactable.
        /// </param>
        internal static void SetInteractivity(this ButtonAPI.PlagueButton button, bool state)
        {
            if (button.button != null)
            {
                button.button.interactable = state;
            }
        }

        /// <summary>
        /// Returns The Sprite Of A Given Button's GameObject. | Created By Plague | Discord Server: http://Krewella.co.uk/Discord
        /// </summary>
        /// <param name="button">
        /// The GameObject Of The Button To Pull The Sprite From.
        /// </param>
        internal static Sprite GetSprite(this ButtonAPI.PlagueButton button)
        {
            if (button.image != null)
            {
                return button.image.sprite;
            }

            return null;
        }
    }
    #endregion
}
