using System.Collections;
using UnityEngine;

namespace Zeef {

    public static class TransformExtensions {

        public static void DestroyChildren (this Transform transform) {
            foreach (Transform child in transform)            
                GameObject.Destroy(child.gameObject); 
        }

        public static IEnumerator Shake(this Transform transform, float duration, float strength = 1) {
            Vector3 originalPosition = transform.position;

            int interval = 2;
            int i = 0;
            bool left = true;

            while (duration > 0) {
                if (i >= interval) {
                    transform.position = (left) 
                        ? originalPosition - (Vector3.right * strength * duration) 
                        : originalPosition + (Vector3.right * strength * duration);

                    left = !left;
                    i = 0;
                }

                i++;
                duration -= 1 * Time.deltaTime;
                yield return null;
            }

            transform.position = originalPosition;
        }
    }
}