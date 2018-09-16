using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
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
    
	[CustomEditor(typeof(Map))]
	public class MapEditor : Editor {
        
		Map map;

		GameObject currentTile;

		string currentLayer { get { return map.LayerOptions[map.CurrentLayerIdx]; } }

		private GameObject cursor;
		private List<FolderListItem> folderListItems;

		// --- 
		// Inspector

		void OnEnable() {
			map = (Map)target;
		}

		public override void OnInspectorGUI() {

			if (!GameObject.FindObjectOfType<Map>()) {
				GUILayout.Label("Drag me into scene to make stuff! :)");
				return;
			}

			RenderInspector();
		}

		// ---
		// Render Inspector

		void RenderInspector() {
			GUILayout.Label("Map Settings", EditorStyles.boldLabel);
			DrawDefaultInspector();
			
			// Header
			GUILayout.Label("____________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________"); // whatever lol
			GUILayout.Label("Map Editor", EditorStyles.boldLabel);

			// Load folders from scriptables
			if (GUILayout.Button("Reload Folders") || folderListItems.IsNullOrEmpty()) {
				var folders = FillObjectList<TileFolderScriptable>(nameof(Map.TileFolders));			
				folderListItems = new List<FolderListItem>();
				foreach (var folder in folders) 
					folderListItems.Add(new FolderListItem(folder));
			}

			// Layer select
			map.CurrentLayerIdx = EditorGUILayout.Popup("Layer", map.CurrentLayerIdx, map.LayerOptions.ToArray());
			
			// Render folders selection
			if (!folderListItems.IsNullOrEmpty()) {
				foreach (var folder in folderListItems) {
					folder.Visible = EditorGUILayout.Foldout(folder.Visible, folder.Name);
					if (folder.Visible) { 
						RenderButtons(folder);					
						GUILayout.Label("");
					}
				}
			} else {
				GUILayout.Label("-- No folders to display --");
			}
		}

		void RenderButtons(FolderListItem folder) {
			int old = folder.SelectionIdx;

			folder.SelectionIdx = GUILayout.SelectionGrid(
				selected: folder.SelectionIdx,
				texts: folder.Tiles.Select(p => p.name).ToArray(), 
				xCount: 4
			);
			
			if (old != folder.SelectionIdx) {
				currentTile = folder.Tiles[folder.SelectionIdx];
				FocusSceneView();
				foreach (var item in folderListItems) {
					if (folder == item) continue;
					item.SelectionIdx = -1;				
				}
			}
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

		// ---
		// Scene

		void OnSceneGUI() {
			RenderSceneBoxes();
			HandleInput();
		}

		void RenderSceneBoxes() {
			Handles.BeginGUI();

			GUI.color = Color.red;
			GUILayout.Box("Map Edit Mode");

			GUI.color = Color.yellow;
			if (EditorWindow.focusedWindow != (SceneView)SceneView.sceneViews[0]) GUILayout.Box("Scene view is not focused");
			if (currentTile == null) { 
				GUI.color = Color.yellow;
				GUILayout.Box("No tile selected");
			} else  {
				GUI.color = Color.cyan;
				GUILayout.Box($"Tile: {currentTile.name}");
			}

			GUI.color = Color.cyan;
			GUILayout.Box($"Layer: {currentLayer}");
			
			Handles.EndGUI();
		}

		void HandleInput() {
			Vector2 mousePosition = Event.current.mousePosition;
			mousePosition.y = Camera.current.pixelHeight - mousePosition.y;
			Vector3 clickPosition = Camera.current.ScreenPointToRay(mousePosition).origin;
			
			MoveCursor(GridPosition(clickPosition));

			if (Event.current.type == EventType.KeyDown && 
				Event.current.keyCode.ToString().ToLower() == serializedObject.FindProperty(nameof(Map.DeleteKey)).stringValue.ToLower()) {
				Delete(GridPosition(clickPosition));
				Event.current.Use(); // This gets rid of the error boop
			}
			if (Event.current.type == EventType.KeyDown && 
				Event.current.keyCode.ToString().ToLower() == serializedObject.FindProperty(nameof(Map.PickerKey)).stringValue.ToLower()) {
				PickTile(GridPosition(clickPosition));
				Event.current.Use();
			}

			if (folderListItems.IsNullOrEmpty()) return;
			
			// Vector3 clickPosition = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
			if (Event.current.type == EventType.KeyDown && 
				Event.current.keyCode.ToString().ToLower() == serializedObject.FindProperty(nameof(Map.PlaceKey)).stringValue.ToLower()) {
				Delete(GridPosition(clickPosition));
				Spawn(GridPosition(clickPosition));
				Event.current.Use();
			}
		}

		// Interactions

		void Spawn(Vector3 position) {
			if (GameObject.Find(currentLayer) == null) {
				Debug.Log("The current layer requested does not exist in the scene.");
				return;
			}
			GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(currentTile);

			go.transform.parent = GameObject.Find(currentLayer).transform;
			go.transform.localPosition = position;

			Undo.RegisterCreatedObjectUndo(go, "Create " + currentTile.name.ToString());
		}

		void Delete(Vector3 position) {
			GameObject layer = GameObject.Find(currentLayer);

			if (layer == null) return;

			foreach (var child in layer.GetComponentsInChildren<Transform>()) {
				if (child.name == layer.name) continue;

				if (Mathf.Abs(child.transform.position.x - position.x) < 0.1f && Mathf.Abs(child.transform.position.y - position.y) < 0.1f) {
					GameObject garbage =  GameObject.Find("_Garbage");

					if (garbage == null) { 
						garbage = new GameObject("_Garbage");
						garbage.transform.parent = FindObjectOfType<Map>().transform;
						garbage.transform.position = Vector3.up * -10000;
					}

					child.transform.parent = garbage.transform;
					child.transform.localPosition = Vector3.zero;
				}	
			}	
		}

		void PickTile(Vector3 position) {
			GameObject layer = GameObject.Find(currentLayer);

			if (layer == null) return;

			foreach (var child in layer.GetComponentsInChildren<Transform>()) {
				if (child.name == layer.name) continue;

				if (child.transform.position.x == position.x && child.transform.position.y == position.y) {
					foreach (var folder in folderListItems) {
						foreach (var tile in folder.Tiles) {
							if (tile.name == RemoveIterationFromName(child.name)) { 
								currentTile = tile;
								folder.SelectionIdx = folder.Tiles.IndexOf(tile);
								return;
							}
						}	
					}
				}	
			}		
		}


		// ---
		// Helper

		private string RemoveIterationFromName(string name) {
			int end = name.IndexOf("(");
			if (end == -1) return name;
			else return name.Substring(0, end - 1);
		}	

		Vector2 GridPosition(Vector2 oldPos) {
			Vector2 pos = new Vector2();
			
			pos.x = (Mathf.Ceil((oldPos.x - (map.GridSize / 2)) / map.GridSize) * map.GridSize) - ((map.GridSize % 1 == 0) ? 0.5f : 0);
			pos.y = (Mathf.Ceil((oldPos.y - (map.GridSize / 2)) / map.GridSize) * map.GridSize)- ((map.GridSize % 1 == 0) ? 0.5f : 0);

			return pos;
		}

		private void MoveCursor(Vector2 pos) {
			serializedObject.FindProperty(nameof(Map.CursorPosition)).vector3Value = pos;
			serializedObject.ApplyModifiedProperties();
		}

		private List<string> FillStringList(string propertyName){
			SerializedProperty sp = serializedObject.FindProperty(propertyName).Copy();
			sp.Next(true);
			sp.Next(true);

			int length = sp.intValue;

			sp.Next(true);

			List<string> result = new List<string>();
		
			int lastIndex = length - 1;
			for (int i = 0; i < length; i++) {
				result.Add(sp.stringValue);
				if (i < lastIndex) sp.Next(false);
			}

			return result;
		}

		private List<T> FillObjectList<T>(string propertyName) where T : UnityEngine.Object {
			
			SerializedProperty sp = serializedObject.FindProperty(propertyName).Copy();
			sp.Next(true);
			sp.Next(true);

			int length = sp.intValue;

			sp.Next(true);

			List<T> result = new List<T>();

			int lastIndex = length - 1;
			for (int i = 0; i < length; i++) {
				result.Add((T)sp.objectReferenceValue);
				if (i < lastIndex) sp.Next(false);
			}

			return result;
		}

		void FocusSceneView() {
			if (SceneView.sceneViews.Count > 0) {
				SceneView sceneView = (SceneView)SceneView.sceneViews[0];
				sceneView.Focus();
			}
		}
	}
}
