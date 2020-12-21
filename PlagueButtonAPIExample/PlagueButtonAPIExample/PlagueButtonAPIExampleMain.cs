using LoadSprite;
using MelonLoader;
using PlagueButtonAPI;
using System;
using System.IO;
using UnityEngine;

namespace PlagueButtonAPIExample
{
    public class PlagueButtonAPIExampleMain : MelonMod
    {
        #region Variables

        internal static Sprite LoadedImage = null;
        internal static bool DisablePortals = false;

        #endregion

        public override void OnApplicationStart()
        {
            if (File.Exists(Environment.CurrentDirectory + "\\ImageToLoad.png"))
            {
                LoadedImage = (Environment.CurrentDirectory + "\\ImageToLoad.png").LoadSpriteFromDisk();
            }
        }

        public override void VRChat_OnUiManagerInit()
        {
            ButtonAPI.CreateInputField("Enter Text Here..", ButtonAPI.VerticalPosition.AboveMenu, ButtonAPI.MakeEmptyPage("SubMenu_1").transform, delegate (string text)
            {
                MelonLogger.Log("New Text: " + text);
            });

            //Make Button To Enter SubMenu_1
            ButtonAPI.CreateButton(ButtonAPI.ButtonType.Default, LoadedImage != null ? "" : "Utils", "Some Random Utils.", ButtonAPI.HorizontalPosition.LeftOfMenu, ButtonAPI.VerticalPosition.TopButton, null, delegate (bool a)
            {
                ButtonAPI.EnterSubMenu(ButtonAPI.MakeEmptyPage("SubMenu_1"));
            }, Color.white, Color.magenta, null, true, false, false, false, LoadedImage ?? LoadedImage, true);

            //SubMenu_1 Contents
            ButtonAPI.CreateButton(ButtonAPI.ButtonType.Default, "Next Page", "Goes To The Next Page.", ButtonAPI.HorizontalPosition.LeftOfMenu, ButtonAPI.VerticalPosition.TopButton, ButtonAPI.MakeEmptyPage("SubMenu_1").transform, delegate (bool a)
            {
                ButtonAPI.EnterSubMenu(ButtonAPI.MakeEmptyPage("SubMenu_2"));
            }, Color.white, Color.magenta, null, false, false, false, false, null, true);

            ButtonAPI.CreateButton(ButtonAPI.ButtonType.Toggle, "Disable Portals", "Disables Portals Entirely - This Stops That Stupid Notorious Menu Disabling Bullshit", ButtonAPI.HorizontalPosition.FirstButtonPos, ButtonAPI.VerticalPosition.TopButton, ButtonAPI.MakeEmptyPage("SubMenu_1").transform, delegate (bool a)
            {
                DisablePortals = a;

                if (!a)
                {
                    Functions.TogglePortals(true);
                }
            }, Color.white, Color.magenta, null, false, false, false, DisablePortals, null, true);

            ButtonAPI.CreateSlider(ButtonAPI.MakeEmptyPage("SubMenu_1").transform, delegate(float v)
            {

            }, (float)ButtonAPI.HorizontalPosition.FirstButtonPos, (float)ButtonAPI.VerticalPosition.SecondButton, "Test Slider", 15f, 100f, 0f);

            //SubMenu_2 Contents
            ButtonAPI.CreateButton(ButtonAPI.ButtonType.Default, "Prev Page", "Goes To The Previous Page.", ButtonAPI.HorizontalPosition.LeftOfMenu, ButtonAPI.VerticalPosition.TopButton, ButtonAPI.MakeEmptyPage("SubMenu_2").transform, delegate (bool a)
            {
                ButtonAPI.EnterSubMenu(ButtonAPI.MakeEmptyPage("SubMenu_1"));
            }, Color.white, Color.magenta, null, false, false, false, false, null, true);

            ButtonAPI.CreateButton(ButtonAPI.ButtonType.Default, "Hello World", "Prints Hello World To Console.", ButtonAPI.HorizontalPosition.FirstButtonPos, ButtonAPI.VerticalPosition.TopButton, ButtonAPI.MakeEmptyPage("SubMenu_2").transform, delegate (bool a)
            {
                MelonLogger.Log(ConsoleColor.Magenta, "Hello World!");
            }, Color.white, Color.magenta, null, false, false, false, false, null, true);
        }

        internal static float OnUpdateRoutineDelay = 0f;

        public override void OnUpdate()
        {
            try
            {
                //If User Has Loaded A World
                if (RoomManager.prop_Boolean_3)
                {
                    ButtonAPI.SubMenuHandler(); // Routine Delay Is Built In

                    if (Time.time > OnUpdateRoutineDelay)
                    {
                        OnUpdateRoutineDelay = Time.time + 1f;

                        if (DisablePortals)
                        {
                            Functions.TogglePortals(false);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                MelonLogger.Log("Error in OnUpdate! - " + ex.Message + " From: " + ex.Source + " - Stack: " + ex.StackTrace);
            }
        }
    }
}
