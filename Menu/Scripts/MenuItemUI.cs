using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zeef.Menu {

    public class MenuItemUIModel {
        public string Text { get; set; }
        public object Data { get; set; }

        public MenuItemUIModel(object data, string text) {
            Data = data;
            Text = text;
        }
    }

    public class MenuItemUI : SelectableUIElement {

        [SerializeField] Text textComponent;
        public object Data { get; set; }

        public static MenuItemUI Initialize(MenuItemUI prefab, RectTransform parent, MenuItemUIModel model) {
            MenuItemUI instance = Instantiate(prefab, parent).GetComponentWithError<MenuItemUI>();

            instance.textComponent.text = model.Text;
            instance.Data = model.Data;

            return instance;
        }

        public override void Highlight() {
            ImageComponent.color = Color.white;
        }
        public override void UnHighlight() {
            ImageComponent.color = Color.clear;
        }
    }
}