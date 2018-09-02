using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zeef.GameManagement;

namespace Zeef.TwoDimensional.Example {

	public class SceneSetupPackage {

		public int SpawnID { get; set; }

		public SceneSetupPackage(int spawnID) {
			SpawnID = spawnID;
		}
	}

	public class SceneSetup : MonoBehaviour {

		void Start() {
			SceneSetupPackage package = GameManager.OpenPackage<SceneSetupPackage>();
			
			Transform spawn = (package != null) 
				? FindObjectsOfType<Spawn>().First(s => s.ID == package.SpawnID).transform
				: FindObjectsOfType<Spawn>().First().transform;

			GameManager.SpawnActor(ExampleContent.PlayerPrefab, spawn.position);
		}
	}
}