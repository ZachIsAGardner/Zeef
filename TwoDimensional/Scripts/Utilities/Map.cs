using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional {

    public class Map : MonoBehaviour {
        [HideInInspector]
        public string currentLayer = "GROUND";
        public int gridSize = 32;
    }

}
