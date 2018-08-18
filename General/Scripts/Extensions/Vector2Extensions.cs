using System;
using UnityEngine;

namespace Zeef {

    public static class Vector2Extensions {
        
        public static float RandomValue (this Vector2 vector2) {
            return UnityEngine.Random.Range(vector2.x, vector2.y);
        }
    }
}