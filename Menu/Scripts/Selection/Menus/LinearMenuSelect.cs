using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zeef.GameManagement;

namespace Zeef.Menu
{
    public abstract class LinearMenuSelect : MonoBehaviour, IMenuSelect
    {
        [SerializeField] public MenuItemUI menuItemPrefab;
        [SerializeField] public RectTransform container;

        public List<MenuItemUI> menuItems;
        public bool cancelable;

        public static LinearMenuSelect Initialize(
            LinearMenuSelect prefab,
            RectTransform parent,
            List<MenuItemUIModel> models,
            bool cancelable = false
        )
        {
            LinearMenuSelect instance = GameManager
                .Spawn(prefab.gameObject, parent.gameObject)
                .GetComponentWithError<LinearMenuSelect>();

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

        public abstract Task<object> GetSelectionAsync(Func<bool> isCancelled = null);

        public abstract void Close();
    }
}
