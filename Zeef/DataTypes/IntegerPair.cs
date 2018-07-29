using UnityEngine;

namespace Zeef {

    [System.Serializable]
    public class IntegerPair {
        
        public int First;
        public int Second;
        
        public IntegerPair(int first, int second) {
            First = first;
            Second = second;
        }

        public int RandomValue() {
            return Random.Range(First, Second);
        }
    }
}