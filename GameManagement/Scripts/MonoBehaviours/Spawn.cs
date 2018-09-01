using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ---
using Zeef;

namespace Zeef.GameManagement {

    [ExecuteInEditMode]
	public class Spawn : MonoBehaviour {

        public int ID { get; set; }

        // Set my ID to first available int
        void Awake() {
            Spawn[] spawns = FindObjectsOfType<Spawn>();

            int? result = null;
            if (spawns != null && spawns.Length > 0) {

                int i = 0;
                while (result == null) {
                    if (!spawns.Any(s => s.ID == i)) result = i;
                    i++;
                }
            } else {
                result = 0;
            }

            ID = result.Value;
            gameObject.name = $"Spawn {result}";
        }

        void Update() {
            gameObject.name = $"Spawn {ID}";
        }
	}
}
