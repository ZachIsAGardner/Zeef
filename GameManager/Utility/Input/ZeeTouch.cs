using UnityEngine;

namespace Zeef.GameManager
{
    // Simple touch inputs for mobile devices
    public static class ZeeTouch {

        public static bool GetTouch(string type, TouchPhase phase) {

            if (Input.touchCount <= 0 || Input.GetTouch(0).phase != phase) {
                return false;
            }

            switch (type)
            {
                case "Left": 
                    if (Input.GetTouch(0).position.x < 700) {
                        return true;
                    } else {
                        return false;
                    }
                case "Right": 
                    if (Input.GetTouch(0).position.x >= 700) {
                        return true;
                    } else {
                        return false;
                    }
                default:
                    return false;
            }
        }
    }
}