﻿using System.Linq;
using UnityEngine;
using VRC.SDKBase;
using VRC.UI.Elements;

public class VRCUtils
{
    public static bool IsWorldLoaded => Resources.FindObjectsOfTypeAll<VRC_SceneDescriptor>() != null;

    public static bool IsQuickMenuOpen
    {
        get
        {
            return Resources.FindObjectsOfTypeAll<QuickMenu>().FirstOrDefault(Menu => Menu != null && Menu.gameObject.active) != null;
        }
    }
}