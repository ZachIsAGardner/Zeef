using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zeef.GameManagement;
using Zeef.Sound;

namespace Zeef.Menu
{
    public class VerticalMenuSelectUI : LinearMenuSelect
    { 
        private int focus = 0;
        private bool queueCancel = false;

        public static VerticalMenuSelectUI Initialize(
            VerticalMenuSelectUI prefab,
            List<MenuItemUIModel> models,
            bool cancelable = false,
            string moveSound = null,
            string selectSound = null,
            string cancelSound = null
        )
        {
            return prefab._Initialize(prefab, models, cancelable, moveSound, selectSound, cancelSound) as VerticalMenuSelectUI;
        }
    
        public override void Execute(List<MenuItemUIModel> models)
        {
            CreateMenuItems(models, this);
        }

        protected override LinearMenuSelect _Initialize(
            LinearMenuSelect prefab, 
            List<MenuItemUIModel> models, 
            bool cancelable = false,
            string moveSound = null,
            string selectSound = null,
            string cancelSound = null
        )
        {
            VerticalMenuSelectUI instance = SpawnManager
                .SpawnCanvasElement(prefab.gameObject, 5)
                .GetComponentWithError<VerticalMenuSelectUI>();

            instance.cancelable = cancelable;

            instance.moveSound = moveSound;
            instance.selectSound = selectSound;
            instance.cancelSound = cancelSound;

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

            foreach (MenuItemUI item in menuItems) 
                item.UnHighlight();

            menuItems[focus].Highlight();
                    
            while (true)
            {
                int oldFocus = focus;

                // Handle player input
                if (ControlManager.GetInputPressed(ControlManager.Up) || ControlManager.GetAxisPressed(ControlManager.Vertical, true))
                    focus--;
                if (ControlManager.GetInputPressed(ControlManager.Down) || ControlManager.GetAxisPressed(ControlManager.Vertical, false))
                    focus++;

                // Cap focus
                if (focus < 0)
                    focus = 0;
                if (focus >= menuItems.Count)
                    focus = menuItems.Count - 1; 

                // Handle moved selection
                if (focus != oldFocus)
                {
                    foreach (MenuItemUI item in menuItems) 
                        item.UnHighlight();
                        
                    menuItems[focus].Highlight();

                    if (!string.IsNullOrWhiteSpace(moveSound))
                        AudioManager.PlaySoundEffect(moveSound);
                    
                }

                // Execute context action
                if (menuItems[focus].ContextAction != null)
                    menuItems[focus].ContextAction(menuItems[focus]);
                
                // Return selected option
                if (ControlManager.GetInputPressed(ControlManager.Accept))
                {
                    // Play select sound
                    if (!string.IsNullOrWhiteSpace(menuItems[focus].SelectSound))
                        AudioManager.PlaySoundEffect(menuItems[focus].SelectSound);
                    else if (!string.IsNullOrWhiteSpace(selectSound))
                        AudioManager.PlaySoundEffect(selectSound);

                    return menuItems[focus].Data;
                }
                
                // Cancel
                if (
                    (ControlManager.GetInputPressed(ControlManager.Deny) && cancelable || queueCancel) 
                    || (isCancelled != null && isCancelled())
                )
                {
                    if (!string.IsNullOrWhiteSpace(cancelSound))
                        AudioManager.PlaySoundEffect(cancelSound);

                    return null;
                }
                
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