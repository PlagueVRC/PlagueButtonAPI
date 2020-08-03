using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlagueButtonAPI
{
    public class ButtonAPI
    {
        #region Creditation And Disclaimer
        // Plague Button API
        // Created By @Plague#2850
        // http://discord.me/Poppy
        // Copyright Reserved
        // MIT Licensed
        // https://github.com/OFWModz/PlagueButtonAPI
        #endregion

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
        ///         ButtonAPI.CreateButton(ButtonAPI.ButtonType.Toggle, "Toggle Pickups", "Toggles All Pickups In The Current Instance.", ButtonAPI.HorizontalPosition.FirstButtonPos, ButtonAPI.VerticalPosition.TopButton, null, delegate (bool a)
        ///            {
        ///                //Do Something Here
        ///            }, Color.magenta, null, false, false, true);
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
        /// <param name="CurrentToggleState">
        /// The Toggle State You Want The Button To Be On Creation.
        /// </param>
        public static GameObject CreateButton(ButtonType ButtonType, string Text, string ToolTip, HorizontalPosition X, VerticalPosition Y, Transform Parent, Action<bool> ButtonListener, Color ToggledOnTextColour, Color? BorderColour, bool FullSizeButton = false, bool    BottomHalf = true, bool CurrentToggleState = false)
            {
                //Prevent Weird Bugs Due To A Invalid Parent - Set It To The Main QuickMenu
                if (Parent == null)
                {
                    Parent = QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu").gameObject.transform;
                }

                //Get The Transform Of The Settings Button - Which We Are Going To Use As Our Template
                Transform transform = UnityEngine.Object.Instantiate(QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu/SettingsButton").gameObject).transform;

                //Button Position Calculation
                float num = QuickMenu.prop_QuickMenu_0.transform.Find("UserInteractMenu/ForceLogoutButton").localPosition.x - QuickMenu.prop_QuickMenu_0.transform.Find("UserInteractMenu/BanButton").localPosition.x;

                //Define Position To Place This Button In The Parent, Appended To Later
                if (BottomHalf || FullSizeButton)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x + num * (float)X, transform.localPosition.y + num * ((float)Y - 1.95f), transform.localPosition.z);
                }
                else
                {
                    transform.localPosition = new Vector3(transform.localPosition.x + num * (float)X, transform.localPosition.y + num * ((float)Y - 1.45f), transform.localPosition.z);
                }

                //Define Where To Put This Button
                transform.SetParent(Parent, worldPositionStays: false);

                //Set Text, Tooltip & Colours
                transform.GetComponentInChildren<Text>().text = Text;
                transform.GetComponentInChildren<UiTooltip>().text = ToolTip;

                if (CurrentToggleState && ButtonType != ButtonAPI.ButtonType.Default)
                {
                    transform.GetComponentInChildren<Text>().color = ToggledOnTextColour;
                }
                else
                {
                    transform.GetComponentInChildren<Text>().color = Color.white;
                }

                //Set The Button's Border Colour
                if (BorderColour != null)
                {
                    transform.GetComponentInChildren<Image>().color = (Color)BorderColour;
                }

                //Size Scaling & Repositioning
                if (!FullSizeButton)
                {
                    transform.localPosition += new Vector3(0f, transform.GetComponent<RectTransform>().sizeDelta.y / 5f, 0f);
                    transform.localPosition -= new Vector3(0f, transform.GetComponent<RectTransform>().sizeDelta.y / 2f, 0f);
                    transform.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, transform.GetComponent<RectTransform>().sizeDelta.y / 2f);
                }
                else
                {
                    transform.localPosition -= new Vector3(0f, 20f, 0f);
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
                    //Set The Text Colour To The Toggle State, Magenta Being Toggled On
                    transform.GetComponent<Button>().onClick.AddListener(new Action(() =>
                    {
                        if (transform.GetComponentInChildren<Text>().color == ToggledOnTextColour)
                        {
                            transform.GetComponentInChildren<Text>().color = Color.white;
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
            #endregion

            #region Sub Menu Creation And Handling
            /// <summary>
            /// Creates A Empty Page For Adding Buttons To. | Created By Plague | Discord Server: http://discord.me/Poppy
            /// </summary>

            /// <param name="name">
            /// The Name You Want To Give The Page Internally.
            /// </param>
            public static GameObject MakeEmptyPage(string name)
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                {
                    MelonLoader.MelonModLogger.Log("Your Empty Page Name Cannot Be Empty!");
                    return null;
                }

                //Clone The ShortcutMenu
                Transform transform = UnityEngine.Object.Instantiate(QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu").gameObject).transform;

                //Change Internal Names
                transform.transform.name = name;
                transform.name = name;

                //Remove All Buttons
                for (int i = 0; i < transform.childCount; i++)
                {
                    UnityEngine.Object.Destroy(transform.GetChild(i).gameObject);
                }

                //Make This Page We Cloned A Child Of The ShortcutMenu
                transform.SetParent(QuickMenu.prop_QuickMenu_0.transform, worldPositionStays: false);

                //Make This Page We Cloned Inactive By Default
                transform.gameObject.SetActive(value: false);

                //Add It To The Handler
                SubMenus.Add(transform.gameObject);

                //Start SubMenu Handler
                if (!HandlerIsRunning)
                {
                    MelonLoader.MelonCoroutines.Start(SubMenuHandler());
                }

                //Return The GameObject For Handling It Elsewhere
                return transform.gameObject;
            }

            /// <summary>
            /// Enters The Submenu | Created By Plague | Discord Server: http://discord.me/Poppy
            /// </summary>

            /// <param name="name">
            /// The GameObject Of The SubMenu You Want To Enter
            /// </param>
            public static void EnterSubMenu(GameObject menu)
            {
                if (QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu").gameObject.active)
                {
                    QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu").gameObject.SetActive(false);
                }

                if (QuickMenu.prop_QuickMenu_0.transform.Find("UserInteractMenu").gameObject.active)
                {
                    QuickMenu.prop_QuickMenu_0.transform.Find("UserInteractMenu").gameObject.SetActive(false);
                }

                if (menu != null)
                {
                    menu.SetActive(true);
                }
            }
            #endregion
        #endregion

        #region Internal Enumerations
        /// <summary>
        /// The Horizontal Position Of The Button You Are Creating.
        /// </summary>
        public enum HorizontalPosition
        {
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
        #endregion

        #region Internal Functions - Not For The End User
        //Any Created Sub Menus By The User Are Stored Here
        private static List<GameObject> SubMenus = new List<GameObject>();

        //If The Handler Was Ran And Therefore, Is Looping
        private static bool HandlerIsRunning = false;

        private static IEnumerator SubMenuHandler()
        {
            if (!HandlerIsRunning)
            {
                HandlerIsRunning = true;
            }

            //If User Has Loaded A World
            if (RoomManagerBase.prop_Boolean_3)
            {
                //Vital Null Check
                if (VRCPlayer.field_Internal_Static_VRCPlayer_0 != null)
                {
                    if (!QuickMenu.prop_QuickMenu_0.prop_Boolean_0 && !QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu").gameObject.active)
                    {
                        foreach (GameObject Menu in SubMenus)
                        {
                            Menu.SetActive(false);
                        }
                    }
                }
            }

            //Wait
            yield return new WaitForSeconds(0.1f);

            //Re-Run Self
            MelonLoader.MelonCoroutines.Start(SubMenuHandler());
        }
        #endregion

    }
}
