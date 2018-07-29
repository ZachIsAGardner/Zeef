using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Zeef.Menu {

    public class UIElement : MonoBehaviour {

        [SerializeField] private Image imageComponent;
        public Image ImageComponent { get { return imageComponent; } }
        public RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }

        private Color initialColor;

        protected virtual void Awake() {
            imageComponent = imageComponent ?? GetComponentInChildren<Image>();
            initialColor = ImageComponent.color;
        }

        public virtual void Highlight() {
            imageComponent.color = Color.black;
        }

        public virtual void UnHighlight() {
            imageComponent.color = initialColor;
        }
        
    }
}