using LoadSprite;
using MelonLoader;
using PlagueButtonAPI;
using System;
using System.Collections;
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
            MelonCoroutines.Start(RunMe());

            IEnumerator RunMe()
            {
                yield return new WaitForSeconds(15f);

                var TestMenu_1 = ButtonAPI.MakeEmptyPage(ButtonAPI.Wing.Left, "TestMenu_1", "Test Menu", "Opens The Test Menu");

                ButtonAPI.CreateButton(TestMenu_1.transform, "TestButton_1", "Test Button 1", "A Test Button",
                () =>
                {
                    MelonLogger.Msg("Test Button 1 Clicked!");
                });

                ButtonAPI.CreateToggle(TestMenu_1.transform, "TestToggle_1", "Test Toggle 1", "A Test Toggle",
                    (a) =>
                    {
                        MelonLogger.Msg("Test Toggle 1 Clicked! - State: " + a);
                    }, false, Color.red);

                yield break;
            }
        }

        internal static float OnUpdateRoutineDelay = 0f;

        public override void OnUpdate()
        {
            try
            {
                if (VRCUtils.IsWorldLoaded && Time.time > OnUpdateRoutineDelay)
                {
                    OnUpdateRoutineDelay = Time.time + 1f;
                    if (DisablePortals)
                    {
                        Functions.TogglePortals(false);
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Msg("Error in OnUpdate! - " + ex.Message + " From: " + ex.Source + " - Stack: " + ex.StackTrace);
            }
        }
    }
}
