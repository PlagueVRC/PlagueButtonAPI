using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace PlagueButtonAPI
{
    public class ButtonAPI
    {
        #pragma warning disable 414
            #region Creditation And Disclaimer
                private static string Creditation =
                "Plague Button API" +
                "http://discord.me/Poppy" +
                "Copyright Reserved" +
                "Use-Only Licensed" +
                "https://github.com/OFWModz/PlagueButtonAPI" +
                "Removal Or Modification Of This String Breaches The License." +
                "This String Is To Be Preserved AS IS.";
            #endregion Creditation And Disclaimer
        #pragma warning restore 414

        #region Public Variables
            public static Transform ShortcutMenuTransform =>
                QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu").gameObject.transform;

            public static Transform UserInteractMenuTransform =>
                QuickMenu.prop_QuickMenu_0.transform.Find("UserInteractMenu").gameObject.transform;
        #endregion Public Variables

        #region Main Functions
            #region Button Creation
                /// <summary>
                /// Creates A Button With A Lot Of Customization And Returns The GameObject Of The Button Made. | Created By Plague | Discord Server: http://discord.me/Poppy
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
                public static GameObject CreateButton(ButtonType ButtonType, string Text, string ToolTip, HorizontalPosition X,
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
                        .Instantiate(QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu/SettingsButton").gameObject)
                        .transform;

                     //Button Position Calculation
                     float num =
                         QuickMenu.prop_QuickMenu_0.transform.Find("UserInteractMenu/ForceLogoutButton").localPosition.x -
                         QuickMenu.prop_QuickMenu_0.transform.Find("UserInteractMenu/BanButton").localPosition.x;

                     //Change Internal Names & Sanitize Them
                     transform.name = "PlagueButton_" + Text.Replace(" ", "_".Replace(",", "_").Replace(":", "_"));
                     transform.transform.name = "PlagueButton_" + Text.Replace(" ", "_".Replace(",", "_").Replace(":", "_"));

                     //Define Position To Place This Button In The Parent, Appended To Later
                     if (BottomHalf || FullSizeButton)
                     {
                         transform.localPosition = new Vector3(transform.localPosition.x + num * (float) X,
                             transform.localPosition.y + num * ((float) Y - 1.95f), transform.localPosition.z);
                     }
                     else
                     {
                         transform.localPosition = new Vector3(transform.localPosition.x + num * (float) X,
                             transform.localPosition.y + num * ((float) Y - 1.45f), transform.localPosition.z);
                     }

                    //Define Where To Put This Button
                    transform.SetParent(Parent, worldPositionStays: false);

                    //Set Text, Tooltip & Colours
                    transform.GetComponentInChildren<Text>().supportRichText = true;
                    transform.GetComponentInChildren<Text>().text = Text;
                    transform.GetComponentInChildren<UiTooltip>().text = ToolTip;

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
                        transform.GetComponentInChildren<Image>().color = (Color) BorderColour;
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

                    //Return The GameObject For Handling It Elsewhere
                    return transform.gameObject;
                }

                /// <summary>
                /// Creates A Button With A Lot Of Customization And Returns The GameObject Of The Button Made. | Created By Plague | Discord Server: http://discord.me/Poppy
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
                public static GameObject CreateButton(ButtonType ButtonType, string Text, string ToolTip, float X, float Y,
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
                        .Instantiate(QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu/SettingsButton").gameObject)
                        .transform;

                    //Button Position Calculation
                    float num =
                        QuickMenu.prop_QuickMenu_0.transform.Find("UserInteractMenu/ForceLogoutButton").localPosition.x -
                        QuickMenu.prop_QuickMenu_0.transform.Find("UserInteractMenu/BanButton").localPosition.x;

                    //Change Internal Names & Sanitize Them
                    transform.name = "PlagueButton_" + Text.Replace(" ", "_".Replace(",", "_").Replace(":", "_"));
                    transform.transform.name = "PlagueButton_" + Text.Replace(" ", "_".Replace(",", "_").Replace(":", "_"));

                    //Define Position To Place This Button In The Parent, Appended To Later
                    if (BottomHalf || FullSizeButton)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x + num * X,
                            transform.localPosition.y + num * (Y - 1.95f), transform.localPosition.z);
                    }
                    else
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x + num * X,
                            transform.localPosition.y + num * (Y - 1.45f), transform.localPosition.z);
                    }

                    //Define Where To Put This Button
                    transform.SetParent(Parent, worldPositionStays: false);

                    //Set Text, Tooltip & Colours
                    transform.GetComponentInChildren<Text>().supportRichText = true;
                    transform.GetComponentInChildren<Text>().text = Text;
                    transform.GetComponentInChildren<UiTooltip>().text = ToolTip;

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
                        transform.GetComponentInChildren<Image>().color = (Color) BorderColour;
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

                    //Return The GameObject For Handling It Elsewhere
                    return transform.gameObject;
                }
            #endregion Button Creation

            #region Sub Menu Creation And Handling
                /// <summary>
                /// Creates A Empty Page For Adding Buttons To, If The Page Already Exists, This Will Return It. | Created By Plague | Discord Server: http://discord.me/Poppy
                /// </summary>
                /// <param name="name">
                /// The Name You Want To Give The Page/Find Internally.
                /// </param>
                public static GameObject MakeEmptyPage(string name)
                {
                    if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                    {
                        MelonLogger.Log("Your Empty Page Name Cannot Be Empty!");
                        return null;
                    }

                    //If This Page Already Exists, Return It
                    foreach (GameObject menu in SubMenus)
                    {
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
                    transform.SetParent(QuickMenu.prop_QuickMenu_0.transform, worldPositionStays: false);

                    //Make This Page We Cloned Inactive By Default
                    transform.gameObject.SetActive(value: false);

                    //Add It To The Handler
                    SubMenus.Add(transform.gameObject);

                    //Start SubMenu Handler
                    if (!HandlerIsRunning)
                    {
                        MelonCoroutines.Start(SubMenuHandler());
                    }

                    //Return The GameObject For Handling It Elsewhere
                    return transform.gameObject;
                }

                /// <summary>
                /// Sets A Button To Be Interactable Or Not. | Created By Plague | Discord Server: http://discord.me/Poppy
                /// </summary>
                /// <param name="button">
                /// The GameObject Of The Button To Set The Interactivity Of.
                /// </param>
                /// <param name="state">
                /// If You Want The Button To Be Interactable.
                /// </param>
                public static void SetButtonInteractivity(GameObject button, bool state)
                {
                    button.transform.GetComponent<Button>().interactable = state;
                }

                /// <summary>
                /// Returns The Sprite Of A Given Button's GameObject. | Created By Plague | Discord Server: http://discord.me/Poppy
                /// </summary>
                /// <param name="button">
                /// The GameObject Of The Button To Pull The Sprite From.
                /// </param>
                public static Sprite GetSpriteFromButton(GameObject button)
                {
                    return button.transform.GetComponentInChildren<Image>().sprite;
                }

                /// <summary>
                /// Finds A SubMenu Inside Said Transform Created By My Button API. This Method Will Not Create One Under This Name If Not Found. | Created By Plague | Discord Server: http://discord.me/Poppy
                /// </summary>
                /// <param name="name">
                /// The Name OF The SubMenu To Find.
                /// </param>
                /// <param name="WhereTheSubMenuIsInside">
                /// Where You Placed The SubMenu, Such As The ShortcutMenu Or UserInteractMenu.
                /// </param>
                public static GameObject FindSubMenu(string name, Transform WhereTheSubMenuIsInside)
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
                /// Enters The Submenu. | Created By Plague | Discord Server: http://discord.me/Poppy
                /// </summary>
                /// <param name="name">
                /// The GameObject Of The SubMenu You Want To Enter.
                /// </param>
                public static void EnterSubMenu(GameObject menu)
                {
                    if (ShortcutMenuTransform.gameObject.active)
                    {
                        ShortcutMenuTransform.gameObject.SetActive(false);
                    }

                    if (UserInteractMenuTransform.gameObject.active)
                    {
                        UserInteractMenuTransform.gameObject.SetActive(false);
                    }

                    foreach (GameObject Menu in SubMenus)
                    {
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
                public static void CloseAllSubMenus()
                {
                    ShortcutMenuTransform.gameObject.SetActive(false);
                    UserInteractMenuTransform.gameObject.SetActive(false);

                    foreach (GameObject Menu in SubMenus)
                    {
                        Menu.SetActive(false);
                    }
                }

                /// <summary>
                /// Sets The Buttons Toggle State. | Created By Plague | Discord Server: http://discord.me/Poppy
                /// </summary>
                /// <param name="button">
                /// The GameObject Of The Button You Wish To Set The Toggle State Of.
                /// </param>
                /// <param name="OffColour">
                /// The Off Colour You Chose For When The Button Is Toggled On Before.
                /// </param>
                /// <param name="OnColour">
                /// The On Colour You Chose For When The Button Is Toggled On Before.
                /// </param>
                /// <param name="StateToSetTo">
                /// The Toggle State You Wish To Set This Button To.
                /// </param>
                public static void SetToggleState(GameObject button, Color OffColour, Color OnColour, bool StateToSetTo)
                {
                    if (button != null)
                    {
                        if (button.GetComponentInChildren<Text>().color == OnColour)
                        {
                            button.GetComponentInChildren<Text>().color = OffColour;
                        }
                        else
                        {
                            button.GetComponentInChildren<Text>().color = OnColour;
                        }
                    }
                }
            #endregion Sub Menu Creation And Handling
        #endregion Main Functions

        #region Internal Enumerations
            /// <summary>
            /// The Horizontal Position Of The Button You Are Creating.
            /// </summary>
            public enum HorizontalPosition
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
            public enum VerticalPosition
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
            public enum ButtonType
            {
                Default,
                Toggle
            }
        #endregion Internal Enumerations

        #region Internal Functions - Not For The End User
            //Any Created Sub Menus By The User Are Stored Here
            private static List<GameObject> SubMenus = new List<GameObject>();

            //If The Handler Was Ran And Therefore, Is Looping
            private static bool HandlerIsRunning = false;

            private static IEnumerator SubMenuHandler()
            {
                HandlerIsRunning = true;

                //If User Has Loaded A World
                if (RoomManagerBase.prop_Boolean_3)
                {
                    //Vital Null Check
                    if (VRCPlayer.field_Internal_Static_VRCPlayer_0 != null)
                    {
                        foreach (GameObject Menu in SubMenus)
                        {
                            if (Menu.active)
                            {
                                if (!QuickMenu.prop_QuickMenu_0.prop_Boolean_0 && !ShortcutMenuTransform.gameObject.active)
                                {
                                    Menu.SetActive(false);
                                }
                                else
                                {
                                    ShortcutMenuTransform.gameObject.SetActive(false);
                                }
                            }
                        }
                    }
                }

                //Wait
                yield return new WaitForSeconds(0.1f);

                //Re-Run Self
                MelonCoroutines.Start(SubMenuHandler());
            }
        #endregion Internal Functions - Not For The End User
    }
}
