using System;
using System.Reflection;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace PlagueButtonAPI.Misc
{
	internal class ComponentManager : MelonLoaderEvents
	{
		public override void OnApplicationStart()
		{
			Type[] types = Assembly.GetExecutingAssembly().GetTypes();
			for (int i = 0; i < types.Length; i++)
			{
				RegisterTypeRecursive(types[i]);
			}
		}

		private static void RegisterTypeRecursive(Type t)
		{
			if (!(t == null) && t.IsSubclassOf(typeof(MonoBehaviour)))
			{
				ClassInjector.RegisterTypeInIl2Cpp(t, false);
			}
		}
	}
}
