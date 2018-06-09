using System;
using UnityEngine;

namespace Zeef.GameManager {
    [Serializable]
    public class Vector2Range {
        public Vector2 min;
        public Vector2 max;

        public Vector2Range(Vector2 min, Vector2 max) {
            this.min = min;
            this.max = max;
        }

        public Vector2 RandomValue() {
            float x = UnityEngine.Random.Range(min.x, max.x);
            float y = UnityEngine.Random.Range(min.y, max.y);
            return new Vector2(x, y);
        }
    }
}