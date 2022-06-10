using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlagueButtonAPI.Pages;
using Object = UnityEngine.Object;

namespace PlagueButtonAPI.Misc
{
    public class QuickMenuPopup
    {
        public MenuPage Page;

        public static QuickMenuPopup CreatePopup(string Title)
        {
            var instance = new QuickMenuPopup
            {
                Page = MenuPage.CreatePage("TempPopupPage", Title, false, false, Gridify: true)
            };

            instance.Page.page.gameObject.AddComponent<ObjectHandler>().OnDisabled += _ =>
            {
                Object.Destroy(instance.Page.page.gameObject);
            };
        }

        public void Show()
        {
            Page.OpenMenu();
        }

        public void Close()
        {
            Page.CloseMenu();
        }
    }
}
