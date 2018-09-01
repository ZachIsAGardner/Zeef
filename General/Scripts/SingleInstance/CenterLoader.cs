using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zeef {

	public class CenterLoader : MonoBehaviour {
		
		[Required]
		[SerializeField] private GameObject centerPrefab;

		private static bool loaded;

		public void Awake() {	
			if (!loaded) {
				Instantiate(centerPrefab);
				loaded = true;
			}
		}
	}
}
