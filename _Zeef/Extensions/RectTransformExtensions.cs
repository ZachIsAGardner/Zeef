using System.Collections;
using UnityEngine;

namespace Zeef {
    public static class RectTransformExtensions {

        // Gets a ui elements anchored position relative to the canvas
        public static Vector2 AnchoredPositionCanvas(this RectTransform trans, bool adjustFromPivot = false) {
            Vector2 result = trans.anchoredPosition;
            if (adjustFromPivot) result += AdjustFromPivot(trans);

            RectTransform parent = trans.parent.GetComponent<RectTransform>();

            int i = 0;
            while (true) {
                if (parent.GetComponent<Canvas>() || i > 10) {
                    break;
                }
                i++;
                result += parent.anchoredPosition;
                if (adjustFromPivot) result += AdjustFromPivot(parent);                    

                parent = parent.parent.GetComponent<RectTransform>();
            }
            return result;
        }

        public static Vector2 AdjustFromPivot(RectTransform trans) {
            float halfWidth = trans.rect.width / 2;
            float pivotX = trans.pivot.x;
            float offsetX = halfWidth - (halfWidth * (pivotX + pivotX));

            float halfHeight = trans.rect.height / 2;
            float pivotY = trans.pivot.y;
            float offsetY = halfHeight - (halfHeight * (pivotY + pivotY));

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
        public static IEnumerator MoveTo(this RectTransform trans, RectTransform target, float smoothing = 10, bool adjustFromPivot = false, Vector2 offset = new Vector2()) {
            bool complete = false;
        
            Vector2 destination = AnchoredPositionCanvas(target, adjustFromPivot) + offset;

            while (!complete) {
                if (Vector2.Distance(trans.anchoredPosition, destination) < 0.05f) {
                    complete = true;
                }

                trans.anchoredPosition = Vector3.Lerp(trans.anchoredPosition, destination, smoothing * Time.deltaTime);

                yield return null;
            }
        }

    }
}