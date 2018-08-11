using UnityEngine;
using UnityEngine.UI;
// ---
using Zeef;
using Zeef.Menu;

namespace Zeef.Menu {
    
    public class ListItemUIModel {
        public string Text { get; set; }
        public int? Count { get; set; }
        public Color TextColor { get; set; }

        public ListItemUIModel(string text, Color textColor) {
            Text = text;
            Count = null;
            TextColor = textColor;
        }
        public ListItemUIModel(string text, Color textColor, int? count) {
            Text = text;
            Count = count;
            TextColor = textColor;
        }
    }

    public class ListItemUI : UIElement {

        public Text TextComponent;
        public GameObject CountContainer;
        public Text CountComponent;

        public static ListItemUI Initialize(ListItemUI prefab, Transform parent, ListItemUIModel model) {
            ListItemUI instance = Instantiate(prefab, parent).GetComponentWithError<ListItemUI>();
            
            instance.TextComponent.text = model.Text;
            instance.TextComponent.color = model.TextColor;

            instance.CountComponent.text = model.Count.ToString();
            instance.CountComponent.color = model.TextColor;
            instance.CountContainer.SetActive(model.Count != null);

            // Image.sprite = sprite;

            return instance;
        }
    }
}