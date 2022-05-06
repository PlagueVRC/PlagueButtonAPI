using System.Linq;
using PlagueButtonAPI;
using UnityEngine;
using VRC.SDKBase;
using VRC.UI.Elements;

public class VRCUtils
{
    public static bool IsWorldLoaded => Mod.CurrentSceneIndex == -1;

    public static bool IsQuickMenuOpen => ButtonAPI.QuickMenuObj?.active ?? false;
}