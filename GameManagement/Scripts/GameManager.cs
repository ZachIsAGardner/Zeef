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

        protected GameStatesEnum gameState = GameStatesEnum.Play;
		public static GameStatesEnum GameState { get { return GetInstance().gameState; } }

		protected string lastLoadedScene;

		public static event EventHandler BeforeLeaveScene;

		public enum GameStatesEnum {
			Play,
			Pause,
			Cutscene,
			Loading,
		}

		// ---
		// Setup

		protected virtual void Awake() {
			base.Awake();
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

		protected virtual void Update() {
			ListenForPause();
		}

		void ListenForPause() {
			if (ControlManager.GetInputDown(ControlManager.Start) && (IsPaused() || IsPlaying()))
				gameState = (IsPlaying()) ? GameStatesEnum.Pause : GameStatesEnum.Play;
		}

		public static GameObject SpawnActor(Vector2 position) {
			GameObject actor =  new GameObject();

			actor.gameObject.transform.parent = Utility.FindGameObjectWithTagWithError(TagConstants.DynamicFolder).transform;
			actor.gameObject.transform.position = position;

			return actor;
		}

		public static GameObject SpawnActor(GameObject prefab, Vector2 position) {
			return Instantiate(
				original: prefab, 
				position: position, 
				rotation: Quaternion.identity, 
				parent: Utility.FindGameObjectWithTagWithError(TagConstants.DynamicFolder).transform
			);
		}

		public static void SpawnCanvasElement(GameObject prefab, Vector3 position) {
			Instantiate(
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

				GetInstance().gameState = GameStatesEnum.Loading;
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

				GetInstance().gameState = GameStatesEnum.Play;
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

		public static bool IsPaused() => GetInstance().gameState == GameStatesEnum.Pause;
		
		public static bool IsPlaying() => GetInstance().gameState == GameStatesEnum.Play;
		
		public static bool IsInCutscene() => GetInstance().gameState == GameStatesEnum.Cutscene;
		
		public static bool IsLoading() => GetInstance().gameState == GameStatesEnum.Loading;
		
		public static void EnterCutscene() {
			GetInstance().gameState = GameStatesEnum.Cutscene;
		}

		public static void ExitCutscene() {
			GetInstance().gameState = GameStatesEnum.Play;
		}
	}
}