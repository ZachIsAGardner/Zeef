using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Zeef.Menu
{
    public class MatrixMenuSelectUI : MonoBehaviour, IMenuSelect
    { 
        [SerializeField] MenuItemUI menuItemPrefab;
        [SerializeField] RectTransform container;

        private List<List<MenuItemUI>> menuItems;
        private bool cancelable;

        public static MatrixMenuSelectUI Initialize
            (MatrixMenuSelectUI prefab, RectTransform parent, List<List<MenuItemUIModel>> models, bool cancelable = false)
        {
            MatrixMenuSelectUI instance = Instantiate(prefab, parent).GetComponentWithError<MatrixMenuSelectUI>();

            instance.menuItems = new List<List<MenuItemUI>>();
            instance.cancelable = cancelable;

            // Create menu items
            instance.container.DestroyChildren();

            int rowIdx = 0;
            foreach (List<MenuItemUIModel> row in models)
            {

                int colIdx = 0;
                instance.menuItems.Add(new List<MenuItemUI>());

                foreach (MenuItemUIModel model in row)
                { 
                    MenuItemUI menuItem = MenuItemUI.Initialize(instance.menuItemPrefab, instance.container, model);

                    menuItem.RectTransform.anchoredPosition = new Vector2(
                        colIdx * menuItem.RectTransform.sizeDelta.x, 
                        rowIdx * -menuItem.RectTransform.sizeDelta.y
                    );

                    instance.menuItems.Last().Add(menuItem);

                    colIdx++;
                }

                rowIdx++;
            }

            return instance;
        }

        public async Task<object> GetSelectionAsync(Func<bool> isCancelled = null)
        {
            Coordinates focus = new Coordinates();

            foreach (List<MenuItemUI> row in menuItems) 
                foreach(MenuItemUI item in row)
                    item.UnHighlight();
            menuItems[focus.Row][focus.Col].Highlight();

            while (true)
            {
                Coordinates oldFocus = new Coordinates(focus);

                if (ControlManager.GetInputPressed(ControlManager.Left) || ControlManager.GetAxisPressed(ControlManager.Horizontal, false))
                    focus.Col--;
                if (ControlManager.GetInputPressed(ControlManager.Right) || ControlManager.GetAxisPressed(ControlManager.Horizontal, true))
                    focus.Col++;
                if (ControlManager.GetInputPressed(ControlManager.Up) || ControlManager.GetAxisPressed(ControlManager.Vertical, true))
                    focus.Row--;
                if (ControlManager.GetInputPressed(ControlManager.Down) || ControlManager.GetAxisPressed(ControlManager.Vertical, false))
                    focus.Row++;

                if (focus.Row < 0) focus.Row = 0;
                if (focus.Row >= menuItems.Count) focus.Row = menuItems.Count - 1; 
                if (focus.Col < 0) focus.Col = 0;
                if (focus.Col >= menuItems[focus.Row].Count) focus.Col = menuItems[focus.Row].Count - 1; 

                if (!focus.SameAs(oldFocus))
                {
                    foreach (List<MenuItemUI> row in menuItems) 
                        foreach(MenuItemUI item in row)
                            item.UnHighlight();

                    menuItems[focus.Row][focus.Col].Highlight();
                }
                
                if (ControlManager.GetInputPressed(ControlManager.Accept))
                { 
                    Close();
                    return menuItems[focus.Row][focus.Col].Data;
                }
                if (ControlManager.GetInputPressed(ControlManager.Deny) && cancelable)
                { 
                    Close();
                    return null;
                }

                if (isCancelled != null && isCancelled())
                    return null;

                await new WaitForUpdate();
            }
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}