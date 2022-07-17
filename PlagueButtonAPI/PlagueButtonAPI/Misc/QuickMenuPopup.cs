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
        public event Action OnDestroy;

        public static QuickMenuPopup CreatePopup(string Title)
        {
            var instance = new QuickMenuPopup
            {
                Page = MenuPage.CreatePage("TempPopupPage", Title, false, false, Gridify: true)
            };

            // This should only run once the object has been enabled in the first place; since we added this AFTER it was disabled above, it should not run on init.
            instance.Page.page.gameObject.AddComponent<ObjectHandler>().OnDisabled += _ =>
            {
                instance.OnDestroy?.Invoke();
                instance.Close();
                Object.Destroy(instance.Page.page.gameObject);
            };

            return instance;
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
