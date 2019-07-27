using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef 
{
    public static class ListExtensions 
    {        
        public static T Random<T> (this List<T> list) 
            => list[Utility.RandomInt(list.Count)];

        public static bool IsNullOrEmpty<T> (this List<T> list)
            => list == null || list.Count < 1;
        
        ///<summary>
        /// Gets whatever it can from start index plus the amount to try to grab.
        ///</summary>
        public static List<T> TryGetRange<T> (this List<T> list, int index, int count) 
        {
            List<T> result = new List<T>();

            for (int i = index; i < index + count; i++) {
                if (i >= list.Count) break;
                result.Add(list[i]);    
            }

            return result;
        }
    }
}