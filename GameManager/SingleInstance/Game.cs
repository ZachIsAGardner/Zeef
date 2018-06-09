using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
// ---
using Zeef.Sound;

namespace Zeef.GameManager 
{
	[RequireComponent (typeof(AudioSource))]
	[RequireComponent (typeof(SingleInstanceChild))]
	public class Game : MonoBehaviour 
	{
		// Pixel per unit
		public int ppu = 100;
		[SerializeField] string entryScene = "Entry";

		[SerializeField] protected string saveDataFileName = "saveData.dat";
		
		public bool dev = true;
		public GameState gameState = GameState.Play;

		public SceneInfo firstScene;

		// References
		RuntimePlatform platform;
		protected GameDB gameDB;
		GameReference gameRef;
		protected AudioPlayer songPlayer;

		protected SceneInfo lastLoadedSceneInfo;

		public enum GameState 
		{
			Play,
			Pause,
			Cutscene,
			Loading,
			Fight  
		}
		
		public static Game Main() {
			return SingleInstance.Main().GetComponentInChildren<Game>();
		}

		public static Canvas Canvas() {
			return SingleInstance.Main().GetComponentInChildren<Canvas>();
		}
		
		#region Setup


		// ---

		void Start() {
			Setup();
		}

		protected virtual void Setup() {
			Application.targetFrameRate = 60;			
				
			GetComponents();
			GetPlatform();

			SceneManager.sceneLoaded += OnSceneLoaded;

			// Start game from entry scene or just spawn player
			if (SceneManager.GetActiveScene().name == entryScene) {
				LoadScene(firstScene);
			} else {
				SceneInfo info = new SceneInfo(SceneManager.GetActiveScene().name);
				StartCoroutine(SpawnPlayer(info));
				lastLoadedSceneInfo = info;
			}
		}

		protected virtual void GetComponents() {
			gameDB = GameDB.Main();
			gameRef = GameReference.Main();
			songPlayer = AudioPlayer.Main();
		}

		void GetPlatform() {
			platform = Application.platform;
		}

		void PrintPlatform() {
			print(platform);
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
			if (Input.GetButtonDown("Submit") && (Paused() || Playing())) { 

				gameState = (Playing()) 
					? GameState.Pause
					: GameState.Play;

				// if (Paused()) {
				// 	OpenMenu();
				// } else {
				// 	CloseMenu();
				// }
			}
		}

		public GameObject SpawnEntity(GameObject go, Vector3 position) {
			GameObject result = Container.PlaceCopyInContainer(go, ContainerID.Entities);
			result.transform.position = position;
			return result;
		}
		public GameObject SpawnEntity(EntityID id, Vector3 position) {
			GameObject go = gameRef.GetEntity(id);
			GameObject result = Container.PlaceCopyInContainer(go, ContainerID.Entities);
			result.transform.position = position;
			return result;
		}

		public string SaveDataFileName() {
			return saveDataFileName;
		}

		#endregion
		
		#region LevelState

		public bool Paused() {
			return gameState == GameState.Pause;
		}
		public bool Playing() {
			return gameState == GameState.Play;
		}
		public bool InCutscene() {
			return gameState == GameState.Cutscene;
		}
		public bool Loading() {
			return gameState == GameState.Loading;
		}
		public bool InFight() {
			return gameState == GameState.Fight;
		}

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

			yield return StartCoroutine(ScreenTransition.Appear(screen, 0.25f));
			yield return StartCoroutine(LoadAction(info));
			yield return StartCoroutine(ScreenTransition.Fade(screen, 0.25f));
			Destroy(screen.gameObject);
			EndLoadAction();
		}

		protected virtual IEnumerator LoadAction(SceneInfo info) {
			SceneManager.LoadScene(info.scene, info.loadMode);
			yield return null;
			if (info.loadMode != LoadSceneMode.Additive) StartCoroutine(SpawnPlayer(info));
		}

		protected virtual IEnumerator SpawnPlayer(SceneInfo info) {	
			// Wait one frame after loading
			yield return null;

			Spawn spawn = FindObjectsOfType<Spawn>().FirstOrDefault(s => s.ID == info.spawn);
			if (spawn == null) throw new Exception("A spawn with the requested ID does not exist");

			SpawnEntity(gameDB.player, spawn.transform.position);
		}

		protected virtual void EndLoadAction() {
			gameState = GameState.Play;
		}

		#endregion

	}

}