using UnityEngine;
using VRC.SDKBase;

namespace ExampleButtonAPIUsage
{
    internal class Functions
    {
        internal static void TogglePortals(bool state)
        {
            foreach (var i in Resources.FindObjectsOfTypeAll<PortalInternal>())
            {
                if (i != null)
                {
                    i.enabled = state;
                    i.gameObject.SetActive(state);

                    if (!state)
                    {
                        Networking.Destroy(i.gameObject);
                    }
                }
            }
        }
    }
}
