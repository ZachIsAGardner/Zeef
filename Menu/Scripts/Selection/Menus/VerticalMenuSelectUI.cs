using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zeef.GameManagement;

namespace Zeef.Menu
{
    public class VerticalMenuSelectUI : LinearMenuSelect
    { 
        private int focus = 0;
        private bool queueCancel = false;

        public static VerticalMenuSelectUI Initialize(
            VerticalMenuSelectUI prefab,
            List<MenuItemUIModel> models,
            bool cancelable = false
        )
        {
            return prefab._Initialize(prefab, models, cancelable) as VerticalMenuSelectUI;
        }
    
        public override void Execute(List<MenuItemUIModel> models)
        {
            CreateMenuItems(models, this);
        }

        protected override LinearMenuSelect _Initialize(LinearMenuSelect prefab, List<MenuItemUIModel> models, bool cancelable = false)
        {
            VerticalMenuSelectUI instance = GameManager
                .SpawnCanvasElement(prefab.gameObject, 5)
                .GetComponentWithError<VerticalMenuSelectUI>();

            instance.cancelable = cancelable;

            CreateMenuItems(models, instance);

            return instance;
        }

        private void CreateMenuItems(List<MenuItemUIModel> models, VerticalMenuSelectUI instance)
        {
            // Clear current menu items
            instance.container.DestroyChildren();
            instance.menuItems = new List<MenuItemUI>();

            // Create menu items
            int i = 0;
            foreach (MenuItemUIModel model in models)
            {
                MenuItemUI menuItem = MenuItemUI.Initialize(instance.menuItemPrefab, instance.container, model);

                menuItem.RectTransform.anchoredPosition = new Vector2(0, i * -menuItem.RectTransform.sizeDelta.y);
                if (i == focus) menuItem.Highlight();
                else menuItem.UnHighlight();

                instance.menuItems.Add(menuItem);

                i++;
            }
        }

        public override async Task<object> GetSelectionAsync(Func<bool> isCancelled = null)
        {
            return await GetSelectionAsync(isCancelled);
        }

        public async Task<object> GetSelectionAsync(Func<bool> isCancelled = null, int? startFocus = null)
        {       
            focus = startFocus ?? focus;

            foreach (MenuItemUI item in menuItems) item.UnHighlight();
            menuItems[focus].Highlight();
                    
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
                    foreach (MenuItemUI item in menuItems) item.UnHighlight();
                    menuItems[focus].Highlight();
                }

                if (menuItems[focus].ContextAction != null)
                    menuItems[focus].ContextAction(menuItems[focus]);
                
                if (ControlManager.GetInputPressed(ControlManager.Accept))
                { 
                    return menuItems[focus].Data;
                }

                if (ControlManager.GetInputPressed(ControlManager.Deny) && cancelable || queueCancel)
                {
                    return null;
                }

                if (isCancelled != null && isCancelled())
                    return null;

                await new WaitForUpdate();
            }
        }

        public override void Cancel()
        {
            queueCancel = true;
        }

        public override void Close()
        {
            Destroy(gameObject);
        }
    }
}