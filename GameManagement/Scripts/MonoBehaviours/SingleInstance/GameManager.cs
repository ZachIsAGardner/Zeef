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

	public class GameManager : MonoBehaviour {

		private static GameManager gameManager;
		private static GameManager GetGameManager() { 
			if (gameManager == null) {
				throw new Exception("No GameManager exists.");
			} else { 
				return gameManager;
			}
		}

		// Special actions are available only in dev mode
        [SerializeField] private bool isDev = true;
		public static bool IsDev { get { return GetGameManager().isDev; } }

		[Required]
        [SerializeField] private string saveDataFileName = "saveData.dat";
		public static string SaveDataFileName { get { return GetGameManager().saveDataFileName; }}

		[Required]
		[SerializeField] private Canvas canvas;
		public static Canvas Canvas { get { return GetGameManager().canvas; }}

		[Range(0, 5)]
		[SerializeField] private float transitionTime = 1;

		[SerializeField] Color transitionColor = Color.black;

		// ---

		private object scenePackage { get; set; }

        protected GameStatesEnum gameState = GameStatesEnum.Play;
		public static GameStatesEnum GameState { get { return GetGameManager().gameState; } }

		protected string lastLoadedScene;

		public enum GameStatesEnum {
			Play,
			Pause,
			Cutscene,
			Loading,
		}

		// ---
		// Setup

		protected virtual void Awake() {
			if (gameManager != null) throw new Exception("Only one GameManager may exist at a time."); 
			gameManager = this;	
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

		public static void SpawnActor(GameObject prefab, Vector3 position) {
			Instantiate(
				original: prefab, 
				position: position, 
				rotation: Quaternion.identity, 
				parent: Utility.FindGameObjectWithTagWithError(TagsConstant.DynamicFolder).transform
			);
		}

		public static void SpawnCanvasElement(GameObject prefab, Vector3 position) {
			Instantiate(
				original: prefab, 
				position: position, 
				rotation: Quaternion.identity, 
				parent: Utility.FindGameObjectWithTagWithError(TagsConstant.DynamicCanvasFolder).transform
			);
		}

		// ---
		// Loading

		public static T OpenPackage<T>() where T : class =>	(T)GetGameManager().scenePackage;
		
		public static async Task LoadSceneAsync(string scene, LoadSceneMode loadMode = LoadSceneMode.Single, object package = null) {
			GetGameManager().lastLoadedScene = scene;
			GetGameManager().scenePackage = package;

			GetGameManager().gameState = GameStatesEnum.Loading;
			await new WaitForUpdate();

			ScreenTransition screenTransition = ScreenTransition.Initialize(
				GetGameManager().canvas.gameObject, 
				GetGameManager().transitionColor
			);

			await screenTransition.FadeOutAsync(GetGameManager().transitionTime);

			SceneManager.LoadScene(scene, loadMode);

			await screenTransition.FadeInAsync(GetGameManager().transitionTime);

			Destroy(screenTransition.gameObject);

			GetGameManager().gameState = GameStatesEnum.Play;
		} 

		// ---
		// GameState

		public static bool IsPaused() => GetGameManager().gameState == GameStatesEnum.Pause;
		
		public static bool IsPlaying() => GetGameManager().gameState == GameStatesEnum.Play;
		
		public static bool IsInCutscene() => GetGameManager().gameState == GameStatesEnum.Cutscene;
		
		public static bool IsLoading() => GetGameManager().gameState == GameStatesEnum.Loading;
		
		public static void EnterCutscene() {
			GetGameManager().gameState = GameStatesEnum.Cutscene;
		}

		public static void ExitCutscene() {
			GetGameManager().gameState = GameStatesEnum.Play;
		}
	}
}