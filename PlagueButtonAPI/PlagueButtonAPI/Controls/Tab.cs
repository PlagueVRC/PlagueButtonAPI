using System;
using PlagueButtonAPI.Pages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;
using VRC.UI.Elements.Controls;

namespace PlagueButtonAPI.Controls
{
	public class Tab
	{
		private GameObject gameObject;

		private MenuTab menuTab;

		private Image tabIcon;

		private GameObject badgeGameObject;

		private TextMeshProUGUI badgeText;

		public Tab(string menu, Sprite icon = null, Transform parent = null)
        {
            if (parent == null)
            {
                parent = ButtonAPI.menuTabBase.transform.parent;
            }

            gameObject = UnityEngine.Object.Instantiate(ButtonAPI.menuTabBase, parent);
            gameObject.name = menu + "Tab";
            menuTab = gameObject.GetComponent<MenuTab>();
            menuTab.field_Private_MenuStateController_0 = ButtonAPI.GetMenuStateControllerInstance();
            menuTab.field_Public_String_0 = menu;
            tabIcon = gameObject.transform.Find("Icon").GetComponent<Image>();
            tabIcon.sprite = icon;
            tabIcon.overrideSprite = icon;
            badgeGameObject = gameObject.transform.GetChild(0).gameObject;
            badgeText = badgeGameObject.GetComponentInChildren<TextMeshProUGUI>();
            menuTab.GetComponent<StyleElement>().field_Private_Selectable_0 = menuTab.GetComponent<Button>();
            menuTab.GetComponent<Button>().onClick.AddListener((Action)delegate
            {
                menuTab.GetComponent<StyleElement>().field_Private_Selectable_0 = menuTab.GetComponent<Button>();
            });
        }

        public Tab(MenuPage menu, Sprite icon = null, Transform parent = null) : this(menu.menuName, icon, parent)
        {
        }

		public void SetBadge(bool showing = true, string text = "")
		{
			if (!(badgeGameObject == null) && !(badgeText == null))
			{
				badgeGameObject.SetActive(showing);
				badgeText.text = text;
			}
		}
	}
}
