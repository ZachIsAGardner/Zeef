using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Zeef.Menu {

    public class HorizontalMenuSelectUI : MonoBehaviour { 

        [SerializeField] MenuItemUI menuItemPrefab;
        [SerializeField] RectTransform container;

        private List<MenuItemUI> menuItems;

        public static HorizontalMenuSelectUI Initialize(HorizontalMenuSelectUI prefab, RectTransform parent, List<MenuItemUIModel> models) {
            HorizontalMenuSelectUI instance = Instantiate(prefab, parent).GetComponentWithError<HorizontalMenuSelectUI>();

            instance.menuItems = new List<MenuItemUI>();
            instance.container.DestroyChildren();

            // Create menu items
            int i = 0;
            foreach (MenuItemUIModel model in models) {
                MenuItemUI menuItem = MenuItemUI.Initialize(instance.menuItemPrefab, instance.container, model);

                menuItem.RectTransform.anchoredPosition = new Vector2(i * menuItem.RectTransform.sizeDelta.x, 0);
                if (i == 0) menuItem.Highlight();
                else menuItem.UnHighlight();

                instance.menuItems.Add(menuItem);

                i++;
            }

            return instance;
        }

        public async Task<object> GetSelectionAsync() {

            int focus = 0;

            while (true) {
                int oldFocus = focus;

                if (Control.GetInputDown(Control.Left)) focus--;
                if (Control.GetInputDown(Control.Right)) focus++;

                if (focus < 0) focus = 0;
                if (focus > menuItems.Count) focus = menuItems.Count; 

                if (focus != oldFocus) {
                    foreach (MenuItemUI item in menuItems) item.UnHighlight();
                    menuItems[focus].Highlight();
                }
                
                if (Input.GetKeyDown("z")) { 
                    Close();
                    return menuItems[focus].Data;
                }
                if (Control.GetInputDown(Control.Deny)) return null;

                await new WaitForUpdate();
            }
        }

        public void Close() {
            Destroy(gameObject);
        }
    }
}