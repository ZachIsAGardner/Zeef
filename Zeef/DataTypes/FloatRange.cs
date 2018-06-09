using System;

namespace Zeef.GameManager {
    [Serializable]
    public class FloatRange 
    {
        public float min;
        public float max;
        public FloatRange(float min, float max) {
            this.min = min;
            this.max = max;
        }

        public float RandomValue() {
            return UnityEngine.Random.Range(min, max);
        }
    }
}
