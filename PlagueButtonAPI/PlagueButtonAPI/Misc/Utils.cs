using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PlagueButtonAPI.Main;
using UnhollowerRuntimeLib.XrefScans;
using VRC.UI.Elements.Menus;

namespace PlagueButtonAPI.Misc
{
    public class Utils
    {
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
