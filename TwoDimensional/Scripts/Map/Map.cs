using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional {

    public class FolderListItem {

		public string Name { get; set; }
		public int SelectionIdx { get; set; }
		public bool Visible { get; set; }
		public List<GameObject> Tiles { get; set; }

		public FolderListItem(TileFolderScriptable folder) {
			Name = folder.name;
			SelectionIdx = -1;
			Visible = false;
			Tiles = folder.Tiles;
		}
	}

    public class Map : SingleInstance<Map> {
        
        [Required]
        public string PlaceKey = "E";
        [Required]
        public string DeleteKey = "D";
        [Required]
        public string PickerKey = "F";

        public bool CreateLayerIfMissing = true;

        public List<string> LayerOptions = new List<string>() { "_Tiles" };
        public float GridSize = 1;
        public List<TileFolderScriptable> TileFolders;
        public Color CursorColor = new Color(0.5f,0.5f,0.75f,0.75f);

        [HideInInspector] public GameObject CurrentTile;
        [HideInInspector] public int CurrentLayerIdx;
        public string CurrentLayer { get { return LayerOptions[CurrentLayerIdx]; } }
        [HideInInspector] public Vector3 CursorPosition;
        [HideInInspector] public List<FolderListItem> FolderListItems;
        [HideInInspector] public GameObject Garbage;

        protected override void Awake() {
            Destroy(this.gameObject);
        }

        void OnDrawGizmos() {
            Gizmos.color = CursorColor;
            Gizmos.DrawCube(CursorPosition, new Vector3(GridSize, GridSize, 1));
        }
    }
}
