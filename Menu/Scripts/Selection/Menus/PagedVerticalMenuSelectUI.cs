using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Zeef.Menu
{
    public class PagedVerticalMenuSelectUI : MonoBehaviour, IMenuSelect
    { 
        [SerializeField] MenuItemUI menuItemPrefab;
        [SerializeField] GameObject containerComponent;
        [SerializeField] Text pageCountComponent;

        private List<PageUI> pages;
        private bool cancelable;

        public static PagedVerticalMenuSelectUI Initialize
            (PagedVerticalMenuSelectUI prefab, RectTransform parent, List<MenuItemUIModel> models, int pageLength, bool cancelable = false)
        {
            PagedVerticalMenuSelectUI instance = Instantiate(prefab, parent).GetComponentWithError<PagedVerticalMenuSelectUI>();

            instance.pages = new List<PageUI>();
            instance.cancelable = cancelable;

            // Create menu items
            // instance.containerComponent.DestroyChildren();
            
            int idx = 0;
            int pageIdx = 0;
            while (idx < models.Count)
            {
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

        public async Task<object> GetSelectionAsync(Func<bool> isCancelled = null)
        {
            return await GetSelectionAsync(isCancelled);
        }

        public async Task<object> GetSelectionAsync(Func<bool> isCancelled = null, int focus = 0, int pageFocus = 0)
        {
            pages[pageFocus].gameObject.SetActive(true);
            pageCountComponent.text = $"{pageFocus + 1}/{pages.Count}";

            while (true)
            {
                int oldFocus = focus;
                int oldPageFocus = pageFocus;

                if (ControlManager.GetInputPressed(ControlManager.Up) || ControlManager.GetAxisPressed(ControlManager.Vertical, true)) focus--;
                if (ControlManager.GetInputPressed(ControlManager.Down) || ControlManager.GetAxisPressed(ControlManager.Vertical, false)) focus++;
                if (ControlManager.GetInputPressed(ControlManager.Left) || ControlManager.GetAxisPressed(ControlManager.Horizontal, false)) pageFocus--;
                if (ControlManager.GetInputPressed(ControlManager.Right) || ControlManager.GetAxisPressed(ControlManager.Horizontal, true)) pageFocus++;

                if (pageFocus < 0) 
                    pageFocus = 0;
                if (pageFocus > pages.Count - 1) 
                    pageFocus = pages.Count - 1;
                if (focus < 0) 
                    focus = 0;
                if (focus > pages[pageFocus].MenuItems.Count - 1) 
                    focus = pages[pageFocus].MenuItems.Count - 1; 

                if (pageFocus != oldPageFocus)
                {
                    pageCountComponent.text = $"{pageFocus + 1}/{pages.Count}";
                    foreach (PageUI page in pages) page.gameObject.SetActive(false);
                    pages[pageFocus].gameObject.SetActive(true);
                    pages[pageFocus].HighlightItem(focus);
                }
                if (focus != oldFocus)
                    pages[pageFocus].HighlightItem(focus);
                
                if (ControlManager.GetInputPressed(ControlManager.Accept))
                { 
                    return pages[pageFocus].MenuItems[focus].Data;
                }
                if (ControlManager.GetInputPressed(ControlManager.Deny) && cancelable)
                { 
                    return null;
                }

                if (isCancelled != null && isCancelled())
                    return null;

                await new WaitForUpdate();
            }
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}