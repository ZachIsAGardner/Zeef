using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Menu
{    
    // Associated with PagedVerticalMenuSelectUI
    public class PageUI : MonoBehaviour
    {    
        public List<MenuItemUI> MenuItems { get; private set; }

        public static PageUI Initialize(GameObject parent, List<MenuItemUIModel> models, MenuItemUI menuItemPrefab, int pageIdx, int? selected = null)
        {
            PageUI instance = new GameObject($"Page {pageIdx}").AddComponent<PageUI>();
            instance.gameObject.transform.SetParent(parent.transform);

            RectTransform rectTransform = instance.gameObject.AddComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1,1,1);
            rectTransform.anchorMin = new Vector2(0,0);
            rectTransform.anchorMax = new Vector2(1,1);
            rectTransform.offsetMin = new Vector2(0,0);
            rectTransform.offsetMax = new Vector2(0,0);

            // Generate list items
            instance.MenuItems = new List<MenuItemUI>();

            // Create elements
            foreach (MenuItemUIModel model in models)
                instance.MenuItems.Add(MenuItemUI.Initialize(
                    menuItemPrefab,
                    instance.GetComponent<RectTransform>(),
                    model
                ));

            // Position elements
            int i = 0;
            foreach (MenuItemUI listItem in instance.MenuItems)
            {
                listItem.RectTransform.anchoredPosition = new Vector2
                    (0, i * -listItem.RectTransform.sizeDelta.y);
                i++;
            }

            // Highlight selected element
            instance.HighlightItem((selected.HasValue) ? selected.Value : 0);

            return instance;
        }

        public void HighlightItem(int idx)
        {
            foreach (MenuItemUI item in MenuItems) item.UnHighlight(); 
            MenuItems[idx].Highlight();
        }
    } 
}