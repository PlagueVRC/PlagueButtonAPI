using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using PlagueButtonAPI.Misc;

[assembly: MelonInfo(typeof(PlagueButtonAPI.Mod), "PlagueButtonAPI", "1.0", "Plague", "https://github.com/PlagueVRC/PlagueButtonAPI/tree/beta-ui")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace PlagueButtonAPI
{
    internal class Mod : MelonMod
    {
        private static readonly IEnumerable<MelonLoaderEvents> eventListeners = from type in typeof(Mod).Assembly.GetTypes()
            where type.IsSubclassOf(typeof(MelonLoaderEvents))
            orderby ((PriorityAttribute)Attribute.GetCustomAttribute(type, typeof(PriorityAttribute)))?.priority ?? 0
            select (MelonLoaderEvents)Activator.CreateInstance(type);

        public override void OnApplicationStart()
        {
            foreach (var eventListener in eventListeners)
            {
                try
                {
                    eventListener.OnApplicationStart();
                }
                catch (Exception ex)
                {
                    MelonLogger.Error("Encountered an exception while running OnApplicationStart of \"" + eventListener.GetType().FullName + "\":\n" + ex);
                }
            }

            MelonCoroutines.Start(WaitForUIInit());
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            foreach (var eventListener in eventListeners)
            {
                try
                {
                    eventListener.OnSceneWasInitialized(buildIndex, sceneName);
                }
                catch (Exception ex)
                {
                    MelonLogger.Error("Encountered an exception while running OnSceneInit of \"" + eventListener.GetType().FullName + "\":\n" + ex);
                }
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            foreach (var eventListener in eventListeners)
            {
                try
                {
                    eventListener.OnSceneWasLoaded(buildIndex, sceneName);
                }
                catch (Exception ex)
                {
                    MelonLogger.Error("Encountered an exception while running OnSceneLoad of \"" + eventListener.GetType().FullName + "\":\n" + ex);
                }
            }
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            foreach (var eventListener in eventListeners)
            {
                try
                {
                    eventListener.OnSceneWasUnloaded(buildIndex, sceneName);
                }
                catch (Exception ex)
                {
                    MelonLogger.Error("Encountered an exception while running OnSceneUnload of \"" + eventListener.GetType().FullName + "\":\n" + ex);
                }
            }
        }

        private IEnumerator WaitForUIInit()
        {
            while (VRCUiManager.field_Private_Static_VRCUiManager_0 == null)
            {
                yield return null;
            }

            foreach (var eventListener in eventListeners)
            {
                try
                {
                    eventListener.OnUiManagerInit();
                }
                catch (Exception ex)
                {
                    MelonLogger.Error("Encountered an exception while running UiManagerInit of \"" + eventListener.GetType().FullName + "\":\n" + ex);
                }
            }

            yield break;
        }
    }
}
