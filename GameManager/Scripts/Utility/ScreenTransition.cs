using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Zeef.GameManagement{

    public static class ScreenTransition 
    {
        public static Image CreateLoadScreen() 
        {
            GameObject go = new GameObject();

            go.name = "LoadingScreen";           
            go.transform.SetParent(Identifier.FindIdentifierObject(IdentifiersEnum.GameCanvas).transform); 
            Image image = go.AddComponent<Image>();
            image.color = Color.clear;
            image.rectTransform.anchoredPosition = Vector2.zero;
            image.rectTransform.sizeDelta = new Vector2(900, 600);
            image.rectTransform.anchorMin = new Vector2(0, 0);
            image.rectTransform.anchorMax = new Vector2(1, 1);

            return image;
        }

        public static IEnumerator FadeOut (Image screen, float duration = 1) 
        {
            while (screen.color.a < 1) {
                screen.color += new Color(0,0,0,(1 / duration) * Time.deltaTime);
                yield return null;
            }
        }

        public static IEnumerator FadeIn (Image screen, float duration = 1, Action callback = null) 
        {
            while (screen.color.a > 0) {
                screen.color -= new Color(0,0,0,(1 / duration) * Time.deltaTime);
                yield return null;
            }

            if (callback != null) callback();
        }
    }
}