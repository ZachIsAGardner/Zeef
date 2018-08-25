using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ---
using Zeef.Menu;

namespace Zeef.GameManagement {

	public class GameManagementExample1Package {

        public string Name { get; set; }

        public GameManagementExample1Package(string name) {
            Name = name;
        }
    }

	public class GameManagementExample1 : MonoBehaviour {

		[Required]
		[SerializeField] VerticalMenuSelectUI verticalMenuSelectUI;

		[Required]
		[SerializeField] string sceneToLoad;

		private async void Start() {

			// Find canvas
			Canvas canvas = GameObject
				.FindGameObjectsWithTag(TagsConstant.SceneCanvas)
				.First(g => g.GetComponent<Canvas>() != null)
				.GetComponent<Canvas>();

			// Create Menu
			await VerticalMenuSelectUI
				.Initialize(
					verticalMenuSelectUI, 
					canvas.GetComponent<RectTransform>(), 
					new List<MenuItemUIModel>() { 
						new MenuItemUIModel(0, "Load Scene 2")
					})
				.GetSelectionAsync();
			
			await GameManager.LoadSceneAsync(
				new SceneInfo(sceneToLoad),
				new GameManagementExample2Package("Hi")
			);
		}
	}
}
