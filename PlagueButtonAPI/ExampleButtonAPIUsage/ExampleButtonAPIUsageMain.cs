﻿using LoadSprite;
using MelonLoader;
using PlagueButtonAPI;
using PlagueButtonAPI.Controls;
using PlagueButtonAPI.Controls.Grouping;
using PlagueButtonAPI.Pages;
using System;
using System.Collections;
using System.Collections.Generic;
using PlagueButtonAPI.Misc;
using UIExpansionKit.API;
using UnityEngine;
using VRC;
using VRChatUtilityKit.Ui;
using ButtonGroup = PlagueButtonAPI.Controls.Grouping.ButtonGroup;
using Label = PlagueButtonAPI.Controls.Label;
using SingleButton = PlagueButtonAPI.Controls.SingleButton;
using ToggleButton = PlagueButtonAPI.Controls.ToggleButton;

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
                    var Combi = MenuPage.CreatePage(WingSingleButton.Wing.Left, ButtonImage, "TestMenu_1", "Main Menu");

                    var Page = Combi.Item1;

                    var FunctionalGroup = Page.AddButtonGroup("Functional Options");

                    FunctionalGroup.AddToggleButton("Disable Portals", "Re-Enables Portals", "Disables Portals Entirely", (val) =>
                    {
                        DisablePortals = val;
                        Functions.TogglePortals(!val);
                    }).SetToggleState(false, true);

                    var NonFunctionalGroup = Page.AddCollapsibleButtonGroup("Non-Functional Options");

                    var PlayerListMenu = NonFunctionalGroup.AddSubMenu(ButtonImage, "PlayersList_1", "Player List", false);

                    var PlayersGroup = PlayerListMenu.Item1.AddButtonGroup("", true, TextAnchor.UpperLeft);
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

                                PlayersGroup.AddSingleButton(player.field_Private_APIUser_0.displayName, "Selects This Player", () =>
                                {
                                    UiManager.OpenUserInQuickMenu(player.field_Private_APIUser_0);
                                }, true, image);
                            }
                        }
                    };

                    NonFunctionalGroup.AddSimpleSingleButton("Show Alerts", "Simple Button", () =>
                    {
                        ButtonAPI.GetQuickMenuInstance().ShowAlert("Alert Test");

                        ButtonAPI.GetQuickMenuInstance().ShowOKDialog("Title", "Message", () =>
                        {
                            MelonLogger.Msg("Okay Clicked! Showing Yes/No Popup..");

                            MelonCoroutines.Start(RunMe());

                            IEnumerator RunMe()
                            {
                                yield return new WaitForSeconds(3f);

                                ButtonAPI.GetQuickMenuInstance().ShowConfirmDialog("Title", "Message", () =>
                                {
                                    MelonLogger.Msg("Yes Clicked!");
                                }, () =>
                                {
                                    MelonLogger.Msg("No Clicked!");
                                });

                                yield break;
                            }
                        });
                    });

                    NonFunctionalGroup.AddToggleButton("Toggle", "Toggle Off", "Toggle On", (val) =>
                    {
                        MelonLogger.Msg("Toggle Button Clicked! -> State: " + val);
                    }).SetToggleState(true, true);

                    NonFunctionalGroup.AddSlider("Slider", "Slider", (val) =>
                    {
                        MelonLogger.Msg("Slider Adjusted! -> State: " + val);
                    });

                    Page.AddSlider("Slider", "Slider", (val) =>
                    {
                        MelonLogger.Msg("Slider Adjusted! -> State: " + val);
                    });

                    var Dropdown = Page.AddCollapsibleButtonGroup("Dropdown");

                    Dropdown.AddSingleButton("Button", "Button", () =>
                    {
                        MelonLogger.Msg("Button Clicked!");
                    }, false, ButtonImage);

                    Dropdown.AddLabel("Label", "Label", () =>
                    {
                        MelonLogger.Msg("Label Clicked!");
                    });

                    Page.AddLabel("Thanks For Looking. :)", null);

                    //Ideally Use UIX For Menu Entering Instead Of Tabs. You Can Also Use UIX When You Select Someone To Enter Your Menu.
                    Player SelectedPlayer = null;

                    var UserPage = MenuPage.CreatePage("UserTestMenu_1", "User Menu");

                    var OptionsGroup = UserPage.AddButtonGroup("Options");

                    OptionsGroup.AddSingleButton("Button", "Button", () =>
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
