using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PlagueButtonAPI.Main
{
    public class TransformHelper
    {
        internal static void Init()
        {
            QuickMenu = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)").transform;

            MainLaunchpadDashboard = QuickMenu.Find("Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup");

            SelectedUser_Local = QuickMenu.Find("Container/Window/QMParent/Menu_SelectedUser_Local/ScrollRect/Viewport/VerticalLayoutGroup");
            SelectedUser_Remote = QuickMenu.Find("Container/Window/QMParent/Menu_SelectedUser_Remote/ScrollRect/Viewport/VerticalLayoutGroup");

            HerePage = QuickMenu.Find("Container/Window/QMParent/Menu_Here/ScrollRect/Viewport/VerticalLayoutGroup");
            CameraPage = QuickMenu.Find("Container/Window/QMParent/Menu_Camera/ScrollRect/Viewport/VerticalLayoutGroup");
            AudioPage = QuickMenu.Find("Container/Window/QMParent/Menu_AudioSettings/ScrollRect/Viewport/VerticalLayoutGroup");
            SettingsPage = QuickMenu.Find("Container/Window/QMParent/Menu_Settings/ScrollRect/Viewport/VerticalLayoutGroup");
        }

        internal static Transform QuickMenu = null;
        public static Transform MainLaunchpadDashboard = null;
        public static Transform SelectedUser_Local = null;
        public static Transform SelectedUser_Remote = null;
        public static Transform HerePage = null;
        public static Transform CameraPage = null;
        public static Transform AudioPage = null;
        public static Transform SettingsPage = null;
    }
}
