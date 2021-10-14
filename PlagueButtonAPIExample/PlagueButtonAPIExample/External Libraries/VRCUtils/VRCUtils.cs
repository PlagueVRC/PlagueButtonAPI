using System.Linq;
using UnityEngine;
using VRC.SDKBase;
using VRC.UI.Elements;

internal class VRCUtils
{
    internal static bool IsWorldLoaded => Resources.FindObjectsOfTypeAll<VRC_SceneDescriptor>() != null;

    internal static bool IsQuickMenuOpen
    {
        get
        {
            return Resources.FindObjectsOfTypeAll<QuickMenu>().FirstOrDefault(Menu => Menu != null && Menu.gameObject.active) != null;
        }
    }
}