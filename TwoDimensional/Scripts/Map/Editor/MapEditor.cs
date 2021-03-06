﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Zeef.TwoDimensional
{
	public class LayerColorSet
    {
		public string Layer { get; set; }
		public List<SpriteRendererColorSet> SpriteRendererColorSets { get; set; }

		public LayerColorSet(string layer)
        {
			Layer = layer;
			SpriteRendererColorSets = new List<SpriteRendererColorSet>();
		}
	}

	public class SpriteRendererColorSet
    {
		public SpriteRenderer SpriteRenderer { get; set; }
		public Color Color { get; set; }

		public SpriteRendererColorSet(SpriteRenderer spriteRenderer, Color color)
        {
			SpriteRenderer = spriteRenderer;
			Color = color;
		}
	}
    
	[CustomEditor(typeof(Map))]
	public class MapEditor : Editor
    {
		Map map;

		private bool placed;
		private string keyHeld;
		private int framesHeld;
		private Vector2? lastGridDragPosition; 
		private Vector2 cursorPosition;

		private List<LayerColorSet> LayerColorSets { get; set; }

		// ---
		// ???

		 void OnFocus() {
			
		}

		void OnDestroy() {
			
		}

		// --- 
		// Inspector

		void OnEnable()
        {	
			SceneView.onSceneGUIDelegate -= this.MapOnSceneGUI;
			SceneView.onSceneGUIDelegate += this.MapOnSceneGUI;

			map = (Map)target; 
			if (map == null)
                return;

			keyHeld = null;
			map.Editing = true;
		}

		void OnDisable()
        {
			SceneView.onSceneGUIDelegate -= this.MapOnSceneGUI;

			if (map == null)
                return;

			map.Editing = false;
		}

		public override void OnInspectorGUI()
        {
			if (!GameObject.FindObjectOfType<Map>())
            {
				GUILayout.Label("Drag me into scene to make stuff! :)");
				return;
			}

			RenderInspector();
		}

		// ---
		// Render Inspector

		void RenderInspector()
        {
			GUILayout.Label("Map Settings", EditorStyles.boldLabel);
			DrawDefaultInspector();
			
			// Header
			GUILayout.Label("____________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________"); // whatever lol
			GUILayout.Label("Map Editor", EditorStyles.boldLabel);

			// Load folders from scriptables
			if (GUILayout.Button("Reload Folders") || map.FolderListItems.IsNullOrEmpty())
            {
				var folders = FillObjectList<TileFolderScriptable>(nameof(Map.TileFolders));			
				map.FolderListItems = new List<FolderListItem>();
				foreach (var folder in folders) 
					map.FolderListItems.Add(new FolderListItem(folder));
			}

			// Layer select
			// int oldLayerIdx = map.CurrentLayerIdx;
			map.CurrentLayerIdx = EditorGUILayout.Popup("Layer", map.CurrentLayerIdx, map.LayerOptions.ToArray());
			// Fade unselected layers
			// if (map.CurrentLayerIdx != oldLayerIdx) {

			// 	if (LayerColorSets != null) {
			// 		foreach (string layer in map.LayerOptions) {
			// 			LayerColorSet layerColorSet = LayerColorSets.FirstOrDefault(l => l.Layer == layer);

			// 			if (layerColorSet != null) {
			// 				foreach (SpriteRendererColorSet colorSet in layerColorSet.SpriteRendererColorSets) {
			// 					colorSet.SpriteRenderer.color = (map.CurrentLayer == layer) ? colorSet.Color : Color.gray;
			// 				}
			// 			}
			// 		}
			// 	}

			// 	PopulateLayerColorSets();
			// }

			// Render folders selection
			if (!map.FolderListItems.IsNullOrEmpty())
            {
				foreach (var folder in map.FolderListItems)
                {
					folder.Visible = EditorGUILayout.Foldout(folder.Visible, folder.Name);
					if (folder.Visible) { 
						RenderButtons(folder);					
						GUILayout.Label("");
					}
				}
			}
            else
            {
				GUILayout.Label("-- No folders to display --");
			}
		}

		void PopulateLayerColorSets()
        {
			if (LayerColorSets == null) LayerColorSets = new List<LayerColorSet>();

			GameObject layerGameObject = GameObject.Find(map.CurrentLayer);

			if (layerGameObject != null)
            {
				LayerColorSet layerColorSet = LayerColorSets.FirstOrDefault(l => l.Layer == map.CurrentLayer);

				if (layerColorSet == null)
                { 
					layerColorSet = new LayerColorSet(map.CurrentLayer);
					LayerColorSets.Add(layerColorSet);
				}

				layerColorSet.SpriteRendererColorSets = layerGameObject.GetComponentsInChildren<SpriteRenderer>()
					.Select(s => new SpriteRendererColorSet(s, s.color))
					.ToList();
			}			
		}

		void RenderButtons(FolderListItem folder)
        {
			if (folder.Tiles.IsNullOrEmpty())
                return;

			int old = folder.SelectionIdx;

			folder.SelectionIdx = GUILayout.SelectionGrid(
				selected: folder.SelectionIdx,
				texts: folder.Tiles.Select(p => p.name).ToArray(), 
				xCount: 3
			);
			
			if (old != folder.SelectionIdx)
            {
				map.CurrentTile = folder.Tiles[folder.SelectionIdx];

				foreach (var item in map.FolderListItems)
                {
					if (folder == item)
                        continue;

					item.SelectionIdx = -1;				
				}
			}
		}

		// ---
		// Scene

		void MapOnSceneGUI(SceneView sceneView)
        {
			// if (EditorWindow.focusedWindow != (SceneView)SceneView.sceneViews[0])
            //     return;

			RenderSceneBoxes();
			HandleInput();
		}

		void RenderSceneBoxes()
        {
			Handles.BeginGUI();

			GUI.color = Color.red;
			GUILayout.Box("Map Edit Mode");

			GUI.color = Color.yellow;

			bool anySceneFocused = false;
			foreach (var sceneView in SceneView.sceneViews)
			{
				if (sceneView == EditorWindow.focusedWindow)
				{
					anySceneFocused = true;
				}
			}	
			if (!anySceneFocused) 
			{
				GUILayout.Box("Scene view is not focused");
			}

			if (map.CurrentTile == null)
            { 
				GUI.color = Color.yellow;
				GUILayout.Box("No tile selected");
			}
            else
            {
				GUI.color = Color.cyan;
				GUILayout.Box($"Tile: {map.CurrentTile.name}");
			}

			GUI.color = Color.cyan;
			GUILayout.Box($"Layer: {map.CurrentLayer}");
			
			Handles.EndGUI();
		}

		void HandleInput()
        {
			Event e = Event.current;
			if (e.type == EventType.MouseMove)
            {
				Vector2 mousePosition = Event.current.mousePosition;
				mousePosition.y = Camera.current.pixelHeight - mousePosition.y;
				Vector3 clickPosition = Camera.current.ScreenPointToRay(mousePosition).origin;
				cursorPosition = GridPosition(clickPosition);
			}

			MoveCursor(cursorPosition);

			if (e.type == EventType.KeyDown && e.keyCode.ToString().ToLower() != "none")
            { 
				keyHeld = e.keyCode.ToString().ToLower();
				framesHeld++;
				e.Use(); // This gets rid of the error boop
			}

			if (e.type == EventType.KeyUp) { 
				keyHeld = null;
				framesHeld = 0;
				placed = false;
				lastGridDragPosition = null;
			}

			// Delete
			if (keyHeld == map.DeleteKey.ToLower() && cursorPosition != lastGridDragPosition)
            {
				Delete(cursorPosition);
				lastGridDragPosition = cursorPosition;
			}

			// Pick
			if (keyHeld == map.PickerKey.ToLower() && cursorPosition != lastGridDragPosition)
            {
				PickTile(cursorPosition);
				lastGridDragPosition = cursorPosition;
			}

			if (map.FolderListItems.IsNullOrEmpty())
                return;
			
			// Spawn
			if (keyHeld == map.PlaceKey.ToLower() && cursorPosition != lastGridDragPosition)
            {
				Delete(cursorPosition);
				Spawn(cursorPosition);
				lastGridDragPosition = cursorPosition;
			}
		}

		// Interactions

		void Spawn(Vector3 position)
        {
			if (map.CurrentTile == null) {
				Debug.Log("No tile is selected. Select a tile to place it.");

				return;
			}

			if (GameObject.Find(map.CurrentLayer) == null)
            {
				if (map.CreateLayerIfMissing)
                {
					new GameObject(map.CurrentLayer);
				}
                else
                {
					Debug.Log($"The '{map.CurrentLayer}' layer does not exist in the scene.");
					return;
				}
			} 

			GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(map.CurrentTile);

			go.transform.parent = GameObject.Find(map.CurrentLayer).transform;
			go.transform.localPosition = position;

			Undo.RegisterCreatedObjectUndo(go, "Create " + map.CurrentTile.name.ToString());
		}

		void Delete(Vector3 position)
        {
			GameObject layer = GameObject.Find(map.CurrentLayer);

			if (layer == null) return;

			foreach (var child in layer.GetComponentsInChildren<Transform>())
            {
				if (child.name == layer.name) continue;

				// Found a tile to delete
				if (Mathf.Abs(child.transform.position.x - position.x) < 0.1f && Mathf.Abs(child.transform.position.y - position.y) < 0.1f)
                {					
					if (map.Garbage == null)
                    {
						map.Garbage = new GameObject("_Garbage");
						map.Garbage.transform.parent = map.transform;
						map.Garbage.transform.position = Vector2.down * -10000;
					}

					child.transform.parent = map.Garbage.transform;
					child.transform.localPosition = Vector3.zero;

					// Garbage is set to inactive to ensure there are no shenanigans 
					// if it doesn't get deleted before runtime
					map.Garbage.SetActive(false);
				}	
			}	
		}

		void PickTile(Vector3 position)
        {
			GameObject layer = GameObject.Find(map.CurrentLayer);

			if (layer == null)
                return;

			foreach (var child in layer.GetComponentsInChildren<Transform>())
            {
				if (child.name == layer.name)
                    continue;

				if (child.transform.position.x == position.x && child.transform.position.y == position.y)
                {
					foreach (var folder in map.FolderListItems)
                    {
						foreach (var tile in folder.Tiles)
                        {
							if (tile.name == RemoveIterationFromName(child.name))
                            { 
								map.CurrentTile = tile;
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

		private string RemoveIterationFromName(string name)
        {
			int end = name.IndexOf("(");

			if (end == -1)
                return name;
			else
                return name.Substring(0, end - 1);
		}	

		Vector2 GridPosition(Vector2 oldPos)
        {
			Vector2 pos = new Vector2();
			
			pos.x = (Mathf.Ceil((oldPos.x - (map.GridSize / 2)) / map.GridSize) * map.GridSize) - ((map.GridSize % 1 == 0) ? 0.5f : 0);
			pos.y = (Mathf.Ceil((oldPos.y - (map.GridSize / 2)) / map.GridSize) * map.GridSize)- ((map.GridSize % 1 == 0) ? 0.5f : 0);

			return pos;
		}

		private void MoveCursor(Vector2 pos)
        {
			serializedObject.FindProperty(nameof(Map.CursorPosition)).vector3Value = pos;
			serializedObject.ApplyModifiedProperties();
		}

		private List<string> FillStringList(string propertyName)
        {
			SerializedProperty sp = serializedObject.FindProperty(propertyName).Copy();
			sp.Next(true);
			sp.Next(true);

			int length = sp.intValue;

			sp.Next(true);

			List<string> result = new List<string>();
		
			int lastIndex = length - 1;
			for (int i = 0; i < length; i++)
            {
				result.Add(sp.stringValue);

				if (i < lastIndex)
                    sp.Next(false);
			}

			return result;
		}

		private List<T> FillObjectList<T>(string propertyName) where T : UnityEngine.Object
        {
			
			SerializedProperty sp = serializedObject.FindProperty(propertyName).Copy();
			sp.Next(true);
			sp.Next(true);

			int length = sp.intValue;

			sp.Next(true);

			List<T> result = new List<T>();

			int lastIndex = length - 1;
			for (int i = 0; i < length; i++)
            {
				result.Add((T)sp.objectReferenceValue);

				if (i < lastIndex)
                    sp.Next(false);
			}

			return result;
		}

		void FocusSceneView()
        {
			if (SceneView.sceneViews.Count > 0)
            {
				SceneView sceneView = (SceneView)SceneView.sceneViews[0];
				sceneView.Focus();
			}
		}
	}
}
