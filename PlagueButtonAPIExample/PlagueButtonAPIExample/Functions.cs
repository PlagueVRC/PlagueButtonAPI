using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.SDKBase;

namespace PlagueButtonAPIExample
{
    internal class Functions
    {
        internal static void TogglePortals(bool state)
        {
            var array = Resources.FindObjectsOfTypeAll<PortalInternal>();
            foreach (var i in array)
            {
                if (i == null)
                {
                    continue;
                }

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
