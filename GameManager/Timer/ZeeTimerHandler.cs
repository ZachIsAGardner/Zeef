using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zeef.GameManager {
    // Attach this to any object for timer functionality 
    public class ZeeTimerHandler : MonoBehaviour {
        [HideInInspector]
        public List<ZeeTimer> timers = new List<ZeeTimer>();

        public void AddTimer(Action newAction, float time) {
            timers.Add(
                new ZeeTimer(newAction, time)
            );
        }

        void Update() {
            HandleTimers();
        }
        
        public void HandleTimers() {
            // Go thorugh timers and tick them down
            timers.ForEach((timer) => {
                timer.Run();
            });

            // after each loop remove completed timers
            timers.RemoveAll(t => t.completed);
        }
    }
}