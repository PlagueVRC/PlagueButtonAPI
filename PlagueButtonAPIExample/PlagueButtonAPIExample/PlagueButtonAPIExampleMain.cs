using MelonLoader;
using System;
using UnityEngine;
using PlagueButtonAPI;

namespace PlagueButtonAPIExample
{
    public class PlagueButtonAPIExampleMain : MelonMod
    {
        public static GameObject SubMenu_1;

        public static GameObject SubMenu_2;

        public override void VRChat_OnUiManagerInit()
        {
            SubMenu_1 = ButtonAPI.MakeEmptyPage("SubMenu_1");

            SubMenu_2 = ButtonAPI.MakeEmptyPage("SubMenu_2");

            ButtonAPI.CreateButton(ButtonAPI.ButtonType.Default, "Utils", "Some Random Utils.", ButtonAPI.HorizontalPosition.LeftOfMenu, ButtonAPI.VerticalPosition.TopButton, null, delegate (bool a)
            {
                ButtonAPI.EnterSubMenu(SubMenu_1);
            }, Color.magenta, null, false, true, true);

            //SubMenu_1 Contents
            ButtonAPI.CreateButton(ButtonAPI.ButtonType.Toggle, "Next Page", "Goes To The Next Page.", ButtonAPI.HorizontalPosition.LeftOfMenu, ButtonAPI.VerticalPosition.TopButton, SubMenu_1.transform, delegate (bool a)
            {
                ButtonAPI.EnterSubMenu(SubMenu_2);
            }, Color.magenta, null, false, false, true);

            ButtonAPI.CreateButton(ButtonAPI.ButtonType.Toggle, "Toggle Pickups", "Toggles All Pickups In The Current Instance.", ButtonAPI.HorizontalPosition.FirstButtonPos, ButtonAPI.VerticalPosition.TopButton, SubMenu_1.transform, delegate (bool a)
            {
                TogglePickups(a);
            }, Color.magenta, null, false, false, true);

            //SubMenu_2 Contents
            ButtonAPI.CreateButton(ButtonAPI.ButtonType.Default, "Hello World", "Prints Hello World To Console.", ButtonAPI.HorizontalPosition.FirstButtonPos, ButtonAPI.VerticalPosition.TopButton, SubMenu_2.transform, delegate (bool a)
            {
                MelonModLogger.Log(ConsoleColor.Magenta, "Hello World!");
            }, Color.magenta, null, false, false, true);
        }

        public static void TogglePickups(bool state)
        {
            var array = Resources.FindObjectsOfTypeAll<VRCSDK2.VRC_Pickup>();

            foreach (var i in array)
            {
                if (i == null)
                {
                    continue;
                }

                if (i.name.Contains("ViewFinder"))
                {
                    continue;
                }

                // ;)
                i.DisallowTheft = false;

                if (i.gameObject.active == !state)
                {
                    i.gameObject.SetActive(state);
                }
            }
        }

        /*//Example OnSpawn
        public enum NHDDDDJNDMB
        {
            Undefined,
            Loading,
            Error,
            Blocked,
            Safety,
            Substitute,
            Performance,
            Custom
        }

        public override void OnUpdate()
        {
            try
            {
                //If User Has Loaded A World
                if (RoomManagerBase.prop_Boolean_3)
                {
                    //Vital Null Check
                    if (VRCPlayer.field_Internal_Static_VRCPlayer_0 != null)
                    {
                        //If Avatar Has Loaded In
                        if (Enum.GetName(typeof(NHDDDDJNDMB), VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.prop_EnumNPublicSealedva9vUnique_0).Contains("Custom"))
                        {

                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                MelonModLogger.Log("Error in OnUpdate! " + ex.Message + " in " + ex.Source + " Stack: " + ex.StackTrace);
            }
        }*/
    }
}
