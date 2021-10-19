using LoadSprite;
using MelonLoader;
using PlagueButtonAPI;
using System;
using System.IO;
using UnityEngine;

[assembly: MelonInfo(typeof(ExampleButtonAPIUsage.ExampleButtonAPIUsageMain), "Example PlagueButtonAPI Usage", "1.0", "Plague")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace ExampleButtonAPIUsage
{
    public class ExampleButtonAPIUsageMain : MelonMod
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

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "ui")
            {
                ButtonAPI.MakeEmptyPage(ButtonAPI.Wing.Left, "TestMenu_1", "Test Menu", "Opens The Test Menu", null, null,
                    (page) =>
                    {
                        ButtonAPI.CreateButton(page, "Test Button 1", "A Test Button",
                            () =>
                            {
                                MelonLogger.Msg("Test Button 1 Clicked!");
                            });

                        ButtonAPI.CreateToggle(page, "Disable Portals", "Disables Portals Entirely.",
                            (a) =>
                            {
                                DisablePortals = a;
                            }, false, false);

                        ButtonAPI.CreateToggle(page, "Test Toggle 2", "A Test Toggle",
                            (a) =>
                            {
                                MelonLogger.Msg("Test Toggle 2 Clicked! - State: " + a);
                            }, true, true);

                        ButtonAPI.MakeEmptyPage(ButtonAPI.Wing.Left, "TestSubMenu_1", "Test Sub Menu", "Opens The Test Sub Menu", page.page);

                        ButtonAPI.CreateSlider(page, "Test Slider", "A Test Slider", (val) =>
                        {
                            MelonLogger.Msg("Slider Value Changed! - Value: " + val);
                        }, 0f, 0f, 100f);
                    });
            }
        }

        internal static float OnUpdateRoutineDelay = 0f;

        public override void OnUpdate()
        {
            try
            {
                if (Time.time > OnUpdateRoutineDelay && VRCUtils.IsWorldLoaded)
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
