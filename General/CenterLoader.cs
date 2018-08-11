using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zeef {

	public class CenterLoader : MonoBehaviour {
		
		[SerializeField] private string centerScene = "_Center";

		private static bool loaded;

		public void Awake() {	
			// Load center partial if not yet loaded
			if (!loaded) {
				SceneManager.LoadScene(centerScene, LoadSceneMode.Additive);
				loaded = true;
			}
		}
	}
}
