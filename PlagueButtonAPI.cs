using System;
using UnityEngine;
using UnityEngine.UI;

namespace PlagueButtonAPI
{
    public static class ButtonAPI
    {
        public enum HorizontalPosition
        {
            LeftOfMenu = -4,

            FirstButtonPos = -3,

            SecondButtonPos = -2,

            ThirdButtonPos = -1,

            FourthButtonPos = 0,

            RightOfMenu = 1
        }

        public enum VerticalPosition
        {
            AboveMenu = 5,

            AboveTopButton = 4,

            TopButton = 3,

            SecondButton = 2,

            BottomButton = 1,

            BelowBottomButton = 0
        }

        public enum ButtonType
        {
            Default,
            Toggle
        }

        public static GameObject CreateButton(ButtonType ButtonType, string Text, string ToolTip, HorizontalPosition X, VerticalPosition Y, Transform Parent, Action<bool> ButtonListener, Color ToggledOnTextColour, Color? BorderColour, bool FullSizeButton = false, bool BottomHalf = true, bool CurrentToggleState = false)
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
            if (BottomHalf)
            {
                transform.localPosition = new Vector3(transform.localPosition.x + num * (float)X, transform.localPosition.y + num * ((float)Y - 1.95f), transform.localPosition.z);
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x + num * (float)X, transform.localPosition.y + num * ((float)Y - 1.35f), transform.localPosition.z);
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
    }
}
