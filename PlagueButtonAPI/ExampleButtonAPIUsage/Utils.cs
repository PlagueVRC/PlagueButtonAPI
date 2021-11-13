using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;
using VRC.UI.Elements.Menus;
using PlagueButtonAPI.Misc;

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

            return GetPlayerFromIDInLobby(GameObject.Find("UserInterface").gameObject.GetComponentInChildren<SelectedUserMenuQM>().field_Private_IUser_0.prop_String_0);
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

        internal static Sprite CreateSpriteFromTex(Texture2D tex)
        {
            Sprite sprite = Sprite.CreateSprite(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f, 0, 0, new Vector4(), false);

            sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            return sprite;
        }

        private static GameObject AvatarPicTaker;
        internal static Texture2D TakePictureOfPlayer(VRCPlayer player)
        {
            var avatar = player.transform.Find("ForwardDirection/Avatar").gameObject;
            var manager = player.prop_VRCAvatarManager_0;

            var OldLayer = player == VRCPlayer.field_Internal_Static_VRCPlayer_0
                        ? (1 << LayerMask.NameToLayer("PlayerLocal"))
                        : (1 << LayerMask.NameToLayer("Player"));

            var layer = new System.Random().Next(0, 9999);

            avatar.layer = layer;

            if (AvatarPicTaker == null)
            {
                AvatarPicTaker = new GameObject("AvatarPicTaker");
            }

            AvatarPicTaker.SetActive(false);

            var CamComp = AvatarPicTaker.GetOrAddComponent<Camera>();

            CamComp.clearFlags = CameraClearFlags.SolidColor;
            CamComp.backgroundColor = new Color(0f, 0f, 0f, 0f);

            /* Enable camera */
            AvatarPicTaker.SetActive(true);

            /* Move camera infront of head */
            var descriptor = (manager.prop_VRCAvatarDescriptor_0 ?? manager.prop_VRC_AvatarDescriptor_1 ?? manager.prop_VRC_AvatarDescriptor_0);
            var head_height = descriptor.ViewPosition.y;
            var head = avatar.transform.position + new Vector3(0, head_height, 0);
            var target = head + avatar.transform.forward * 0.3f;
            var camera = CamComp;

            camera.useOcclusionCulling = false;
            camera.farClipPlane = 0.6f;
            camera.nearClipPlane = 0.05f;
            camera.transform.position = target;
            camera.transform.LookAt(head);

            camera.cullingMask = layer;
            camera.orthographic = true;
            camera.orthographicSize = head_height / 8;

            if (camera.targetTexture == null)
            {
                camera.targetTexture = new RenderTexture(256, 256, 0);
            }

            var currentRT = RenderTexture.active;
            RenderTexture.active = camera.targetTexture;

            camera.Render();

            var image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height,
                    TextureFormat.RGBA32, false, true)
            { name = $"{player.field_Private_ApiAvatar_0.id}" };
            image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
            image.Apply();
            image.hideFlags = HideFlags.DontUnloadUnusedAsset;

            RenderTexture.active = currentRT;

            AvatarPicTaker.SetActive(false);

            avatar.layer = OldLayer;

            return image;
        }
    }
}
