using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Zeef.Menu {

    public class MatrixMenuSelectUI : MonoBehaviour { 

        [SerializeField] MenuItemUI menuItemPrefab;
        [SerializeField] RectTransform container;

        private List<List<MenuItemUI>> menuItems;

        public static MatrixMenuSelectUI Initialize(MatrixMenuSelectUI prefab, RectTransform parent, List<List<MenuItemUIModel>> models) {
            MatrixMenuSelectUI instance = Instantiate(prefab, parent).GetComponentWithError<MatrixMenuSelectUI>();

            instance.menuItems = new List<List<MenuItemUI>>();
            instance.container.DestroyChildren();

            // Create menu items
            int rowIdx = 0;
            foreach (List<MenuItemUIModel> row in models) {

                int colIdx = 0;
                instance.menuItems.Add(new List<MenuItemUI>());

                foreach (MenuItemUIModel model in row) { 
                    MenuItemUI menuItem = MenuItemUI.Initialize(instance.menuItemPrefab, instance.container, model);

                    menuItem.RectTransform.anchoredPosition = new Vector2(
                        colIdx * menuItem.RectTransform.sizeDelta.x, 
                        rowIdx * menuItem.RectTransform.sizeDelta.y
                    );

                    instance.menuItems.Last().Add(menuItem);

                    colIdx++;
                }

                rowIdx++;
            }

            return instance;
        }

        public async Task<object> GetSelectionAsync() {

            Coordinates focus = new Coordinates();

            while (true) {
                Coordinates oldFocus = focus;

                if (Control.GetInputDown(Control.Left)) focus.Col--;
                if (Control.GetInputDown(Control.Right)) focus.Col++;
                if (Control.GetInputDown(Control.Up)) focus.Row--;
                if (Control.GetInputDown(Control.Down)) focus.Row++;

                if (focus.Row < 0) focus.Row = 0;
                if (focus.Row > menuItems.Count) focus.Col = menuItems.Count; 
                if (focus.Col < 0) focus.Col = 0;
                if (focus.Col > menuItems[focus.Row].Count) focus.Col = menuItems[focus.Row].Count; 

                if (focus != oldFocus) {
                    foreach (List<MenuItemUI> row in menuItems) 
                        foreach(MenuItemUI item in row)
                            item.UnHighlight();

                    menuItems[focus.Row][focus.Col].Highlight();
                }
                
                if (Input.GetKeyDown("z")) { 
                    Close();
                    return menuItems[focus.Row][focus.Col].Data;
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