using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Zeef.Menu {

    public enum ListItemOptionsEnum {
        Vertical,
        Horizontal,
        Matrix
    }

    public class ListItemUIElement : UIElement {

        [SerializeField] private Text textComponent;
        public Text TextComponent { get { return textComponent; } }

        public static ListItemUIElement Initialize(GameObject prefab, Transform parent, int idx, int padding, ListItemOptionsEnum option, string text) {
            // Make copy of prefab and find ListItemUIElement component attached to it
            ListItemUIElement instance = Instantiate(prefab, parent).GetComponent<ListItemUIElement>();
            if (instance == null) throw new Exception("agrument 'prefab' does not have a ListItemUIElement component attached to it.");

            // Fill
            instance.TextComponent.text = text; 
            instance.name = $"{Regex.Replace(text, @"\s+", "")}ListItem";
            
            // Set position and size
            switch (option) {
                case ListItemOptionsEnum.Vertical:
                    return InitializeVertical(instance, parent, idx, padding);
                case ListItemOptionsEnum.Horizontal:
                    return InitializeHorizontal(instance, parent, idx, padding);
                case ListItemOptionsEnum.Matrix:
                    return InitializeMatrix(instance, parent, idx, padding);
                default:
                    throw new Exception("Invalid option");
            }
        }

        private static ListItemUIElement InitializeVertical(ListItemUIElement instance, Transform parent, int idx, int padding) {
            RectTransform parentRectTransform = parent.GetComponent<RectTransform>();

            RectTransform rectTransform = instance.GetComponent<RectTransform>();
            Vector2 start = new Vector2(padding, -padding);
            rectTransform.sizeDelta = new Vector2(parentRectTransform.sizeDelta.x - (padding * 2), rectTransform.sizeDelta.y);
            float spacing = -(idx * (rectTransform.sizeDelta.y + padding));

            rectTransform.anchoredPosition = new Vector2(start.x, start.y + spacing);   

            return instance;
        }

        private static ListItemUIElement InitializeHorizontal(ListItemUIElement instance, Transform parent, int idx, int padding) {
            throw new NotImplementedException();
        }

        private static ListItemUIElement InitializeMatrix(ListItemUIElement instance, Transform parent, int idx, int padding) {
            throw new NotImplementedException();
        }
    }
}