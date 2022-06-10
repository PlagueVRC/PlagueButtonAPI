using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MelonLoader;
using UnityEngine;
using VRC;
using VRC.Core;

namespace PlagueButtonAPI
{
    public class Hooks
    {
        #region Hooks (I Like To Call Actions/Events Hooks, Nu Booli.)

        public static List<Player> PlayerList = new();
        public static List<Player> PlayerList_ExcludingSelf = new();

        public static event Action<Player> OnPlayerJoin;
        public static event Action<Player> OnPlayerLeave;

        public static event Action OnRoomJoin;
        public static event Action OnRoomCreated;
        public static event Action OnRoomLeave;
        public static event Action OnInstanceMasterSwitched;

        public static event Action<VRCAvatarManager, ApiAvatar, GameObject> OnAvatarInstantiated;

        [Obsolete("This Hook Is Mostly Pointless, And Is Likely To Be Removed In The Future.")]
        public static event Action<VRCAvatarManager, GameObject, string, float, ApiAvatar> OnAvatarChanged_Pre;

        // Post Has Some Use Cases.
        public static event Action<VRCAvatarManager, GameObject, string, float, ApiAvatar> OnAvatarChanged_Post;

        #endregion

        internal static void Init()
        {
            Mod.harmony.Patch(typeof(VRCPlayer).GetMethod(nameof(VRCPlayer.Awake), AccessTools.all), postfix: new HarmonyMethod(typeof(Hooks).GetMethod(nameof(VRCPlayer_AwakeM), BindingFlags.NonPublic | BindingFlags.Static))); // Post So It Exists.

            // My Join/Leave Hook Method Is Nicer :P
            Mod.harmony.Patch(typeof(Player).GetMethod(nameof(Player.OnDestroy), AccessTools.all), new HarmonyMethod(typeof(Hooks).GetMethod(nameof(Player_OnDestroyM), BindingFlags.NonPublic | BindingFlags.Static))); // Pre So We Can Pass The Object And It **Hopefully** Not Be Dead Yet.

            #region NetworkManager
            Mod.harmony.Patch(typeof(NetworkManager).GetMethod(nameof(NetworkManager.OnJoinedRoom), AccessTools.all), postfix: new HarmonyMethod(typeof(Hooks).GetMethod(nameof(Room_JoinM), BindingFlags.NonPublic | BindingFlags.Static)));

            Mod.harmony.Patch(typeof(NetworkManager).GetMethod(nameof(NetworkManager.OnCreatedRoom), AccessTools.all), postfix: new HarmonyMethod(typeof(Hooks).GetMethod(nameof(Room_CreatedM), BindingFlags.NonPublic | BindingFlags.Static)));

            Mod.harmony.Patch(typeof(NetworkManager).GetMethod(nameof(NetworkManager.OnLeftRoom), AccessTools.all), postfix: new HarmonyMethod(typeof(Hooks).GetMethod(nameof(Room_LeaveM), BindingFlags.NonPublic | BindingFlags.Static)));

            Mod.harmony.Patch(typeof(NetworkManager).GetMethod(nameof(NetworkManager.OnMasterClientSwitched), AccessTools.all), postfix: new HarmonyMethod(typeof(Hooks).GetMethod(nameof(Instance_Master_SwitchedM), BindingFlags.NonPublic | BindingFlags.Static)));
            #endregion

            // Using Contains Like This Resists Against VRC Changing The Method To Internal, Public, Etc.
            Mod.harmony.Patch(typeof(VRCAvatarManager).GetMethods().First(mb => mb.Name.Contains("_Boolean_GameObject_String_Single_ApiAvatar_")), new HarmonyMethod(typeof(Hooks).GetMethod(nameof(Avatar_ChangedM_Pre), BindingFlags.NonPublic | BindingFlags.Static)), new HarmonyMethod(typeof(Hooks).GetMethod(nameof(Avatar_ChangedM_Post), BindingFlags.NonPublic | BindingFlags.Static))); // Pre & Post; For People Perhaps Making An AntiCrash. I Plan To Add A Beyond Earlier Hook. This Is Not Suggested For AntiCrashes Or Alike, As It Runs Too Late To Be Too Useful. Here Mostly For Fun.
        }

        private static void VRCPlayer_AwakeM(VRCPlayer __instance)
        {
            __instance?.Method_Public_add_Void_OnAvatarIsReady_0(new Action(() => OnAvatarInstantiated?.Invoke(__instance.prop_VRCAvatarManager_0, __instance.field_Private_ApiAvatar_0, __instance.field_Internal_GameObject_0)));

            MelonCoroutines.Start(RunMe());

            IEnumerator RunMe()
            {
                while (__instance?.gameObject?.GetComponent<Player>()?.field_Private_APIUser_0 == null) // Wait For APIUser To Exist, Tempermental If Not Done.
                {
                    yield return new WaitForEndOfFrame();
                }

                var player = __instance.gameObject.GetComponent<Player>();

                OnPlayerJoin?.Invoke(player);
                PlayerList.Add(player);

                if (player != Player.prop_Player_0)
                {
                    PlayerList_ExcludingSelf.Add(player);
                }

                yield break;
            }
        }

        private static void Player_OnDestroyM(Player __instance)
        {
            OnPlayerLeave?.Invoke(__instance);
            PlayerList.Remove(__instance);

            if (__instance != Player.prop_Player_0)
            {
                PlayerList_ExcludingSelf.Remove(__instance);
            }
        }

        private static void Room_JoinM() => OnRoomJoin?.Invoke();
        private static void Room_CreatedM() => OnRoomCreated?.Invoke();
        private static void Room_LeaveM() => OnRoomLeave?.Invoke();
        private static void Instance_Master_SwitchedM() => OnInstanceMasterSwitched?.Invoke();

        private static void Avatar_ChangedM_Pre(VRCAvatarManager __instance, GameObject __0, string __1, float __2, ApiAvatar __3) => OnAvatarChanged_Pre?.Invoke(__instance, __0, __1, __2, __3);

        private static void Avatar_ChangedM_Post(VRCAvatarManager __instance, GameObject __0, string __1, float __2, ApiAvatar __3) => OnAvatarChanged_Post?.Invoke(__instance, __0, __1, __2, __3);
    }
}
