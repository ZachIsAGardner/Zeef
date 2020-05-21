using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
// ---
using Zeef.Menu;

namespace Zeef.GameManagement.Example
{
	public class GameManagementExample1Package
    {
        public string Name { get; set; }

        public GameManagementExample1Package(string name) {
            Name = name;
        }
    }

	public class GameManagementExample1 : MonoBehaviour
    {
		[Required]
		[SerializeField] string sceneToLoad;

		private async void Start()
        {
			await ControlManager.WaitForAnyInputAsync();
			
			await SceneLoader.LoadSceneAsync(
				scene: sceneToLoad,
				package: new GameManagementExample2Package("Hi")
			);
		}
	}
}
