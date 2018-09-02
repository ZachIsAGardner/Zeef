using System;
using UnityEngine;

namespace Zeef {

    [Serializable]
    public class Vector2Range {

        public Vector2 Min;
        public Vector2 Max;

        public Vector2Range(Vector2 min, Vector2 max) {
            this.Min = min;
            this.Max = max;
        }

        public Vector2 RandomValue() {
            float x = UnityEngine.Random.Range(Min.x, Max.x);
            float y = UnityEngine.Random.Range(Min.y, Max.y);
            return new Vector2(x, y);
        }
    }
}