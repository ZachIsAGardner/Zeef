using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Zeef.Menu {

    public class PagedVerticalMenuSelectUI : MonoBehaviour { 

        [SerializeField] MenuItemUI menuItemPrefab;
        [SerializeField] GameObject containerComponent;
        [SerializeField] Text pageCountComponent;

        private List<PageUI> pages;
        private bool cancelable;

        public static PagedVerticalMenuSelectUI Initialize(PagedVerticalMenuSelectUI prefab, RectTransform parent, List<MenuItemUIModel> models, int pageLength, bool cancelable = false) {
            PagedVerticalMenuSelectUI instance = Instantiate(prefab, parent).GetComponentWithError<PagedVerticalMenuSelectUI>();

            instance.pages = new List<PageUI>();
            instance.cancelable = cancelable;

            // Create menu items
            // instance.containerComponent.DestroyChildren();
            
            int idx = 0;
            int pageIdx = 0;
            while (idx < models.Count) {
                PageUI page = PageUI.Initialize(
                    instance.containerComponent,
                    models.TryGetRange(idx, pageLength),
                    instance.menuItemPrefab,
                    pageIdx
                );
                instance.pages.Add(page);
                page.gameObject.SetActive(false);

                idx += pageLength;
                pageIdx++;
            }


            return instance;
        }

        public async Task<object> GetSelectionAsync() {

            int focus = 0; // Which item is highligted on the page
            int pageFocus = 0; // which page is highlighted

            pages[pageFocus].gameObject.SetActive(true);
            pageCountComponent.text = $"{pageFocus + 1}/{pages.Count}";

            while (true) {
                int oldFocus = focus;
                int oldPageFocus = pageFocus;

                if (ControlManager.GetInputPressed(ControlManager.Up)) focus--;
                if (ControlManager.GetInputPressed(ControlManager.Down)) focus++;
                if (ControlManager.GetInputPressed(ControlManager.Left)) pageFocus--;
                if (ControlManager.GetInputPressed(ControlManager.Right)) pageFocus++;

                if (pageFocus < 0) 
                    pageFocus = 0;
                if (pageFocus > pages.Count - 1) 
                    pageFocus = pages.Count - 1;
                if (focus < 0) 
                    focus = 0;
                if (focus > pages[pageFocus].MenuItems.Count - 1) 
                    focus = pages[pageFocus].MenuItems.Count - 1; 

                if (pageFocus != oldPageFocus) {
                    pageCountComponent.text = $"{pageFocus + 1}/{pages.Count}";
                    foreach (PageUI page in pages) page.gameObject.SetActive(false);
                    pages[pageFocus].gameObject.SetActive(true);
                    pages[pageFocus].HighlightItem(focus);
                }
                if (focus != oldFocus)
                    pages[pageFocus].HighlightItem(focus);
                
                if (ControlManager.GetInputPressed(ControlManager.Accept)) { 
                    Close();
                    return pages[pageFocus].MenuItems[focus].Data;
                }
                if (ControlManager.GetInputPressed(ControlManager.Deny) && cancelable) { 
                    Close();
                    return null;
                }

                await new WaitForUpdate();
            }
        }

        public void Close() {
            Destroy(gameObject);
        }
    }
}