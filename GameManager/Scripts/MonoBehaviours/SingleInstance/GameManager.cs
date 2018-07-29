using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
// ---
using Zeef.Sound;

namespace Zeef.GameManagement {

	[RequireComponent (typeof(SingleInstanceChild))]
	public class GameManager : MonoBehaviour {

		// Special actions are available in dev mode
        [SerializeField] private bool isDev = true;
		public bool IsDev { get { return isDev; } }

        // Pixel per unit
        [SerializeField] private int ppu = 100;
		public int PPU { get { return ppu; } }

        [SerializeField] private string saveDataFileName = "saveData.dat";
		public string SaveDataFileName { get { return saveDataFileName; }}

        [SerializeField] private string entryScene = "_Entry";
		public string EntryScene { get { return entryScene; }}

		[SerializeField] private SceneInfo firstScene;

        protected GameState gameState = GameState.Play;

		protected SceneInfo lastLoadedSceneInfo;

		public enum GameState {
			Play,
			Pause,
			Cutscene,
			Loading,
			Fight  
		}
		
		public static GameManager Main() => SingleInstance.Main().GetComponentInChildren<GameManager>();
		
		public static Canvas Canvas() => SingleInstance.Main().GetComponentInChildren<Canvas>();
		
		#region Setup

		void Start() {
			Setup();
			SceneSetup();
		}

		protected virtual void Setup() {
			Application.targetFrameRate = 60;			
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		protected virtual void SceneSetup() {
			// Start game from entry scene or just spawn player
			if (SceneManager.GetActiveScene().name == entryScene) {
				LoadScene(firstScene);
			} else {
				SceneInfo info = new SceneInfo(SceneManager.GetActiveScene().name, FacingsEnum.Right, 0, LoadSceneMode.Single);
				StartCoroutine(SpawnPlayer(info));
				lastLoadedSceneInfo = info;
			}
		}

		protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
			// TODO
		}

		#endregion

		#region Running

		protected virtual void Update() {
			ListenForPause();
		}

		void ListenForPause() {
			if (Input.GetButtonDown("Submit") && (IsPaused() || IsPlaying())) { 
				gameState = (IsPlaying()) 
					? GameState.Pause
					: GameState.Play;

			}
		}

		public GameObject SpawnEntity(GameObject go, Vector3 position) {
			GameObject result = Container.PlaceCopyInContainer(go, ContainersEnum.Entities);
			result.transform.position = position;
			return result;
		}
		public GameObject SpawnEntity(EntitiesEnum id, Vector3 position) {
			GameObject go = GameReference.Main().GetEntity(id);
			GameObject result = Container.PlaceCopyInContainer(go, ContainersEnum.Entities);
			result.transform.position = position;
			return result;
		}

		#endregion
		
		#region LevelState

		public bool IsPaused() => gameState == GameState.Pause;
		
		public bool IsPlaying() => gameState == GameState.Play;
		
		public bool InCutscene() => gameState == GameState.Cutscene;
		
		public bool IsLoading() => gameState == GameState.Loading;
		
		public bool InFight() => gameState == GameState.Fight;
		
		public void EnterCutscene() {
			gameState = GameState.Cutscene;
		}
		public void ExitCutscene() {
			gameState = GameState.Play;
		}

		#endregion

		#region Loading

		public virtual void LoadScene(SceneInfo info) {
			lastLoadedSceneInfo = info;
			StartCoroutine(RunLoadScene(info));
		} 

		protected IEnumerator RunLoadScene(SceneInfo info) {
			gameState = GameState.Loading;
			yield return null;

			Image screen = ScreenTransition.CreateLoadScreen();

			yield return StartCoroutine(ScreenTransition.FadeOut(screen, 0.25f));
			// Do this once screen is black
			yield return StartCoroutine(LoadAction(info));

			yield return StartCoroutine(ScreenTransition.FadeIn(screen, 0.25f));

			Destroy(screen.gameObject);

			EndLoadAction();
		}

		protected virtual IEnumerator LoadAction(SceneInfo info) {
			SceneManager.LoadScene(info.Scene, info.LoadMode);
			yield return null;
			if (info.LoadMode != LoadSceneMode.Additive) StartCoroutine(SpawnPlayer(info));
		}

		protected virtual IEnumerator SpawnPlayer(SceneInfo info) {	
			// Wait one frame after loading
			yield return null;

			Spawn spawn = FindObjectsOfType<Spawn>().FirstOrDefault(s => s.ID == info.Spawn);
			if (spawn == null) throw new Exception("A spawn with the requested ID does not exist");

			SpawnEntity(GameDB.Main().PlayerID, spawn.transform.position);
		}

		protected virtual void EndLoadAction() {
			gameState = GameState.Play;
		}

		#endregion
	}
}