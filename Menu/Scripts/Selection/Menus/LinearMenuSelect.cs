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

        protected string moveSound;
        protected string selectSound;
        protected string cancelSound;

        public bool cancelable;

        public static LinearMenuSelect Initialize(
            LinearMenuSelect prefab,
            List<MenuItemUIModel> models,
            bool cancelable = false,
            string moveSound = null,
            string selectSound = null,
            string cancelSound = null
        )
        {
            return prefab._Initialize(
                prefab, models, cancelable, moveSound, selectSound, cancelSound
            );
        }

        protected abstract LinearMenuSelect _Initialize(
            LinearMenuSelect prefab,
            List<MenuItemUIModel> models,
            bool cancelable = false,
            string moveSound = null,
            string selectSound = null,
            string cancelSound = null
        );

        public abstract void Execute(
            List<MenuItemUIModel> models
        );

        public abstract Task<object> GetSelectionAsync(Func<bool> isCancelled = null);

        public abstract void Cancel();

        public abstract void Close();
    }
}
