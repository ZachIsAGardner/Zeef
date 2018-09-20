using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ---
using Zeef;

namespace Zeef.GameManagement {

    [ExecuteInEditMode]
	public class Spawn : MonoBehaviour {

        public int ID;

        void Update() {
            gameObject.name = $"Spawn {ID}";
        }
	}
}
