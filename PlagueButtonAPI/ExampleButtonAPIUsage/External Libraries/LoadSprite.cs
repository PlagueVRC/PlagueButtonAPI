using System.IO;
using UnityEngine;

namespace LoadSprite
{
    internal static class LoadSprite
    {
        internal static Sprite LoadSpriteFromDisk(this string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            byte[] data = File.ReadAllBytes(path);

            if (data == null || data.Length <= 0)
            {
                return null;
            }

            Texture2D tex = new Texture2D(512, 512);

            if (!Il2CppImageConversionManager.LoadImage(tex, data))
            {
                return null;
            }

            Sprite sprite = Sprite.CreateSprite(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f, 0, 0, new Vector4(), false);

            sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            return sprite;
        }
    }
}
