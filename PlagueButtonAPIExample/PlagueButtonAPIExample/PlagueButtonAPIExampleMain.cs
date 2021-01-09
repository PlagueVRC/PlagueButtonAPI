using LoadSprite;
using MelonLoader;
using PlagueButtonAPI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VRC;

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
            Tuple<string, string, Color?, Color?, Action<bool>> SubMenu1Data = Tuple.Create<string, string, Color?, Color?, Action<bool>>("Sub Menu 1", "The First SubMenu", Color.magenta, null,
                delegate(bool a) { Process.Start("http://Krewella.co.uk/ButtonAPI"); });

            Tuple<string, string, Color?, Color?, Action<bool>> SubMenu2Data = Tuple.Create<string, string, Color?, Color?, Action<bool>>("Sub Menu 2", "The Second SubMenu", Color.magenta,
                null, delegate(bool a) { Process.Start("http://Krewella.co.uk/ButtonAPI"); });

            InputField InputField = null;

            InputField = ButtonAPI.CreateInputField("Enter Text Here..", ButtonAPI.VerticalPosition.BelowBottomButton, ButtonAPI.MakeEmptyPage("SubMenu_1", SubMenu1Data.Item1, SubMenu1Data.Item2, SubMenu1Data.Item3, SubMenu1Data.Item4, SubMenu1Data.Item5).transform, delegate (string text)
            {
                //MelonLogger.Log("New Text: " + text);
            }, delegate ()
            {
                if (InputField.text.StartsWith("teleport ") && InputField.text.Length > 9)
                {
                    Player TargetPlayer = PlayerManager.field_Private_Static_PlayerManager_0
                        .field_Private_List_1_Player_0.ToArray()
                        .FirstOrDefault(o => o.field_Private_APIUser_0.displayName.ToLower().StartsWith(InputField.text.Replace("teleport ", "").ToLower()));

                    if (TargetPlayer != null)
                    {
                        MelonLogger.Log("Teleporting To: " + TargetPlayer.field_Private_APIUser_0.displayName);

                        VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position =
                            TargetPlayer.transform.position;
                    }
                    else
                    {
                        MelonLogger.Log("Player: " + InputField.text.Replace("teleport ", "") + " Not Found!");
                    }
                }
            });

            //Make Button To Enter SubMenu_1
            ButtonAPI.CreateButton(ButtonAPI.ButtonType.Default, LoadedImage != null ? "" : "Utils", "Some Random Utils.", ButtonAPI.HorizontalPosition.LeftOfMenu, ButtonAPI.VerticalPosition.TopButton, null, delegate (bool a)
            {
                ButtonAPI.EnterSubMenu(ButtonAPI.MakeEmptyPage("SubMenu_1", SubMenu1Data.Item1, SubMenu1Data.Item2, SubMenu1Data.Item3, SubMenu1Data.Item4, SubMenu1Data.Item5));
            }, Color.white, Color.magenta, null, true, false, false, false, LoadedImage, true);

            //SubMenu_1 Contents
            ButtonAPI.CreateButton(ButtonAPI.ButtonType.Default, "Next Page", "Goes To The Next Page.", ButtonAPI.HorizontalPosition.LeftOfMenu, ButtonAPI.VerticalPosition.TopButton, ButtonAPI.MakeEmptyPage("SubMenu_1", SubMenu1Data.Item1, SubMenu1Data.Item2, SubMenu1Data.Item3, SubMenu1Data.Item4, SubMenu1Data.Item5).transform, delegate (bool a)
            {
                ButtonAPI.EnterSubMenu(ButtonAPI.MakeEmptyPage("SubMenu_2", SubMenu2Data.Item1, SubMenu2Data.Item2, SubMenu2Data.Item3, SubMenu2Data.Item4, SubMenu2Data.Item5));
            }, Color.white, Color.magenta, null, false, false, false, false, null, true);

            ButtonAPI.CreateButton(ButtonAPI.ButtonType.Toggle, "Disable Portals", "Disables Portals Entirely - This Stops That Stupid Notorious Menu Disabling Bullshit", ButtonAPI.HorizontalPosition.FirstButtonPos, ButtonAPI.VerticalPosition.TopButton, ButtonAPI.MakeEmptyPage("SubMenu_1", SubMenu1Data.Item1, SubMenu1Data.Item2, SubMenu1Data.Item3, SubMenu1Data.Item4, SubMenu1Data.Item5).transform, delegate (bool a)
            {
                DisablePortals = a;

                if (!a)
                {
                    Functions.TogglePortals(true);
                }
            }, Color.white, Color.magenta, null, false, false, false, DisablePortals, null, true);

            //Test Checkbox
            ButtonAPI.PlagueButton CheckBox = null;

            CheckBox = ButtonAPI.CreateButton(ButtonAPI.ButtonType.Toggle, "X", "A CheckBox, Ez", ButtonAPI.HorizontalPosition.SecondButtonPos, ButtonAPI.VerticalPosition.SecondButton, ButtonAPI.MakeEmptyPage("SubMenu_1", SubMenu1Data.Item1, SubMenu1Data.Item2, SubMenu1Data.Item3, SubMenu1Data.Item4, SubMenu1Data.Item5).transform, delegate (bool a)
            {
                CheckBox.text.text = (a ? "✓" : "X");
            }, Color.white, Color.magenta, null, false, false, true, false, null, true);

            ButtonAPI.CreateText(ButtonAPI.ButtonType.Toggle, ButtonAPI.SizeType.ButtonSize, "Some Text", "Just Some Text", ButtonAPI.HorizontalPosition.FirstButtonPos, ButtonAPI.VerticalPosition.SecondButton, ButtonAPI.MakeEmptyPage("SubMenu_1", SubMenu1Data.Item1, SubMenu1Data.Item2, SubMenu1Data.Item3, SubMenu1Data.Item4, SubMenu1Data.Item5).transform, true, true, delegate (bool a)
            {
                //Do Something Here
            }, false, Color.magenta, Color.white);

            ButtonAPI.CreateSlider(ButtonAPI.MakeEmptyPage("SubMenu_1", SubMenu1Data.Item1, SubMenu1Data.Item2, SubMenu1Data.Item3, SubMenu1Data.Item4, SubMenu1Data.Item5).transform, delegate (float v)
            {

            }, (float)ButtonAPI.HorizontalPosition.FirstButtonPos, (float)ButtonAPI.VerticalPosition.BottomButton, "Test Slider", 15f, 100f, 0f);

            //SubMenu_2 Contents
            ButtonAPI.CreateButton(ButtonAPI.ButtonType.Default, "Prev Page", "Goes To The Previous Page.", ButtonAPI.HorizontalPosition.LeftOfMenu, ButtonAPI.VerticalPosition.TopButton, ButtonAPI.MakeEmptyPage("SubMenu_2", SubMenu2Data.Item1, SubMenu2Data.Item2, SubMenu2Data.Item3, SubMenu2Data.Item4, SubMenu2Data.Item5).transform, delegate (bool a)
            {
                ButtonAPI.EnterSubMenu(ButtonAPI.MakeEmptyPage("SubMenu_1", SubMenu1Data.Item1, SubMenu1Data.Item2, SubMenu1Data.Item3, SubMenu1Data.Item4, SubMenu1Data.Item5));
            }, Color.white, Color.magenta, null, false, false, false, false, null, true);

            ButtonAPI.CreateButton(ButtonAPI.ButtonType.Default, "Hello World", "Prints Hello World To Console.", ButtonAPI.HorizontalPosition.FirstButtonPos, ButtonAPI.VerticalPosition.TopButton, ButtonAPI.MakeEmptyPage("SubMenu_2", SubMenu2Data.Item1, SubMenu2Data.Item2, SubMenu2Data.Item3, SubMenu2Data.Item4, SubMenu2Data.Item5).transform, delegate (bool a)
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
