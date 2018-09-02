using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Zeef.Menu {

    public class VerticalMenuSelectUI : MonoBehaviour { 

        [SerializeField] MenuItemUI menuItemPrefab;
        [SerializeField] RectTransform container;

        private List<MenuItemUI> menuItems;
        private bool cancelable;

        public static VerticalMenuSelectUI Initialize(VerticalMenuSelectUI prefab, RectTransform parent, List<MenuItemUIModel> models, bool cancelable = false) {
            VerticalMenuSelectUI instance = Instantiate(prefab, parent).GetComponentWithError<VerticalMenuSelectUI>();

            instance.menuItems = new List<MenuItemUI>();
            instance.cancelable = cancelable;

            // Create menu items
            instance.container.DestroyChildren();

            int i = 0;
            foreach (MenuItemUIModel model in models) {
                MenuItemUI menuItem = MenuItemUI.Initialize(instance.menuItemPrefab, instance.container, model);

                menuItem.RectTransform.anchoredPosition = new Vector2(0, i * -menuItem.RectTransform.sizeDelta.y);
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

                if (ControlManager.GetInputDown(ControlManager.Up)) focus--;
                if (ControlManager.GetInputDown(ControlManager.Down)) focus++;

                if (focus < 0) focus = 0;
                if (focus >= menuItems.Count) focus = menuItems.Count - 1; 

                if (focus != oldFocus) {
                    foreach (MenuItemUI item in menuItems) item.UnHighlight();
                    menuItems[focus].Highlight();
                }
                
                if (ControlManager.GetInputDown(ControlManager.Accept)) { 
                    Close();
                    return menuItems[focus].Data;
                }
                if (ControlManager.GetInputDown(ControlManager.Deny) && cancelable) {
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