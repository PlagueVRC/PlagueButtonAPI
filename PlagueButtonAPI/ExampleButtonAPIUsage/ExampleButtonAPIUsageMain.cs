using LoadSprite;
using MelonLoader;
using PlagueButtonAPI;
using PlagueButtonAPI.Controls;
using PlagueButtonAPI.Controls.Grouping;
using PlagueButtonAPI.Pages;
using System;
using System.Collections.Generic;
using PlagueButtonAPI.Misc;
using UIExpansionKit.API;
using UnityEngine;
using VRC;

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

            VRChatUtilityKit.Utilities.NetworkEvents.OnAvatarInstantiated += NetworkEvents_OnAvatarInstantiated;
        }

        private Dictionary<string, Sprite> UserImages = new Dictionary<string, Sprite>();
        private void NetworkEvents_OnAvatarInstantiated(VRCAvatarManager arg1, VRC.Core.ApiAvatar arg2, GameObject arg3)
        {
            var tex = Utils.TakePictureOfPlayer(arg1.field_Private_VRCPlayer_0);

            var sprite = Utils.CreateSpriteFromTex(tex);

            UserImages[arg1.field_Private_VRCPlayer_0.gameObject.GetOrAddComponent<Player>().field_Private_APIUser_0.id] = sprite;
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
                    }).SetToggleState(false, true);

                    var NonFunctionalGroup = new ButtonGroup(Page, "Non-Functional Options");

                    var PlayerListMenu = new MenuPage("PlayersList_1", "Player List", false);

                    var PlayersGroup = new ButtonGroup(PlayerListMenu, "", true, TextAnchor.UpperLeft);
                    var Handler = PlayersGroup.gameObject.GetOrAddComponent<ObjectHandler>();

                    Handler.OnUpdateEachSecond += (obj, IsEnabled) =>
                    {
                        if (IsEnabled)
                        {
                            PlayersGroup.gameObject.transform.DestroyChildren();

                            foreach (var player in Utils.GetAllPlayers())
                            {
                                var image = UserImages.ContainsKey(player.field_Private_APIUser_0.id) ? UserImages[player.field_Private_APIUser_0.id] : null;

                                if (player.field_Private_APIUser_0 == null)
                                {
                                    MelonLogger.Error("Null APIUser!");
                                    continue;
                                }

                                new SingleButton(PlayersGroup, player.field_Private_APIUser_0.displayName, "Selects This Player", () =>
                                {
                                    
                                }, true, image);
                            }
                        }
                    };

                    new SingleButton(NonFunctionalGroup, "Player List", "Opens A Basic Player List", () =>
                    {
                        PlayerListMenu.OpenMenu();
                    }, true, ButtonImage);

                    new SimpleSingleButton(NonFunctionalGroup, "Simple Button", "Simple Button", () =>
                    {
                        MelonLogger.Msg("Simple Button Clicked!");
                    });

                    new ToggleButton(NonFunctionalGroup, "Toggle", "Toggle Off", "Toggle On", (val) =>
                    {
                        MelonLogger.Msg("Toggle Button Clicked! -> State: " + val);
                    }).SetToggleState(true, true);

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
                    }, false, ButtonImage);

                    new Label(Dropdown, "Label", "Label", () =>
                    {
                        MelonLogger.Msg("Label Clicked!");
                    });

                    new Label(Page, "Thanks For Looking. :)", null);

                    //Ideally Use UIX For Menu Entering Instead Of Tabs. You Can Also Use UIX When You Select Someone To Enter Your Menu.
                    Player SelectedPlayer = null;

                    var UserPage = new MenuPage("UserTestMenu_1", "User Menu");

                    var OptionsGroup = new ButtonGroup(UserPage, "Options");

                    new SingleButton(OptionsGroup, "Button", "Button", () =>
                    {
                        MelonLogger.Msg("Button Clicked! - Selected Player: " + (SelectedPlayer != null ? SelectedPlayer.field_Private_APIUser_0.displayName : "<Null>"));
                    }, false, ButtonImage);

                    ExpansionKitApi.GetExpandedMenu(ExpandedMenu.UserQuickMenu).AddSimpleButton("User Options Example", () =>
                    {
                        SelectedPlayer = Utils.GetCurrentlySelectedPlayer();

                        UserPage.SetTitle("User Menu: " + (SelectedPlayer != null ? SelectedPlayer.field_Private_APIUser_0.displayName : "<Null>"));

                        UserPage.OpenMenu();
                    });

                    ExpansionKitApi.GetExpandedMenu(ExpandedMenu.UserQuickMenuRemote).AddSimpleButton("User Options Example", () =>
                    {
                        SelectedPlayer = Utils.GetCurrentlySelectedPlayer();

                        UserPage.SetTitle("User Menu: " + (SelectedPlayer != null ? SelectedPlayer.field_Private_APIUser_0.displayName : "<Null>"));

                        UserPage.OpenMenu();
                    });
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
