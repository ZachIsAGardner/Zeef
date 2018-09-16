using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional {

    public class Map : SingleInstance<Map> {
        
        [Required]
        public string PlaceKey = "E";
        [Required]
        public string DeleteKey = "D";
        [Required]
        public string PickerKey = "F";

        public List<string> LayerOptions = new List<string>() { "_Tiles" };
        public float GridSize = 1;
        public List<TileFolderScriptable> TileFolders;
        public Color CursorColor = new Color(0.5f,0.5f,0.75f,0.75f);

        [HideInInspector]
        public int CurrentLayerIdx;
        [HideInInspector]
        public Vector3 CursorPosition;

        void OnDrawGizmos() {
            Gizmos.color = CursorColor;
            Gizmos.DrawCube(CursorPosition, new Vector3(GridSize, GridSize, 1));
        }
    }
}
