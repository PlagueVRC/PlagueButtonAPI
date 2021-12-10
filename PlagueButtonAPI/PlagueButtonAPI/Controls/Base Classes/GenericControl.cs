using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PlagueButtonAPI.Controls.Base_Classes
{
    public class GenericControl
    {
        public GameObject gameObject;

        public bool IsNull => gameObject == null;

        public bool active => gameObject?.active ?? false;

        public bool activeSelf => gameObject?.activeSelf ?? false;

        public Transform transform => gameObject?.transform;

        public void SetActive(bool state)
        {
            gameObject?.SetActive(state);
        }
    }
}