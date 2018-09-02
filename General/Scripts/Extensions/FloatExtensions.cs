using System;
using UnityEngine;

namespace Zeef {

    public static class FloatExtensions {
        
        public static float MoveOverTime (this float num, float destination, float time) {
			return Mathf.Lerp(num, destination, 1 - Mathf.Pow(time, Time.deltaTime));
        }
    }
}