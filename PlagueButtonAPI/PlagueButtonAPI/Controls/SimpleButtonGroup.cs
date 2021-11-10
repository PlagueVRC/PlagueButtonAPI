using System;
using PlagueButtonAPI.Misc;
using PlagueButtonAPI.Pages;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace PlagueButtonAPI.Controls
{
	public class SimpleButtonGroup
	{
		public GameObject gameObject;

		public RectMask2D parentMenuMask;

		public SimpleButtonGroup(Transform parent)
		{
            gameObject = Object.Instantiate(ButtonAPI.buttonGroupBase, parent);
            gameObject.transform.DestroyChildren();
            parentMenuMask = parent.parent.GetComponent<RectMask2D>();
        }

		public SimpleButtonGroup(MenuPage pge)
			: this(pge.menuContents)
		{
		}

		public void Destroy()
		{
			Object.Destroy(gameObject);
		}

		public void SetActive(bool state)
		{
			gameObject.SetActive(state);
		}
	}
}
