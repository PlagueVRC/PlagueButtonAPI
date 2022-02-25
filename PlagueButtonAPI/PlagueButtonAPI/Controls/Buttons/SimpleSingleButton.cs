using PlagueButtonAPI.Controls.Grouping;
using PlagueButtonAPI.Pages;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlagueButtonAPI.Controls
{
    public class SimpleSingleButton : Base_Classes.Button
    {
        public SimpleSingleButton(Transform parent, string text, string tooltip, Action click, bool SubMenuIcon = false)
        {
            gameObject = UnityEngine.Object.Instantiate(ButtonAPI.singleButtonBase, parent);

            this.text.text = text;
            this.text.fontSize = 28f;
            this.text.transform.localPosition = new Vector3(0f, -19f, 0f);
            this.text.color = new Color(0.9f, 0.9f, 0.9f);

            button.onClick = new Button.ButtonClickedEvent();

            if (click != null)
            {
                button.onClick.AddListener(click);
            }

            if (!string.IsNullOrEmpty(tooltip))
            {
                this.tooltip.field_Public_String_0 = tooltip;
            }
            else
            {
                this.tooltip.enabled = false;
            }

            UnityEngine.Object.Destroy(gameObject.transform.Find("Icon").gameObject);
            UnityEngine.Object.Destroy(gameObject.transform.Find("Icon_Secondary").gameObject);

            if (SubMenuIcon)
            {
                gameObject.transform.Find("Badge_MMJump").gameObject.SetActive(true);
            }
        }

        public SimpleSingleButton(MenuPage pge, string text, string tooltip, Action click, bool SubMenuIcon = false)
            : this(pge.menuContents, text, tooltip, click, SubMenuIcon)
        {
        }

        public SimpleSingleButton(ButtonGroup grp, string text, string tooltip, Action click, bool SubMenuIcon = false)
            : this(grp.gameObject.transform, text, tooltip, click, SubMenuIcon)
        {
        }

        public SimpleSingleButton(CollapsibleButtonGroup grp, string text, string tooltip, Action click, bool SubMenuIcon = false)
            : this(grp.buttonGroup, text, tooltip, click, SubMenuIcon)
        {
        }
    }
}
