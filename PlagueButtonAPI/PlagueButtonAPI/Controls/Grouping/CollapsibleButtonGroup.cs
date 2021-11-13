using System;
using System.Collections;
using System.Linq;
using MelonLoader;
using PlagueButtonAPI.Misc;
using PlagueButtonAPI.Pages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;
using VRC.UI.Elements;
using Object = UnityEngine.Object;

namespace PlagueButtonAPI.Controls.Grouping
{
    public class CollapsibleButtonGroup
    {
        public readonly TextMeshProUGUI foldoutButtonObjectText;

        public readonly ButtonGroup buttonGroup;

        public readonly SingleButton mainButtonObject;

        public readonly RectMask2D parentMenuMask;

        private static Sprite arrowDown;

        private static Sprite arrowUp;

        private bool IsOpen;

        public CollapsibleButtonGroup(Transform parent, string text, string tooltip, bool openByDefault = false)
        {
            if (arrowUp == null || arrowDown == null)
            {
                var icons = Resources.FindObjectsOfTypeAll<ActionMenuDriver>().First().field_Public_MenuIcons_0;

                arrowUp = TexToSprite(icons.arrowUp);
                arrowDown = TexToSprite(icons.arrowDown);
            }

            IsOpen = openByDefault;

            mainButtonObject = new SingleButton(parent, text, tooltip, () =>
            {
                IsOpen = !IsOpen;

                mainButtonObject.SetIcon(IsOpen ? arrowUp : arrowDown);
                buttonGroup?.SetActive(IsOpen);
            }, false, openByDefault ? arrowUp : arrowDown);

            mainButtonObject.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1024f, 100f);

            var TextObj = mainButtonObject.gameObject.transform.Find("Text_H4");
            TextObj.GetOrAddComponent<TextMeshProUGUI>().fontSize = 50;
            TextObj.GetComponent<RectTransform>().sizeDelta = new Vector2(500f, 48f);
            TextObj.localPosition = new Vector3(-350f, -23f, 0f);

            mainButtonObject.gameObject.transform.Find("Icon").localPosition = new Vector3(450f, 33f, 0f);
            mainButtonObject.gameObject.transform.localScale = new Vector3(0.88f, 1f, 1f);

            var Handler = mainButtonObject.gameObject.AddComponent<ObjectHandler>();

            Handler.OnEnabled += (obj) =>
            {
                var style = obj.GetOrAddComponent<StyleElement>();

                if (style != null)
                {
                    MelonCoroutines.Start(DelayedAction());

                    IEnumerator DelayedAction()
                    {
                        yield return new WaitForSeconds(0.5f);

                        style.enabled = false;

                        yield break;
                    }
                }
            };

            buttonGroup = new ButtonGroup(parent, "", false, TextAnchor.UpperLeft);

            parentMenuMask = parent.parent.GetOrAddComponent<RectMask2D>();

            mainButtonObject.SetIcon(IsOpen ? arrowUp : arrowDown);
            buttonGroup?.SetActive(IsOpen);
        }

        public CollapsibleButtonGroup(MenuPage parent, string text, string tooltip, bool openByDefault = false) : this(parent.menuContents, text, tooltip, openByDefault)
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
            mainButtonObject?.SetActive(state);
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