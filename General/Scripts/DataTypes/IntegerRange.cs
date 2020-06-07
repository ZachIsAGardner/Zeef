using UnityEngine;

namespace Zeef 
{
    [System.Serializable]
    public class IntegerRange 
    {     
        public int Min;
        public int Max;
        
        public IntegerRange(int min, int max) 
        {
            Min = min;
            Max = max;
        }

        public int RandomValue() 
        {
            return Random.Range(Min, Max);
        }
    }
}