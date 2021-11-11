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
        public readonly TextMeshProUGUI headerText;

        public readonly GameObject gameObject;

        public readonly GameObject headerGameObject;

        public readonly RectMask2D parentMenuMask;

        private readonly bool WasNoText;

        public ButtonGroup(Transform parent, string text, bool NoText = false, TextAnchor ButtonAlignment = TextAnchor.UpperCenter)
        {
            WasNoText = NoText;

            if (!NoText)
            {
                headerGameObject = Object.Instantiate(ButtonAPI.buttonGroupHeaderBase, parent);
                headerText = headerGameObject.GetComponentInChildren<TextMeshProUGUI>(true);
                headerText.text = text;
                headerText.GetComponent<RectTransform>().sizeDelta = new Vector2(915f, 50f);
            }

            gameObject = Object.Instantiate(ButtonAPI.buttonGroupBase, parent);
            gameObject.transform.DestroyChildren();
            gameObject.GetComponent<GridLayoutGroup>().childAlignment = ButtonAlignment;
            parentMenuMask = parent.parent.GetComponent<RectMask2D>();
        }

        public ButtonGroup(MenuPage parent, string text, bool NoText = false, TextAnchor ButtonAlignment = TextAnchor.UpperCenter) : this(parent.menuContents, text, NoText, ButtonAlignment)
        {
        }

        public void SetText(string newText)
        {
            if (!WasNoText)
            {
                headerText.text = newText;
            }
        }

        public void Destroy()
        {
            if (!WasNoText)
            {
                Object.Destroy(headerText.gameObject);
            }

            Object.Destroy(gameObject);
        }

        public void SetActive(bool state)
        {
            if (!WasNoText && headerGameObject != null)
            {
                headerGameObject.SetActive(state);
            }

            gameObject.SetActive(state);
        }
    }
}
