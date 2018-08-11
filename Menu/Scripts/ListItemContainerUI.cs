using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// ---
using Zeef;
using Zeef.Menu;

namespace Zeef.Menu {
    
    public class ListItemContainerUI : UIElement {

        [SerializeField] private int padding = 5;
        [SerializeField] private Text title;
        [SerializeField] private Transform list;
        [SerializeField] private ListItemUI listItem; //prefab
        
        private List<ListItemUI> listItems;

        public static ListItemContainerUI Initialize(ListItemContainerUI prefab, Transform parent, List<ListItemUIModel> models, string title = "") {
            ListItemContainerUI instance = Instantiate(prefab, parent).GetComponentWithError<ListItemContainerUI>();
            
            instance.Show(models, title);

            return instance;
        }

        public void Show(List<ListItemUIModel> models, string title = "", int? idx = null) {
            this.title.text = title;    
            list.DestroyChildren();
            gameObject.SetActive(true);

            // Create elements
            listItems = new List<ListItemUI>();
            foreach (ListItemUIModel model in models)
                listItems.Add(ListItemUI.Initialize(listItem, list, model));

            // Set position and highlight hovered
            int i = 0;
            foreach (ListItemUI ui in listItems) {

                if (idx == i) ui.Highlight();
                else ui.UnHighlight();
                
                ui.RectTransform.anchoredPosition = 
                    new Vector2(0, (i * (ui.RectTransform.sizeDelta.y + padding)) * -1);

                i++;
            };
        }

        public void HighlightListItem(int idx) {
            foreach (ListItemUI listItem in listItems)
                listItem.UnHighlight();

            listItems[idx].Highlight();
        }

        public void Hide() {
            list.transform.DestroyChildren();
            gameObject.SetActive(false);
        }
    }
}