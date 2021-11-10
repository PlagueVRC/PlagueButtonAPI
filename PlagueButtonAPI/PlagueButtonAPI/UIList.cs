using System.Linq;
using System.Reflection;
using Il2CppSystem.Collections.Generic;
using VRC.Core;

namespace PlagueButtonAPI.Utils
{
	public static class UIList
	{
		private static MethodInfo renderElementMethod;

		public static void RenderElement(this UiVRCList uivrclist, List<ApiAvatar> AvatarList)
		{
			if (!uivrclist.gameObject.activeInHierarchy || !uivrclist.isActiveAndEnabled || uivrclist.isOffScreen || !uivrclist.enabled)
			{
				return;
			}
			if (renderElementMethod == null)
			{
				renderElementMethod = typeof(UiVRCList).GetMethods().FirstOrDefault((MethodInfo a) => a.Name.Contains("Method_Protected_Void_List_1_T_Int32_Boolean")).MakeGenericMethod(typeof(ApiAvatar));
			}
			renderElementMethod.Invoke(uivrclist, new object[4] { AvatarList, 0, true, null });
		}

		public static void RenderElement(this UiVRCList uivrclist, List<string> idList)
		{
			if (!uivrclist.gameObject.activeInHierarchy || !uivrclist.isActiveAndEnabled || uivrclist.isOffScreen || !uivrclist.enabled)
			{
				return;
			}
			if (renderElementMethod == null)
			{
				renderElementMethod = typeof(UiVRCList).GetMethods().FirstOrDefault((MethodInfo a) => a.Name.Contains("Method_Protected_Void_List_1_T_Int32_Boolean")).MakeGenericMethod(typeof(ApiAvatar));
			}
			renderElementMethod.Invoke(uivrclist, new object[4] { idList, 0, true, null });
		}
	}
}
