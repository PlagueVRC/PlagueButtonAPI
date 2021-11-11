using MelonLoader;
using PlagueButtonAPI;
using System;
using LoadSprite;
using PlagueButtonAPI.Controls;
using PlagueButtonAPI.Controls.Grouping;
using PlagueButtonAPI.Pages;
using UnityEngine;

[assembly: MelonInfo(typeof(ExampleButtonAPIUsage.ExampleButtonAPIUsageMain), "Example PlagueButtonAPI Usage", "1.0", "Plague")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace ExampleButtonAPIUsage
{
    public class ExampleButtonAPIUsageMain : MelonMod
    {
        #region Variables

        internal static bool DisablePortals = false;
        internal static Sprite ButtonImage = null;

        #endregion

        public override void OnApplicationStart()
        {
            ButtonImage = (Environment.CurrentDirectory + "\\ImageToLoad.png").LoadSpriteFromDisk();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "ui")
            {
                ButtonAPI.OnInit += () =>
                {
                    var Page = new MenuPage("TestMenu_1", "Main Menu");

                    new Tab(Page, ButtonImage);

                    var FunctionalGroup = new ButtonGroup(Page, "Functional Options");

                    new ToggleButton(FunctionalGroup, "Disable Portals", "Re-Enables Portals", "Disables Portals Entirely", (val) =>
                    {
                        DisablePortals = val;
                        Functions.TogglePortals(!val);
                    });

                    var NonFunctionalGroup = new ButtonGroup(Page, "Non-Functional Options");

                    new SingleButton(NonFunctionalGroup, "Button", "Button", () =>
                    {
                        MelonLogger.Msg("Button Clicked!");
                    }, ButtonImage);

                    new SimpleSingleButton(NonFunctionalGroup, "Simple Button", "Simple Button", () =>
                    {
                        MelonLogger.Msg("Simple Button Clicked!");
                    });

                    new ToggleButton(NonFunctionalGroup, "Toggle", "Toggle Off", "Toggle On", (val) =>
                    {
                        MelonLogger.Msg("Toggle Button Clicked! -> State: " + val);
                    });

                    new Slider(NonFunctionalGroup, "Slider", "Slider", (val) =>
                    {
                        MelonLogger.Msg("Slider Adjusted! -> State: " + val);
                    });

                    new Slider(Page, "Slider", "Slider", (val) =>
                    {
                        MelonLogger.Msg("Slider Adjusted! -> State: " + val);
                    });

                    var Dropdown = new CollapsibleButtonGroup(Page, "Dropdown", "Toggles The Dropdown");

                    new SingleButton(Dropdown, "Button", "Button", () =>
                    {
                        MelonLogger.Msg("Button Clicked!");
                    }, ButtonImage);

                    new Label(Dropdown, "Label", "Label", () =>
                    {
                        MelonLogger.Msg("Label Clicked!");
                    });

                    new Label(Page, "Thanks For Looking. :)", null);

                    //Ideally Use UIX For Menu Entering Instead Of Tabs. You Can Also Use UIX When You Select Someone To Enter Your Menu.
                };
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
