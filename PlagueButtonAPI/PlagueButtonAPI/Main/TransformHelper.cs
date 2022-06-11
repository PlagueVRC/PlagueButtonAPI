using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace PlagueButtonAPI.Main
{
    public class TransformHelper
    {
        internal static void Init()
        {
            var Index = 1;
            try
            {

                QuickMenu = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)");

                Index++;

                MainLaunchpadDashboard = QuickMenu.Find("Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup");

                Index++;

                SelectedUser_Local = QuickMenu.Find("Container/Window/QMParent/Menu_SelectedUser_Local/ScrollRect/Viewport/VerticalLayoutGroup");

                Index++;

                SelectedUser_Remote = QuickMenu.Find("Container/Window/QMParent/Menu_SelectedUser_Remote/ScrollRect/Viewport/VerticalLayoutGroup");

                Index++;

                HerePage = QuickMenu.Find("Container/Window/QMParent/Menu_Here/ScrollRect/Viewport/VerticalLayoutGroup");

                Index++;

                CameraPage = QuickMenu.Find("Container/Window/QMParent/Menu_Camera/Scrollrect/Viewport/VerticalLayoutGroup");

                Index++;

                QuickMenu.Find("Container/Window/QMParent/Menu_SelectedUser_Remote/ScrollRect").GetComponent<ScrollRect>().enabled = true;
                QuickMenu.Find("Container/Window/QMParent/Menu_SelectedUser_Remote/ScrollRect/Scrollbar").gameObject.SetActive(true);
            }
            catch (Exception ex)
            {
                MelonLogger.Error("Index: " + Index + "\n\n" + ex);
            }
        }

        public static Transform QuickMenu = null;
        public static Transform MainLaunchpadDashboard = null;
        public static Transform SelectedUser_Local = null;
        public static Transform SelectedUser_Remote = null;
        public static Transform HerePage = null;
        public static Transform CameraPage = null;
    }
}
