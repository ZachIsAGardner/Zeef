using System;

namespace Zeef.GameManager {
    [Serializable]
    public class IntegerRange 
    {
        public int min;
        public int max;
        public IntegerRange(int min, int max) {
            this.min = min;
            this.max = max;
        }

        public int RandomValue() {
            return UnityEngine.Random.Range(min, max);
        }
    }
}