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
        private bool cancelable;

        public static HorizontalMenuSelectUI Initialize(HorizontalMenuSelectUI prefab, RectTransform parent, List<MenuItemUIModel> models, bool cancelable = false) {
            HorizontalMenuSelectUI instance = Instantiate(prefab, parent).GetComponentWithError<HorizontalMenuSelectUI>();

            instance.menuItems = new List<MenuItemUI>();
            instance.cancelable = cancelable;

            // Create menu items
            instance.container.DestroyChildren();

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

                if (ControlManager.GetInputPressed(ControlManager.Left)) focus--;
                if (ControlManager.GetInputPressed(ControlManager.Right)) focus++;

                if (focus < 0) focus = 0;
                if (focus >= menuItems.Count) focus = menuItems.Count - 1; 

                if (focus != oldFocus) {
                    foreach (MenuItemUI item in menuItems) item.UnHighlight();
                    menuItems[focus].Highlight();
                }
                
                if (ControlManager.GetInputPressed(ControlManager.Accept)) { 
                    Close();
                    return menuItems[focus].Data;
                }
                if (ControlManager.GetInputPressed(ControlManager.Deny) && cancelable) { 
                    Close();
                    return null;
                }

                await new WaitForUpdate();
            }
        }

        public void Close() {
            Destroy(gameObject);
        }
    }
}