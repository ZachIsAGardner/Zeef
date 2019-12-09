using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace Zeef.GameManagement {

    public class ScreenTransition : MonoBehaviour
    {
        private Image imageComponent;
        
        public static ScreenTransition Initialize(GameObject parent)
        {
            return Initialize(parent, Color.black);
        }

        public static ScreenTransition Initialize(GameObject parent, Color color)
        {
            ScreenTransition instance = new GameObject("LoadingScreen").AddComponent<ScreenTransition>();

            instance.transform.SetParent(parent.transform);

            instance.imageComponent = instance.gameObject.AddComponent<Image>();
            instance.imageComponent.color = new Color(color.r, color.g, color.b, 0);
            instance.imageComponent.rectTransform.anchoredPosition = Vector2.zero;
            instance.imageComponent.rectTransform.sizeDelta = new Vector2(9000, 6000);
            instance.imageComponent.rectTransform.anchorMin = new Vector2(0, 0);
            instance.imageComponent.rectTransform.anchorMax = new Vector2(1, 1);

            return instance;
        }

        // Darken Screen
        public async Task FadeOutAsync (float duration = 1)
        {
            
            while (imageComponent.color.a < 1)
            {
                imageComponent.color += new Color(0,0,0,(1 / duration) * Time.deltaTime);
                await new WaitForUpdate();
            }
        }

        // Undarken screen
        public async Task FadeInAsync (float duration = 1, Action callback = null)
        {

            while (imageComponent.color.a > 0)
            {
                imageComponent.color -= new Color(0,0,0,(1 / duration) * Time.deltaTime);
                await new WaitForUpdate();
            }

            if (callback != null) callback();
        }
    }
}