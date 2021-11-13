using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using PlagueButtonAPI.Controls.Grouping;
using PlagueButtonAPI.Misc;
using PlagueButtonAPI.Pages;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;

namespace PlagueButtonAPI.Controls
{
    public class Label
    {
        public readonly SimpleSingleButton LabelButton;

        public Label(Transform parent, string text, string tooltip, Action onClick = null)
        {
            LabelButton = new SimpleSingleButton(parent, text, tooltip, onClick);

            LabelButton.gameObject.transform.Find("Background").GetOrAddComponent<Image>().color = new Color(0f, 0f, 0f, 0f);

            if (onClick == null)
            {
                LabelButton.gameObject.GetOrAddComponent<Button>().enabled = false;

                var Handler = LabelButton.gameObject.AddComponent<ObjectHandler>();

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
            }
        }

        public Label(MenuPage pge, string text, string tooltip, Action onClick = null)
            : this(pge.menuContents, text, tooltip, onClick)
        {
        }

        public Label(ButtonGroup grp, string text, string tooltip, Action onClick = null)
            : this(grp.gameObject.transform, text, tooltip, onClick)
        {
        }

        public Label(CollapsibleButtonGroup grp, string text, string tooltip, Action onClick = null)
            : this(grp.buttonGroup, text, tooltip, onClick)
        {
        }
    }
}
