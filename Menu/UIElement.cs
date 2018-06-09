using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Zeef.Menu
{
    public abstract class UIElement : MonoBehaviour 
    {
        public Image image;
        Color color;

        protected virtual void Awake() {
            image = image ?? GetComponentInChildren<Image>();
            color = image.color;
        }

        public virtual void Highlight() {
            image.color = Color.black;
        }

        public virtual void UnHighlight() {
            image.color = color;
        }
        
    }
}