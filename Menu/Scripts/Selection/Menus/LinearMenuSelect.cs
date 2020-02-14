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

        [HideInInspector] public List<MenuItemUI> menuItems;
        public bool cancelable;

        public static LinearMenuSelect Initialize(
            LinearMenuSelect prefab,
            List<MenuItemUIModel> models,
            bool cancelable = false
        )
        {
            return prefab._Initialize(prefab, models, cancelable);
        }

        protected abstract LinearMenuSelect _Initialize(
            LinearMenuSelect prefab,
            List<MenuItemUIModel> models,
            bool cancelable = false
        );

        public abstract Task<object> GetSelectionAsync(Func<bool> isCancelled = null);

        public abstract void Close();
    }
}
