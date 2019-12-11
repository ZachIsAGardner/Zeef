using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Zeef.Menu
{
    public class VerticalMenuSelectUI : LinearMenuSelect
    { 
        public static VerticalMenuSelectUI Initialize(
            VerticalMenuSelectUI prefab,
            RectTransform parent,
            List<MenuItemUIModel> models,
            bool cancelable = false
        )
        {
            VerticalMenuSelectUI instance = Instantiate(prefab, parent).GetComponentWithError<VerticalMenuSelectUI>();

            instance.menuItems = new List<MenuItemUI>();
            instance.cancelable = cancelable;

            // Create menu items
            instance.container.DestroyChildren();

            int i = 0;
            foreach (MenuItemUIModel model in models)
            {
                MenuItemUI menuItem = MenuItemUI.Initialize(instance.menuItemPrefab, instance.container, model);

                menuItem.RectTransform.anchoredPosition = new Vector2(0, i * -menuItem.RectTransform.sizeDelta.y);
                if (i == 0)
                    menuItem.Highlight();
                else
                    menuItem.UnHighlight();

                instance.menuItems.Add(menuItem);

                i++;
            }

            return instance;
        }

        public override async Task<object> GetSelectionAsync(Func<bool> isCancelled = null)
        {
            int focus = 0;

            while (true)
            {
                int oldFocus = focus;

                if (ControlManager.GetInputPressed(ControlManager.Up) || ControlManager.GetAxisPressed(ControlManager.Vertical, true))
                    focus--;
                if (ControlManager.GetInputPressed(ControlManager.Down) || ControlManager.GetAxisPressed(ControlManager.Vertical, false))
                    focus++;

                if (focus < 0)
                    focus = 0;
                if (focus >= menuItems.Count)
                    focus = menuItems.Count - 1; 

                if (focus != oldFocus)
                {
                    foreach (MenuItemUI item in menuItems)
                        item.UnHighlight();
                    menuItems[focus].Highlight();
                }
                
                if (ControlManager.GetInputPressed(ControlManager.Accept))
                { 
                    Close();
                    return menuItems[focus].Data;
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

        public override void Close()
        {
            Destroy(gameObject);
        }
    }
}