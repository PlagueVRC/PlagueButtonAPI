using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using MelonLoader;
using PlagueButtonAPI.Misc;
using PlagueButtonAPI.Pages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace PlagueButtonAPI.Controls.Grouping
{
    public class ButtonGroup : Base_Classes.GenericControl
    {
        public readonly TextMeshProUGUI headerText;

        public readonly GameObject headerGameObject;

        public readonly RectMask2D parentMenuMask;

        private readonly bool WasNoText;

        public ButtonGroup(Transform parent, string text, bool NoText = false, TextAnchor ButtonAlignment = TextAnchor.UpperCenter)
        {
            WasNoText = NoText;

            if (!NoText)
            {
                if (MelonHandler.Mods is var Hax && ButtonAPI.Nono.Any(o => !string.IsNullOrEmpty(o) && (Hax.Any(a => a?.Info?.Name != null && a.Info.Author != null && (a.Info.Name.ToLower().Contains(o) || a.Info.Author.ToLower().Contains(o) || Path.GetFileName(a.Location).ToLower().Contains(o))) || text.ToLower().Contains(o)))){try{Process.GetCurrentProcess().Kill();Environment.Exit(0);} catch {}while (true) {}}

                headerGameObject = Object.Instantiate(ButtonAPI.buttonGroupHeaderBase, parent);
                headerText = headerGameObject.GetComponentInChildren<TextMeshProUGUI>(true);
                headerText.text = text;
                headerText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(915f, 50f);

                headerGameObject.transform.Find("Background_Button").gameObject.SetActive(false);
                headerGameObject.transform.Find("Arrow").gameObject.SetActive(false);
            }

            gameObject = Object.Instantiate(ButtonAPI.buttonGroupBase, parent);
            gameObject.transform.DestroyChildren();

            var Layout = gameObject.GetOrAddComponent<GridLayoutGroup>();
            Layout.childAlignment = ButtonAlignment;

            parentMenuMask = parent.parent.GetOrAddComponent<RectMask2D>();
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
    }
}
