using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ---
using Zeef.Menu;

namespace Zeef.GameManagement {

	public class Example : MonoBehaviour {

		[SerializeField] VerticalMenuSelectUI verticalMenuSelectUI;
		[SerializeField] string sceneToLoad;

		async void Start() {
			Canvas canvas = GameObject
				.FindGameObjectsWithTag(TagsConstant.SceneCanvas)
				.First(g => g.GetComponent<Canvas>() != null)
				.GetComponent<Canvas>();

			await VerticalMenuSelectUI
				.Initialize(
					verticalMenuSelectUI, 
					canvas.GetComponent<RectTransform>(), 
					new List<MenuItemUIModel>() { 
						new MenuItemUIModel(0, "Load Scene 2")
					})
				.GetSelectionAsync();

			await GameManager.LoadSceneAsync(new SceneInfo(sceneToLoad));
		}
	}
}
