using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using PlagueButtonAPI.Main;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;
using VRC;
using VRC.UI.Elements.Menus;

namespace PlagueButtonAPI.Misc
{
    public class Utils
    {
        public static void LoopThisCodeForever(Action action, Func<bool> CancelToken = null)
        {
            MelonCoroutines.Start(RunMe());

            IEnumerator RunMe()
            {
                while (true)
                {
                    if (CancelToken != null && CancelToken.Invoke())
                    {
                        break;
                    }

                    action?.Invoke();

                    yield return new WaitForEndOfFrame();
                }

                yield break;
            }
        }

        public static Player GetCurrentlySelectedPlayer()
        {
            return GetPlayerFromIDInLobby(GameObject.Find("UserInterface")?.gameObject?.GetComponentsInChildren<SelectedUserMenuQM>(true)?.First(o => o.field_Private_IUser_0 != null)?.field_Private_IUser_0?.prop_String_0);
        }

        public static string GetCurrentlySelectedPlayerID_LocalAndRemote()
        {
            return GameObject.Find("UserInterface")?.gameObject?.GetComponentsInChildren<SelectedUserMenuQM>(true)?.First(o => o.field_Private_IUser_0 != null)?.field_Private_IUser_0?.prop_String_0;
        }

        public static Player GetPlayerFromIDInLobby(string id)
        {
            List<Player> all_player = GetAllPlayers();

            foreach (var player in all_player)
            {
                if (player?.prop_APIUser_0 != null)
                {
                    if (player.prop_APIUser_0.id == id)
                    {
                        return player;
                    }
                }
            }

            return null;
        }

        public static List<Player> GetAllPlayers()
        {
            return PlayerManager.field_Private_Static_PlayerManager_0 == null ? null : PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0?.ToArray()?.ToList();
        }

        public static Sprite CreateSpriteFromTex(Texture2D tex)
        {
            Sprite sprite = Sprite.CreateSprite(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f, 0, 0, new Vector4(), false);

            sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            return sprite;
        }

        private static VRCUiManager _uiManagerInstance;

        public static VRCUiManager VRCUiManagerInstance
        {
            get
            {
                if (_uiManagerInstance == null)
                {
                    _uiManagerInstance = (VRCUiManager)typeof(VRCUiManager).GetMethods().First(x => x.ReturnType == typeof(VRCUiManager)).Invoke(null, new object[0]);
                }

                return _uiManagerInstance;
            }
        }

        public static SelectedUserMenuQM SelectedUserLocal => TransformHelper.SelectedUser_Local.parent.parent.parent.GetComponent<SelectedUserMenuQM>();

        public static SelectedUserMenuQM SelectedUserRemote => TransformHelper.SelectedUser_Remote.parent.parent.parent.GetComponent<SelectedUserMenuQM>();

        public static bool CheckMethod(MethodInfo method, string match)
        {
            try
            {
                foreach (var instance in XrefScanner.XrefScan(method))
                {
                    if (instance.Type == XrefType.Global && instance.ReadAsObject().ToString().Contains(match))
                        return true;
                }

                return false;
            }
            catch
            {
            }

            return false;
        }

        public static bool CheckUsedBy(MethodInfo method, string methodName, Type type = null)
        {
            foreach (var instance in XrefScanner.UsedBy(method))
            {
                if (instance.Type == XrefType.Method)
                {
                    try
                    {
                        if ((type == null || instance.TryResolve().DeclaringType == type) && instance.TryResolve().Name.Contains(methodName))
                            return true;
                    }
                    catch
                    {
                    }
                }
            }
            return false;
        }

        public static bool CheckUsing(MethodInfo method, string methodName, Type type = null)
        {
            foreach (var instance in XrefScanner.XrefScan(method))
            {
                if (instance.Type == XrefType.Method)
                {
                    try
                    {
                        if ((type == null || instance.TryResolve().DeclaringType == type) && instance.TryResolve().Name.Contains(methodName))
                            return true;
                    }
                    catch
                    {
                    }
                }
            }
            return false;
        }
    }
}
