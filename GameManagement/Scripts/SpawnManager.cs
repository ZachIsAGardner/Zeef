using System.Collections.Generic;
using UnityEngine;

namespace Zeef.GameManagement
{
	public class SpawnManager : SingleInstance<SpawnManager>
    {	
        private Canvas dynamicCanvas;
        [SerializeField]
        private Canvas dynamicCanvasPrefab;
        
		/// <summary>
		/// Spawns an empty GameObject inside the _DynamicFolder GameObject.
		/// </summary>
		public static GameObject Spawn(Vector3 position)
        {
			GameObject actor =  new GameObject();

            GameObject folder = GameObject.FindGameObjectWithTag("DynamicFolder");

            if (folder == null)
            {
                folder = new GameObject("_DyanamicFolder");
                folder.tag = "DynamicFolder";
            }

			actor.gameObject.transform.parent = folder.transform;
			actor.gameObject.transform.position = position;

			return actor;
        }

		/// <summary>
		/// Spawns a copy of the provided prefab inside the _DynamicFolder GameObject.
		/// </summary>
		public static GameObject Spawn(GameObject prefab, Vector3 position)
        {
			GameObject folder = GameObject.FindGameObjectWithTag("DynamicFolder");
			
			if (folder == null)
            {
				folder = new GameObject("_DyanamicFolder");
				folder.tag = "DynamicFolder";
			}

			return Instantiate(
				original: prefab, 
				position: position, 
				rotation: Quaternion.identity, 
				parent: folder.transform
			);
		}

        /// <summary>
        /// Spawns a copy of the provided prefab inside the provided parent.
        /// </summary>
        public static GameObject Spawn(GameObject prefab, Vector2 position, GameObject parent)
        {
            return Instantiate(
                original: prefab,
                position: position,
                rotation: Quaternion.identity,
                parent: parent.transform
            );
        }

        /// <summary>
        /// Spawns a copy of the provided prefab inside the provided parent.
        /// </summary>
        public static GameObject Spawn(GameObject prefab, GameObject parent)
        {
            return Instantiate(
                original: prefab,
                position: Vector2.zero,
                rotation: Quaternion.identity,
                parent: parent.transform
            );
        }

        /// <summary>
        /// Spawns a copy of the provided prefab inside the DynamicCanvasFolder GameObject.
		/// <param name="prefab">The GameObject to create a copy of.</param>
		/// <param name="hierarchy">The sorting layer to place the prerab instance into. Higher numbers have priority of lower ones.</param>
        /// </summary>
        public static GameObject SpawnCanvasElement(GameObject prefab, int hierarchy = 0)
        {
			List<GameObject> folders = new List<GameObject>();

			int i = 0;
			while(i <= hierarchy)
			{
				string name = $"_DynamicCanvasFolder_{i}";
				GameObject folder = GameObject.Find(name);

				// Create new folder
				if (folder == null)
				{
                    if (Instance.dynamicCanvas == null) {
                        Instance.dynamicCanvas = Instantiate(Instance.dynamicCanvasPrefab);
                        Instance.dynamicCanvas.name = "DyanamicCanvas";
                    }
                    
					folder = new GameObject(name);
					folder.AddComponent<RectTransform>();

					folder.GetComponent<RectTransform>().SetParent(Instance.dynamicCanvas.GetComponent<RectTransform>());

					folder.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
					folder.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
					folder.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
					folder.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
					folder.GetComponent<RectTransform>().localScale = Vector3.one;
				}
				
				folders.Add(folder);

				i++;
			}	

            return Instantiate(
                original: prefab,
                parent: folders[hierarchy].GetComponent<RectTransform>()
            );
		}
	}
}