using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zeef.Menu
{
    public class MenuItemUI : SelectableUIElement
    {
        [SerializeField] Text textComponent;
        public object Data { get; set; }

        public static MenuItemUI Initialize(MenuItemUI prefab, RectTransform parent, MenuItemUIModel model)
        {
            MenuItemUI instance = Instantiate(prefab, parent).GetComponentWithError<MenuItemUI>();

            instance.textComponent.text = model.Text;
            if (model.Color != null) instance.textComponent.color = model.Color.Value;
            instance.Data = model.Data;

            return instance;
        }

        public override void Highlight()
        {
            ImageComponent.color = Color.white;
        }
        public override void UnHighlight()
        {
            ImageComponent.color = Color.clear;
        }
    }
}