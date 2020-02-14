using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
// ---
using Zeef.Sound;
using System.Threading.Tasks;

namespace Zeef.GameManagement
{
	public partial class GameManager : SingleInstance<GameManager>
    {	
		/// <summary>
		/// The main canvas that persists between scenes.
		/// </summary>
		public static Canvas Canvas { get => GetInstance().canvas; }
		[Required]
		[SerializeField] private Canvas canvas;

		[Range(0, 5)]
		[SerializeField] private float transitionTime = 1;

		[SerializeField] private Color transitionColor = Color.black;

		// -

		/// <summary>
		/// A "Package" is an object intended to serve as parameters or arguments for a scene.
		/// </summary>
		public static object ScenePackage { get => GetInstance().scenePackage; }
		private object scenePackage;

        /// <summary>
        /// Returns the Scene specific Canvas, if it exists.
        /// </summary>
        public static Canvas SceneCanvas => GameObject.FindGameObjectWithTag(TagConstant.SceneCanvas).GetComponent<Canvas>();
		
		public static bool IsLoading { get; private set; }
		public static bool IsPaused { get; set; }
		public static bool IsPlaying { get => GetInstance().isPlaying; }
		protected virtual bool isPlaying => !IsLoading && !IsPaused;

		protected string lastLoadedScene;

		public static event EventHandler BeforeLeaveScene;

		// ---
		// Setup

		protected override void Awake()
        {
			base.Awake();
			DontDestroyOnLoad(gameObject);

			Application.targetFrameRate = 60;			
		}

		void Start()
        {
			lastLoadedScene = SceneManager.GetActiveScene().name;
		}

		// ---

		/// <summary>
		/// A "Package" is an object intended to serve as parameters or arguments for a scene.
		/// </summary>
		public static T OpenPackage<T>() => (T)GetInstance().scenePackage;

		// ---
		// Running

		/// <summary>
		/// Spawns an empty GameObject inside the DynamicFolder GameObject.
		/// </summary>
		public static GameObject Spawn(Vector3 position)
        {
			GameObject actor =  new GameObject();

            GameObject folder = GameObject.FindGameObjectWithTag(TagConstant.DynamicFolder);

            if (folder == null)
            {
                folder = new GameObject("_DyanamicFolder");
                folder.tag = TagConstant.DynamicFolder;
            }

			actor.gameObject.transform.parent = folder.transform;
			actor.gameObject.transform.position = position;

			return actor;
        }

		/// <summary>
		/// Spawns a copy of the provided prefab inside the DynamicFolder GameObject.
		/// </summary>
		public static GameObject Spawn(GameObject prefab, Vector3 position)
        {
			GameObject folder = GameObject.FindGameObjectWithTag(TagConstant.DynamicFolder);
			
			if (folder == null)
            {
				folder = new GameObject("_DyanamicFolder");
				folder.tag = TagConstant.DynamicFolder;
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
					folder = new GameObject(name);

					folder.AddComponent<RectTransform>();
					folder.GetComponent<RectTransform>().SetParent(SceneCanvas.GetComponent<RectTransform>());

					folder.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
					folder.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
					folder.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
					folder.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);

					folder.GetComponent<RectTransform>().localScale = Vector3.one;

					// Update folder fields
					folder.tag = TagConstant.DynamicCanvasFolder;
				}
				
				folders.Add(folder);

				i++;
			}	

            return Instantiate(
                original: prefab,
                parent: folders[hierarchy].GetComponent<RectTransform>()
            );
		}

        /// <summary>
        /// Loads the first scene found with a name matching the provided <paramref name="scene"/> parameter.
        /// </summary>
        public static async Task LoadSceneAsync(string scene, LoadSceneMode loadMode = LoadSceneMode.Single, object package = null, bool transition = true)
        {
			if (transition)
            {
				GetInstance().lastLoadedScene = scene;
				GetInstance().scenePackage = package;

				IsLoading = true;
				await new WaitForUpdate();

				ScreenTransition screenTransition = ScreenTransition.Initialize(
					GetInstance().canvas.gameObject, 
					GetInstance().transitionColor
				);

				await screenTransition.FadeOutAsync(GetInstance().transitionTime);

				GetInstance().OnBeforeLeaveScene();
				SceneManager.LoadScene(scene, loadMode);

				await screenTransition.FadeInAsync(GetInstance().transitionTime);

				Destroy(screenTransition.gameObject);

				IsLoading = false;
			} else {
				SceneManager.LoadScene(scene, loadMode);
			}
		} 

		// ---
		// Events

		protected virtual void OnBeforeLeaveScene()
        {
			if (BeforeLeaveScene != null) 
				BeforeLeaveScene(this, EventArgs.Empty);
		}
	}
}