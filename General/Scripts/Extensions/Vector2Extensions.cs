using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef 
{
    public enum DirectionsEnum { Up, Down, Left, Right }

    public static class Vector2Extensions 
    {   
        /// <summary>
        /// Returns a random value between x and y.
        /// </summary>
        public static float RandomValue (this Vector2 vector2) 
            => UnityEngine.Random.Range(vector2.x, vector2.y);
        
        /// <summary>
        /// Returns the direction as a Vector2 from this Vector2 to the provided target.
        /// </summary>
        public static Vector2 Direction(this Vector2 vector2, Vector2 target) 
        {
            Vector2 heading = target - vector2;
            float distance = heading.magnitude;
            return heading / distance;
        }

        /// <summary>
        /// Returns the distance from this Vector2 to the provided target.
        /// </summary>
        public static float Distance(this Vector2 vector2, Vector2 target) 
        {
            Vector2 heading = target - vector2;
            return heading.sqrMagnitude;
        }
    }
}