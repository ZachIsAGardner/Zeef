using System;
using UnityEngine;
using UnityEngine.SceneManagement;
// ---
using System.Threading.Tasks;

namespace Zeef.GameManagement
{
	public class SceneLoader : SingleInstance<SceneLoader>
    {	
        /// <summary>
        /// Arguments for the next scene that is loaded.
        /// </summary>
		public static object ScenePackage { get; private set; }
        public SceneTransition SceneTransitionPrefab; 
		public string LastLoadedScene { get; private set; }

        public bool IsTransitioning { get; private set; }
        
		public event EventHandler BeforeLeaveScene;
        
        private SceneTransition sceneTransitionInstance;

		// ---
		// Setup

		protected override void Awake()
        {
			base.Awake();
			DontDestroyOnLoad(gameObject);		
		}

		void Start()
        {
			LastLoadedScene = SceneManager.GetActiveScene().name;
		}

        /// <summary>
        /// Loads the first scene found with a name matching the provided <paramref name="scene"/> parameter.
        /// </summary>
        public static async Task LoadSceneAsync(string scene, object package = null)
        {
            Instance.LastLoadedScene = scene;
            ScenePackage = package;

            Instance.IsTransitioning = true;

            // Transition out
            Instance.sceneTransitionInstance = Instantiate(Instance.SceneTransitionPrefab);
            Instance.sceneTransitionInstance.Out();
             
            while(!Instance.sceneTransitionInstance.DidReachHalfway) {
                await new WaitForUpdate();
            }

            Instance.OnBeforeLeaveScene();
            SceneManager.LoadScene(scene);

            // Transition in
            Instance.sceneTransitionInstance.In();

            Instance.IsTransitioning = false;
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