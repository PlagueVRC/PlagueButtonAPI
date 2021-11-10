using System;
using PlagueButtonAPI.Misc;
using PlagueButtonAPI.Pages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace PlagueButtonAPI.Controls.Grouping
{
	public class ButtonGroup
	{
		private TextMeshProUGUI headerText;

		public GameObject gameObject;

		private GameObject headerGameObject;

		public RectMask2D parentMenuMask;

		public ButtonGroup(Transform parent, string text)
        {
            headerGameObject = Object.Instantiate(ButtonAPI.buttonGroupHeaderBase, parent);
            headerText = headerGameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            headerText.text = text;
            headerText.GetComponent<RectTransform>().sizeDelta = new Vector2(915f, 50f);
            gameObject = Object.Instantiate(ButtonAPI.buttonGroupBase, parent);
            gameObject.transform.DestroyChildren();
            parentMenuMask = parent.parent.GetComponent<RectMask2D>();
        }

        public ButtonGroup(MenuPage parent, string text) : this(parent.menuContents, text)
        {

        }

		public void SetText(string newText)
		{
			headerText.text = newText;
		}

		public void Destroy()
		{
			Object.Destroy(headerText.gameObject);
			Object.Destroy(gameObject);
		}

		public void SetActive(bool state)
		{
			if (headerGameObject != null)
			{
				headerGameObject.SetActive(state);
			}
			gameObject.SetActive(state);
		}
	}
}
