using System;
using System.Linq;
using PlagueButtonAPI.Misc;
using PlagueButtonAPI.Pages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;
using Object = UnityEngine.Object;

namespace PlagueButtonAPI.Controls.Grouping
{
    public class CollapsibleButtonGroup
    {
        public TextMeshProUGUI foldoutButtonObjectText;

        public ButtonGroup buttonGroup;

        public GameObject foldoutButtonObject;

        public RectMask2D parentMenuMask;

        private static Sprite arrowDown;

        private static Sprite arrowUp;

        public CollapsibleButtonGroup(Transform parent, string text, bool openByDefault = false)
        {
            if (arrowUp == null || arrowDown == null)
            {
                var icons = Resources.FindObjectsOfTypeAll<ActionMenuDriver>().First().field_Public_MenuIcons_0;

                arrowUp = TexToSprite(icons.arrowUp);
                arrowDown = TexToSprite(icons.arrowDown);
            }

            foldoutButtonObject = Object.Instantiate(ButtonAPI.collapsibleButtonGroupFoldoutButtonBase, parent);
            buttonGroup = new ButtonGroup(parent, "", false, TextAnchor.UpperLeft);

            parentMenuMask = parent.parent.GetComponent<RectMask2D>();

            foldoutButtonObjectText = foldoutButtonObject.GetComponentInChildren<TextMeshProUGUI>(true);
            foldoutButtonObjectText.text = text;

            var ToggleObj = foldoutButtonObject.GetComponentInChildren<Toggle>();

            ToggleObj.onValueChanged = new Toggle.ToggleEvent();
            ToggleObj.onValueChanged.AddListener(new Action<bool>((val) =>
            {
                foldoutButtonObject.transform.Find("Arrow").GetComponent<Image>().sprite = val ? arrowUp : arrowDown;
                buttonGroup?.SetActive(val);
            }));

            foldoutButtonObject.transform.Find("Arrow").GetComponent<Image>().sprite = openByDefault ? arrowUp : arrowDown;
            buttonGroup?.SetActive(openByDefault);
        }

        public CollapsibleButtonGroup(MenuPage parent, string text, bool openByDefault = false) : this(parent.menuContents, text, openByDefault)
        {

        }

        public void SetText(string newText)
        {
            foldoutButtonObjectText.text = newText;
        }

        public void Destroy()
        {
            Object.Destroy(foldoutButtonObjectText.gameObject);
            Object.Destroy(buttonGroup.gameObject);
        }

        public void SetActive(bool state)
        {
            foldoutButtonObject?.SetActive(state);
            buttonGroup?.SetActive(state);
        }

        private static Sprite TexToSprite(Texture2D tex)
        {
            Sprite sprite = Sprite.CreateSprite(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f, 0, 0, new Vector4(), false);

            sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            return sprite;
        }
    }
}