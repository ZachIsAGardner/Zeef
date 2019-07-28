using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef 
{
    public static class Vector3Extensions 
    {   
        /// <summary>
        /// Returns whether or not this vector2 is visible on the main camera.
        /// </summary>
        public static bool IsVisibleOnCamera(this Vector3 vector3, float bufferX = 0, float bufferY = 0)
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(vector3);

            return screenPoint.z > 0 
                && screenPoint.x > 0 - bufferX && screenPoint.x < 1 + bufferX
                && screenPoint.y > 0 - bufferY && screenPoint.y < 1 + bufferY;
        }
    }
}