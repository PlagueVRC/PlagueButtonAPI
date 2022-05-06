using System;
using System.IO;
using System.Linq;
using System.Reflection;

using UnhollowerRuntimeLib;

using UnityEngine;
using Object = UnityEngine.Object;

namespace IL2CPPAssetBundleAPI
{
    public class IL2CPPAssetBundle
    {
        /// <summary>
        /// The Loaded AssetBundle, Null By Default
        /// </summary>
        internal AssetBundle bundle = null;

        internal bool HasLoadedABundle = false;

        internal string error = "";

        internal IL2CPPAssetBundle(string resource = null)
        {
            if (!string.IsNullOrEmpty(resource))
            {
                LoadBundle(resource);
            }
        }

        /// <summary>
        /// Loads An Asset Bundle For Using Data Such As Sprites
        /// </summary>
        /// <param name="resource">The Path To The Embedded Resource File - Example: VRCAntiCrash.Resources.plaguelogo.asset</param>
        /// <returns>True If Successful</returns>
        internal bool LoadBundle(string resource)
        {
            if (HasLoadedABundle)
            {
                return true;
            }

            try
            {
                if (string.IsNullOrEmpty(resource))
                {
                    error = "Null Or Empty Resource String!";
                    return false;
                }

                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);

                if (stream != null && stream.Length > 0)
                {
                    var memStream = new MemoryStream((int)stream.Length);

                    stream.CopyTo(memStream);

                    if (memStream != null && memStream.Length > 0)
                    {
                        var assetBundle = AssetBundle.LoadFromMemory(memStream.ToArray());

                        if (assetBundle != null)
                        {
                            assetBundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;

                            bundle = assetBundle;

                            HasLoadedABundle = true;

                            return true;
                        }

                        assetBundle = AssetBundle.GetAllLoadedAssetBundles_Native().First(o => o.GetAllAssetNames().Any(p => p.ToLower().Contains("plague")));

                        assetBundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;

                        bundle = assetBundle;

                        HasLoadedABundle = true;

                        return true;
                    }

                    error = "Null memStream!";
                }
                else
                {
                    error = "Null Stream!";
                }

                return false;
            }
            catch (Exception ex)
            {
                error = ex.ToString();
                return false;
            }
        }

        /// <summary>
        /// Loads An Asset From The Previously Loaded AssetBundle
        /// </summary>
        /// <param name="str">The Internal Name Of The Asset Inside The AssetBundle</param>
        /// <returns>The Asset You Searched For, Null If No AssetBundle Was Previously Loaded</returns>
        internal T Load<T>(string str) where T : Object
        {
            try
            {
                if (HasLoadedABundle)
                {
                    var Asset = bundle.LoadAsset(str, Il2CppType.Of<T>()).Cast<T>();

                    Asset.hideFlags |= HideFlags.DontUnloadUnusedAsset;

                    return Asset;
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }

            return null;
        }
    }
}
