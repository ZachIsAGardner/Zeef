using System.Collections;
using UnityEngine;
using Zeef.GameManagement;

namespace Zeef {
    
    public static class RectTransformExtensions {

        // Gets a ui elements anchored position relative to the canvas
        public static Vector2 AnchoredPositionCanvas(this RectTransform trans) {
            Vector2 result = trans.anchoredPosition + GetOffsetFromPivot(trans);

            RectTransform parent = trans.parent.GetComponent<RectTransform>();
            int i = 0;
            while (true) {
                if (parent.GetComponent<Canvas>() || i > 30) break;
                
                i++;
                result += parent.anchoredPosition + GetOffsetFromPivot(parent);                    

                parent = parent.parent.GetComponent<RectTransform>();
            }
            return result;
        }

        // Only looks at anchor max
        private static Vector2 GetOffsetFromPivot(RectTransform trans) {
            // offset x

            float halfWidth = trans.rect.width / 2;
            RectTransform parent = trans.parent.GetComponent<RectTransform>();

            // Get offset from anchor
            float anchorX = trans.anchorMax.x;
            float halfParentWidth = parent.sizeDelta.x / 2; 
            float offsetX = -halfParentWidth + (halfParentWidth * (anchorX * 2));

            // Get offset from pivot
            float pivotX = trans.pivot.x;
            offsetX += halfWidth - (halfWidth * (pivotX * 2));

            // ---
            // offset y

            float halfHeight = trans.rect.height / 2;
            
            // Get offset from anchor
            float anchorY = trans.anchorMax.y;
            float halfParentHeight = parent.sizeDelta.y / 2;
            float offsetY = -halfParentHeight + (halfParentHeight * (anchorY * 2));

            // Get offset from pivot
            float pivotY = trans.pivot.y;
            offsetY += halfHeight - (halfHeight * (pivotY * 2));

            return new Vector2(offsetX, offsetY);
        }


        public static IEnumerator MoveTo(this RectTransform trans, Vector2 destination, float smoothing = 10) {
            bool complete = false;

            while (!complete) {
                if (Vector2.Distance(trans.anchoredPosition, destination) < 0.05f) {
                    complete = true;
                }

                trans.anchoredPosition = Vector3.Lerp(trans.anchoredPosition, destination, smoothing * Time.deltaTime);

                yield return null;
            }
        }
        public static IEnumerator MoveTo(this RectTransform trans, RectTransform target, Canvas canvas, float smoothing = 10, Vector2 offset = new Vector2()) {
            bool complete = false;
        
            Vector2 destination = AnchoredPositionCanvas(target) + offset;

            while (!complete) {
                if (Vector2.Distance(trans.anchoredPosition, destination) < 0.05f) 
                    complete = true;
                
                trans.anchoredPosition = Vector3.Lerp(trans.anchoredPosition, destination, smoothing * Time.deltaTime);

                yield return null;
            }
        }

    }
}