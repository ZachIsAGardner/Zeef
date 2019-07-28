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

        /// <summary>
        /// Returns the general direction as an enum from this Vector2 to the provided target.
        /// </summary>
        public static List<DirectionsEnum> GeneralDirection(this Vector2 vector2, Vector2 target)
        {
            List<DirectionsEnum> result = new List<DirectionsEnum>();
            if (target.x < vector2.x)
                result.Add(DirectionsEnum.Left);
            else
                result.Add(DirectionsEnum.Right);

            if (target.y < vector2.y)
                result.Add(DirectionsEnum.Down);
            else
                result.Add(DirectionsEnum.Up);

            return result;
        }

        /// <summary>
        /// Returns whether or not this Vector2 is to the left of the provided target.
        /// </summary>
        public static bool IsLeftOf(this Vector2 vector2, Vector2 target)
            => vector2.x < target.x;

        /// <summary>
        /// Returns whether or not this Vector2 is to the right of the provided target.
        /// </summary>
        public static bool IsRightOf(this Vector2 vector2, Vector2 target)
            => vector2.x > target.x;

        /// <summary>
        /// Returns whether or not this Vector2 is above of the provided target.
        /// </summary>
        public static bool IsAbove(this Vector2 vector2, Vector2 target)
            => vector2.y > target.y;

        /// <summary>
        /// Returns whether or not this Vector2 is below of the provided target.
        /// </summary>
        public static bool IsBelow(this Vector2 vector2, Vector2 target)
            => vector2.y < target.y;
    }
}