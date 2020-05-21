using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zeef.GameManagement;

namespace Zeef.Menu 
{
    public class BarUI : MonoBehaviour 
    {
        [SerializeField] Image barTop;
        [SerializeField] Image barLoss;

        public static BarUI Initialize(BarUI prefab, Transform parent, float percentage) 
        {
            BarUI instance = Instantiate(prefab, parent).GetComponent<BarUI>();
            
            instance.barLoss.rectTransform.sizeDelta = instance.barTop.rectTransform.sizeDelta = new Vector2(
                instance.GetComponent<RectTransform>().sizeDelta.x * percentage, 
                instance.barTop.rectTransform.sizeDelta.y
            );

            return instance;
        }

        public virtual void UpdateDisplay(float percentage) 
        {
            barTop.rectTransform.sizeDelta = barLoss.rectTransform.sizeDelta = new Vector2(
                GetComponent<RectTransform>().sizeDelta.x * percentage, 
                barTop.rectTransform.sizeDelta.y
            );
        }

        // Duration: how long the freeze is after bar top is set and 
        // how long it takes for loss to catch up
        public virtual async Task UpdateDisplayAsync(float percentage, float duration = 0.25f) 
        {
            // If we're already where we need to be, dont bother
            if (barTop.rectTransform.sizeDelta.x == GetComponent<RectTransform>().sizeDelta.x * percentage) return;
            
            // Set top to white and pause for a moment
            Color topColor = barTop.color;
            barTop.color = Color.white;
            while (!GameState.IsPlaying) await new WaitForUpdate();
            await new WaitForSeconds(0.1f);

            // Set loss to old top size
            barLoss.rectTransform.sizeDelta = barTop.rectTransform.sizeDelta;
            // Set top size to new size

            barTop.rectTransform.sizeDelta = new Vector2(
                GetComponent<RectTransform>().sizeDelta.x * percentage, 
                barTop.rectTransform.sizeDelta.y
            );
            barTop.color = topColor;

            while (!GameState.IsPlaying) await new WaitForUpdate();
            await new WaitForSeconds(duration);

            // Move loss bar over time
            while (Mathf.Abs(barTop.rectTransform.sizeDelta.x - barLoss.rectTransform.sizeDelta.x) > 1) 
            {
                while(!GameState.IsPlaying) await new WaitForUpdate();
                if (barLoss == null) return;

                float time = Time.deltaTime / duration;
                barLoss.rectTransform.sizeDelta = new Vector2(
                    Mathf.Lerp(barLoss.rectTransform.sizeDelta.x, barTop.rectTransform.sizeDelta.x, time),
                    barLoss.rectTransform.sizeDelta.y
                );

                await new WaitForUpdate();
            }
            barLoss.rectTransform.sizeDelta = new Vector2(barTop.rectTransform.sizeDelta.x, barLoss.rectTransform.sizeDelta.y);
        }
    }
}