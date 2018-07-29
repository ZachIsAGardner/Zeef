using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef {

    public static class ListExtensions {
        
        public static T Random<T> (this List<T> list) {
            return list[Utility.RandomInt(list.Count - 1)];
        }
    }
}