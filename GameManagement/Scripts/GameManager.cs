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

		// Special actions are available only in dev mode
        [SerializeField] private bool isDev = true;
		public static bool IsDev { get { return GetInstance().isDev; } }

		[Required]
		[SerializeField] private Canvas canvas;
		public static Canvas Canvas { get { return GetInstance().canvas; }}

		[Range(0, 5)]
		[SerializeField] private float transitionTime = 1;

		[SerializeField] Color transitionColor = Color.black;

		// -

		private object scenePackage { get; set; }

		public static GameStatesEnum GameState { get; private set; }

		protected string lastLoadedScene;

		public static event EventHandler BeforeLeaveScene;

		// ---
		// Setup

		protected virtual void Awake() {
			base.Awake();
			GameState = GameStatesEnum.Play;
			DontDestroyOnLoad(gameObject);

			Application.targetFrameRate = 60;			
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		void Start() {
			lastLoadedScene = SceneManager.GetActiveScene().name;
		}

		protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode) { }

		// ---
		// Running

		public static GameObject SpawnActor(Vector2 position) {
			GameObject actor =  new GameObject();

			actor.gameObject.transform.parent = Utility.FindGameObjectWithTagWithError(TagConstants.DynamicFolder).transform;
			actor.gameObject.transform.position = position;

			return actor;
		}

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

		// ---
		// Loading

		public static object OpenPackage() => GetInstance().scenePackage;
		
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