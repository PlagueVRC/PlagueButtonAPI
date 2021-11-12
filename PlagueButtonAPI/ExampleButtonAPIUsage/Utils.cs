using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using VRC.UI.Elements.Menus;

namespace ExampleButtonAPIUsage
{
    internal class Utils
    {
        internal static Player GetCurrentlySelectedPlayer()
        {
            if (GameObject.Find("UserInterface").GetComponentInChildren<SelectedUserMenuQM>() == null)
            {
                return null;
            }

            return GetPlayerFromIDInLobby(GameObject.Find("UserInterface").GetComponentInChildren<SelectedUserMenuQM>().field_Private_IUser_0.prop_String_0);
        }

        internal static Player GetPlayerFromIDInLobby(string id)
        {
            List<Player> all_player = GetAllPlayers();

            foreach (var player in all_player)
            {
                if (player != null && player.prop_APIUser_0 != null)
                {
                    if (player.prop_APIUser_0.id == id)
                    {
                        return player;
                    }
                }
            }

            return null;
        }

        internal static List<Player> GetAllPlayers()
        {
            return PlayerManager.field_Private_Static_PlayerManager_0 == null ? null : PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.ToArray().ToList();
        }
    }
}
