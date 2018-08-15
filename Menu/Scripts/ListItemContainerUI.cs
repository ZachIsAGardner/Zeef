using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// ---
using Zeef;
using Zeef.Menu;

namespace Zeef.Menu {

    // public class PageUI : MonoBehaviour {

    //     public List<ListItemUI> ListItems { get; private set; }

    //     public static PageUI Initialize(Transform parent, int index, List<ListItemUIModel> models, ListItemUI listItemPrefab, int padding, int? selected = null) {
    //         PageUI instance = Instantiate(new GameObject(), parent).AddComponent<PageUI>();
    //         instance.name = $"Page {index}";

    //         RectTransform rectTransform = instance.gameObject.AddComponent<RectTransform>();
    //         rectTransform.anchorMin = new Vector2(0,0);
    //         rectTransform.anchorMax = new Vector2(1,1);
    //         rectTransform.offsetMin = new Vector2(0,0);
    //         rectTransform.offsetMax = new Vector2(0,0);

    //         // Generate list items
    //         instance.ListItems = new List<ListItemUI>();

    //         // Create elements
    //         foreach (ListItemUIModel model in models)
    //             instance.ListItems.Add(ListItemUI.Initialize(listItemPrefab, instance.transform, model));

    //         // Position elements
    //         int i = 0;
    //         foreach (ListItemUI listItem in instance.ListItems) {
    //             float y = ((i * listItem.RectTransform.sizeDelta.y) + padding) * -1;
    //             listItem.RectTransform.anchoredPosition = new Vector2(0, y);
    //             i++;
    //         }

    //         // Highlight selected element
    //         instance.HighlightListItem((selected.HasValue) ? selected.Value : 0);

    //         return instance;
    //     }

    //     public void HighlightListItem(int selected) {
    //         foreach (ListItemUI listItem in ListItems)
    //             listItem.UnHighlight();

    //         ListItems[selected].Highlight();
    //     }
    // }

    public class ListItemContainerUI : UIElement {

        [SerializeField] private int padding = 5;
        [SerializeField] private Text title;
        [SerializeField] private Transform list;
        [SerializeField] private ListItemUI listItemPrefab;
        
        public List<PageUI> Pages { get; private set; }

        public static ListItemContainerUI Initialize(ListItemContainerUI prefab, Transform parent, List<ListItemUIModel> models, int maxLength, string title = "") {
            ListItemContainerUI instance = Instantiate(prefab, parent).GetComponentWithError<ListItemContainerUI>();
            
            instance.Show(models, maxLength, title);

            return instance;
        }

        public void Show(List<ListItemUIModel> models, int maxLength, string title = "", Coordinates selected = null) {
            this.title.text = title;    
            list.DestroyChildren();
            gameObject.SetActive(true);

            Pages = new List<PageUI>();

            // while (Pages.Count * maxLength < models.Count) {
            //     // Get models for just this page
            //     List<ListItemUIModel> pageModels = models.TryGetRange((Pages.Count) * maxLength, maxLength);

            //     // Create new page
            //     PageUI page = PageUI.Initialize(
            //         parent: list, 
            //         index: Pages.Count, 
            //         models: pageModels, 
            //         listItemPrefab: listItemPrefab, 
            //         padding: padding
            //     );

            //     Pages.Add(page);
            // }
        }

        public void HighlightListItem(Coordinates selected) {
            foreach (PageUI page in Pages) page.gameObject.SetActive(false);

            PageUI currentPage = Pages[selected.Col];
            currentPage.gameObject.SetActive(true);
            // currentPage.HighlightListItem(selected.Row);
        }

        public void Hide() {
            list.transform.DestroyChildren();
            gameObject.SetActive(false);
        }
    }
}