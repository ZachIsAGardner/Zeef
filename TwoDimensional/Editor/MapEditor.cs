using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Zeef.TwoDimensional {
    
	[CustomEditor(typeof(Map))]
	public class MapEditor : Editor {
        
		GameObject[] allPrefabs;
		GameObject selectedPrefab;
		Transform[] spawnedPrefabs;

		bool showTiles;
		string[] layers = new string[]{"ENTITIES", "GROUND", "BACKGROUND", "FOREGROUND"};
		int layerIdx;
		string currentLayer;

		int gridSize;

		// --- 
		// Inspector

		public override void OnInspectorGUI() {
			if (!GameObject.Find("MAP")) {
				GUILayout.Label("Drag me into scene to make stuff! :)");
				return;
			}
			DrawDefaultInspector();
			GUILayout.Label("MAP EDITOR");
			GUILayout.Label("");

			FillArrays();
			
			currentLayer = serializedObject.FindProperty("currentLayer").stringValue;
			gridSize = serializedObject.FindProperty("gridSize").intValue;

			layerIdx = EditorGUILayout.Popup("Layer", layerIdx, layers);
			currentLayer = layers[layerIdx];

			Render();
		}

		void Render() {
			string[] directories = GetSubFolders("Assets/Resources/Prefabs/Map");
			foreach (var directory in directories) {
				if (GetSubFolders(directory).Length == 0) {
					GUILayout.Label(GetFolderName(directory));
					RenderButtons(GetPrefabs(directory));
				} else {
					GUILayout.Label(GetFolderName(directory), EditorStyles.boldLabel);
				}
			}
		}

		string GetFolderName(string path) {
			string[] splitFolder = path.Split(new char[]{'/'});
			return splitFolder[splitFolder.Length - 1];
		}

		string[] GetSubFolders(string path) {
			string[] folders = Directory.GetDirectories(path);
			string[] subFolders = new string[]{};

			foreach (var folder in folders){
				subFolders = subFolders.Concat(GetSubFolders(folder)).ToArray();			
			}
			return folders.Concat(subFolders).ToArray();
		}

		GameObject[] GetPrefabs(string path) {
			path = path.Replace("Assets/Resources/", "");
			return Resources.LoadAll<GameObject>(path);
		}

		void FillArrays() {
			allPrefabs = Resources.LoadAll<GameObject>("Prefabs/Map");
			spawnedPrefabs = GameObject.Find("MAP").GetComponentsInChildren<Transform>();
		}

		void RenderButtons(GameObject[] prefabs) {
			GUILayout.BeginHorizontal();
			int rowElements = 0;
			foreach (var prefab in prefabs) {
				rowElements++;
				if (prefab.GetComponent<SpriteRenderer>()) {
					Sprite sprite = prefab.GetComponent<SpriteRenderer>().sprite;

					if (GUILayout.Button(GetTextureFromSheet(sprite), GUILayout.MaxWidth(50), GUILayout.MaxHeight(50))) {
						selectedPrefab = prefab;
						FocusSceneView();
					}
				} else {
					if (GUILayout.Button(prefab.name, GUILayout.MaxWidth(50), GUILayout.MaxHeight(50))) {
						selectedPrefab = prefab;
						FocusSceneView();
					}
				}

				if (rowElements > Screen.width / 70) {
					rowElements = 0;
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
				}
			}
			GUILayout.EndHorizontal();
		}

		Texture2D GetTextureFromSheet(Sprite sprite) {
			Texture2D texture = new Texture2D((int)sprite.textureRect.width, (int)sprite.textureRect.height);
			Color[] pixels = sprite.texture.GetPixels( 
				(int)sprite.textureRect.x,
				(int)sprite.textureRect.y,
				(int)sprite.textureRect.width,
				(int)sprite.textureRect.height
			);

			texture.SetPixels(pixels);
			texture.Apply();

			return texture;
		}

		void FocusSceneView() {
			if (SceneView.sceneViews.Count > 0) {
				SceneView sceneView = (SceneView)SceneView.sceneViews[0];
				sceneView.Focus();
			}
		}

		// ---
		// Scene

		void OnSceneGUI() {
			HandleClick();
			CreateSceneBoxes();
		}

		void HandleClick() {
			Vector2 mousePosition = Event.current.mousePosition;
			mousePosition.y = Camera.current.pixelHeight - mousePosition.y;
			Vector3 clickPosition = Camera.current.ScreenPointToRay(mousePosition).origin;

			// Vector3 clickPosition = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.E) {
				Delete(GridPosition(clickPosition));
				Spawn(GridPosition(clickPosition));
				FillArrays();
			}
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.D) {
				Delete(GridPosition(clickPosition));
				FillArrays();
				// This gets rid of the error boop
				Event.current.Use();
			}
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.I) {
				PickTile(GridPosition(clickPosition));
				// This gets rid of the error boop
				Event.current.Use();
			}
		}

		void CreateSceneBoxes() {
			Handles.BeginGUI();
			GUILayout.Box("Map Edit Mode");
			if (selectedPrefab == null) {
				GUILayout.Box("No prefab selected");
			}
			if (EditorWindow.focusedWindow != (SceneView)SceneView.sceneViews[0]) {
				GUILayout.Box("Scene view is not focused");
			}
			Handles.EndGUI();
		}

		Vector2 GridPosition(Vector2 oldPos) {
			Vector2 pos = new Vector2();
			pos.x = (Mathf.Ceil((oldPos.x - (gridSize / 2)) / gridSize) * gridSize);
			pos.y = (Mathf.Ceil((oldPos.y - (gridSize / 2)) / gridSize) * gridSize);
			return pos;
		}

		void Spawn(Vector3 position) {
			GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
			go.transform.parent = GameObject.Find(currentLayer).transform;
			go.transform.localPosition = position;
			Undo.RegisterCreatedObjectUndo(go, "Create " + selectedPrefab.name.ToString());
		}

		void Delete(Vector3 position) {
			foreach (var prefab in spawnedPrefabs) {
				if (prefab.transform.localPosition == position && prefab.transform.parent && prefab.transform.parent.name == currentLayer) {
					if (layers.Contains(prefab.name) || prefab.name == "MAP" || prefab.name == "SKY") {
						continue;
					}
					Transform parent = GameObject.Find("GARBAGE").transform;
					prefab.transform.parent = parent;
					prefab.transform.position += parent.position;
				}
			}
		}

		void PickTile(Vector3 position) {
			foreach (var prefab in spawnedPrefabs) {
				if (layers.Contains(prefab.name) || prefab.name == "MAP") {
						continue;
				}
				if (prefab.transform.position == position) {
					selectedPrefab = allPrefabs.First((p) => p.name == prefab.name);
				}
			}
		}
		
	}
}
