using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef {

    public static class ListExtensions {
        
        public static T Random<T> (this List<T> list) {
            return list[Utility.RandomInt(list.Count - 1)];
        }

        public static bool IsNullOrEmpty<T> (this List<T> list) {
            return list == null || list.Count < 1;
        }

        // Gets whatever it can from start index plus the amount to try to grab
        public static List<T> TryGetRange<T> (this List<T> list, int index, int count) {
            List<T> result = new List<T>();

            for (int i = index; i < index + count; i++) {
                if (i >= list.Count) break;
                result.Add(list[i]);    
            }

            return result;
        }
    }
}