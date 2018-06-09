using System;

namespace Zeef {
    [Serializable]
    public class IntegerPair 
    {
        public int first;
        public int second;
        
        public IntegerPair(int first, int second) {
            this.first = first;
            this.second = second;
        }

        public int RandomValue() {
            return UnityEngine.Random.Range(first, second);
        }
    }
}