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

		// ---

		// Special actions are available only in dev mode
        [SerializeField] private bool isDev = true;
		public static bool IsDev { get { return gameManager.isDev; } }

        [SerializeField] private string saveDataFileName = "saveData.dat";
		public static string SaveDataFileName { get { return gameManager.saveDataFileName; }}


        [SerializeField] private string entryScene = "_Entry";
		public static string EntryScene { get { return gameManager.entryScene; }}

		[SerializeField] private Canvas canvas;
		public static Canvas Canvas { get { return gameManager.canvas; }}

		[Range(0, 5)]
		[SerializeField] private float transitionTime = 1;

		[SerializeField] Color transitionColor = Color.black;

		[SerializeField] private SceneInfo firstScene;

		// ---

		private object scenePackage { get; set; }

        protected GameStatesEnum gameState = GameStatesEnum.Play;
		public static GameStatesEnum GameState { get { return gameManager.gameState; } }

		protected SceneInfo lastLoadedSceneInfo;

		public enum GameStatesEnum {
			Play,
			Pause,
			Cutscene,
			Loading,
			Fight  
		}

		// ---
		// Setup
		
		protected virtual void Awake() {
			if (gameManager != null) throw new Exception("Only one GameManager may exist at a time."); 
			gameManager = this;	
			DontDestroyOnLoad(gameObject);

			ReferenceCheck.EnsureNotNull(this, new ReferenceCheck(typeof(Canvas), canvas));

			Application.targetFrameRate = 60;			
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		protected virtual async void Start() {
			// Start game from entry scene or just spawn player
			if (SceneManager.GetActiveScene().name == entryScene) {
				await LoadSceneAsync(firstScene);
			} else {
				SceneInfo info = new SceneInfo(SceneManager.GetActiveScene().name, FacingsEnum.Right, 0, LoadSceneMode.Single);
				// StartCoroutine(SpawnPlayer(info));
				lastLoadedSceneInfo = info;
			}
		}

		protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode) { }

		// ---
		// Running

		protected virtual void Update() {
			ListenForPause();
		}

		void ListenForPause() {
			if (Input.GetButtonDown("Submit") && (IsPaused() || IsPlaying()))
				gameState = (IsPlaying()) 
					? GameStatesEnum.Pause
					: GameStatesEnum.Play;
		}

		// TODO: revisit this
		public static GameObject SpawnEntity(GameObject prefab, Vector3 position) {
			GameObject result = Container.PlaceCopyInContainer(prefab, ContainersEnum.Entities);
			result.transform.position = position;
			return result;
		}

		public static GameObject SpawnEntity(EntitiesEnum id, Vector3 position) {
			GameObject result = Container.PlaceCopyInContainer(GameContent.GetEntity(id), ContainersEnum.Entities);
			result.transform.position = position;
			return result;
		}

		// ---
		// Loading

		public static T OpenPackage<T>() where T : class {
			return (T)gameManager.scenePackage;
		}

		public static async Task LoadSceneAsync(SceneInfo info, object scenePackage) {
			gameManager.scenePackage = scenePackage;
			await LoadSceneAsync(info);
		}

		public static async Task LoadSceneAsync(SceneInfo info) {
			gameManager.lastLoadedSceneInfo = info;

			gameManager.gameState = GameStatesEnum.Loading;
			await new WaitForUpdate();

			ScreenTransition screenTransition = ScreenTransition.Initialize(
				gameManager.canvas.gameObject, 
				gameManager.transitionColor
			);

			await screenTransition.FadeOutAsync(gameManager.transitionTime);

			SceneManager.LoadScene(info.Scene, info.LoadMode);
			// if (info.LoadMode != LoadSceneMode.Additive) StartCoroutine(SpawnPlayer(info));

			await screenTransition.FadeInAsync(gameManager.transitionTime);

			Destroy(screenTransition.gameObject);

			gameManager.gameState = GameStatesEnum.Play;
		} 

		// ---
		// GameState

		public static bool IsPaused() => gameManager.gameState == GameStatesEnum.Pause;
		
		public static bool IsPlaying() => gameManager.gameState == GameStatesEnum.Play;
		
		public static bool IsInCutscene() => gameManager.gameState == GameStatesEnum.Cutscene;
		
		public static bool IsLoading() => gameManager.gameState == GameStatesEnum.Loading;
		
		public static bool IsInFight() => gameManager.gameState == GameStatesEnum.Fight;
		
		public static void EnterCutscene() {
			gameManager.gameState = GameStatesEnum.Cutscene;
		}
		public static void ExitCutscene() {
			gameManager.gameState = GameStatesEnum.Play;
		}
	}
}