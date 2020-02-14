using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zeef.GameManagement;

namespace Zeef.Menu
{
    public class HorizontalMenuSelectUI : LinearMenuSelect
    {
        public static HorizontalMenuSelectUI Initialize(
            HorizontalMenuSelectUI prefab,
            List<MenuItemUIModel> models,
            bool cancelable = false
        )
        {
            return prefab._Initialize(prefab, models, cancelable) as HorizontalMenuSelectUI;
        }

        protected override LinearMenuSelect _Initialize(LinearMenuSelect prefab, List<MenuItemUIModel> models, bool cancelable = false)
        {
            HorizontalMenuSelectUI instance = GameManager
                .SpawnCanvasElement(prefab.gameObject, 5)
                .GetComponentWithError<HorizontalMenuSelectUI>();

            instance.menuItems = new List<MenuItemUI>();
            instance.cancelable = cancelable;

            // Create menu items
            instance.container.DestroyChildren();

            int i = 0;
            foreach (MenuItemUIModel model in models)
            {
                MenuItemUI menuItem = MenuItemUI.Initialize(instance.menuItemPrefab, instance.container, model);

                menuItem.RectTransform.anchoredPosition = new Vector2(i * menuItem.RectTransform.sizeDelta.x, 0);
                if (i == 0) menuItem.Highlight();
                else menuItem.UnHighlight();

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

                if (ControlManager.GetInputPressed(ControlManager.Left) || ControlManager.GetAxisPressed(ControlManager.Horizontal, false))
                    focus--;

                if (ControlManager.GetInputPressed(ControlManager.Right) || ControlManager.GetAxisPressed(ControlManager.Horizontal, true))
                    focus++;

                if (focus < 0) focus = 0;
                if (focus >= menuItems.Count) focus = menuItems.Count - 1; 

                if (focus != oldFocus)
                {
                    foreach (MenuItemUI item in menuItems) item.UnHighlight();
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