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

namespace Zeef.GameManagement {

	public enum GameStatesEnum {
		Play,
		Pause,
		Cutscene,
		Loading,
	}

	public class GameManager : SingleInstance<GameManager> {
	
		/// <summary>
		/// Special actions are only available in dev mode.
		/// </summary>
		public static bool IsDev { get { return GetInstance().isDev; } }
        [SerializeField] private bool isDev = true;

		/// <summary>
		/// The main canvas that persists between scenes.
		/// </summary>
		public static Canvas Canvas { get { return GetInstance().canvas; }}
		[Required]
		[SerializeField] private Canvas canvas;

		[Range(0, 5)]
		[SerializeField] private float transitionTime = 1;

		[SerializeField] private Color transitionColor = Color.black;

		// -

		/// <summary>
		/// A "Package" is an object intended to serve as parameters or arguments for a scene.
		/// </summary>
		public static object ScenePackage { get { return GetInstance().scenePackage; } }
		private object scenePackage { get; set; }
		
		/// <summary>
		/// The state of the game. Certain behaviours will act differenly depending on the state.
		///
		/// Possible states are Play, Pause, Cutscene, and Loading
		/// </summary>
		public static GameStatesEnum GameState { get; private set; }

		protected string lastLoadedScene;

		public static event EventHandler BeforeLeaveScene;

		// ---
		// Setup

		protected override void Awake() {
			base.Awake();
			GameState = GameStatesEnum.Play;
			DontDestroyOnLoad(gameObject);

			Application.targetFrameRate = 60;			
		}

		void Start() {
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
		public static GameObject SpawnActor(Vector3 position) {
			GameObject actor =  new GameObject();

			actor.gameObject.transform.parent = Utility.FindGameObjectWithTagWithError(TagConstants.DynamicFolder).transform;
			actor.gameObject.transform.position = position;

			return actor;
		}

		/// <summary>
		/// Spawns a copy of the provided prefab inside the DynamicFolder GameObject.
		/// </summary>
		public static GameObject SpawnActor(GameObject prefab, Vector3 position) {
			GameObject folder = GameObject.FindGameObjectWithTag(TagConstants.DynamicFolder);
			
			if (folder == null) {
				folder = new GameObject("_DyanamicFolder");
				folder.tag = TagConstants.DynamicFolder;
			}

			return Instantiate(
				original: prefab, 
				position: position, 
				rotation: Quaternion.identity, 
				parent: Utility.FindGameObjectWithTagWithError(TagConstants.DynamicFolder).transform
			);
		}

		/// <summary>
		/// Spawns a copy of the provided prefab inside the DynamicCanvasFolder GameObject.
		/// </summary>
		public static GameObject SpawnCanvasElement(GameObject prefab, Vector2 position) {
			GameObject folder = GameObject.FindGameObjectWithTag(TagConstants.DynamicCanvasFolder);
			
			if (folder == null) {
				folder = new GameObject("_DyanamicCanvasFolder");
				folder.transform.parent = Utility.FindGameObjectWithTagWithError(TagConstants.SceneCanvas).transform;
				folder.tag = TagConstants.DynamicCanvasFolder;
			}

			return Instantiate(
				original: prefab, 
				position: position, 
				rotation: Quaternion.identity, 
				parent: Utility.FindGameObjectWithTagWithError(TagConstants.DynamicCanvasFolder).transform
			);
		}
		
		/// <summary>
		/// Loads the first scene found with a name matching the provided <paramref name="scene"/> parameter.
		/// </summary>
		public static async Task LoadSceneAsync(string scene, LoadSceneMode loadMode = LoadSceneMode.Single, object package = null, bool transition = true) {

			if (transition) {
				GetInstance().lastLoadedScene = scene;
				GetInstance().scenePackage = package;

				GameState = GameStatesEnum.Loading;
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

				GameState = GameStatesEnum.Play;
			} else {
				SceneManager.LoadScene(scene, loadMode);
			}
		} 

		// ---
		// Events

		protected virtual void OnBeforeLeaveScene() {
			if (BeforeLeaveScene != null) 
				BeforeLeaveScene(this, EventArgs.Empty);
		}

		// ---
		// GameState

		public static bool IsPaused() => GameState == GameStatesEnum.Pause;
		
		public static bool IsPlaying() => GameState == GameStatesEnum.Play;
		
		public static bool IsInCutscene() => GameState == GameStatesEnum.Cutscene;
		
		public static bool IsLoading() => GameState == GameStatesEnum.Loading;

		public static void PauseGame() {
			GameState = GameStatesEnum.Pause;
		}	

		public static void UnpauseGame() {
			if (IsPaused())
				GameState = GameStatesEnum.Play;
		}

		public static void EnterCutscene() {
			GameState = GameStatesEnum.Cutscene;
		}

		public static void ExitCutscene() {
			if (IsInCutscene())
				GameState = GameStatesEnum.Play;
		}
	}
}