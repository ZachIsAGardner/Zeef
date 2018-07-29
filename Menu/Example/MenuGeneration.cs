using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Menu {

	// Example
	public class MenuGeneration : MonoBehaviour {

		[SerializeField] private GameObject listParentPrefab;
		[SerializeField] private GameObject listParent2Prefab;
		[SerializeField] private GameObject listItemPrefab;

		private List<UIElement> items;

		void Start() {
			Generate(new List<string>(){ "Close", "Options", "Quit Game" }, ListItemOptionsEnum.Vertical);
		}

		void Generate(List<string> content, ListItemOptionsEnum option) {
			Canvas canvas = FindObjectOfType<Canvas>();

			items = new List<UIElement>();

			int i = 0;
			foreach (string text in content) {
				items.Add(ListItemUIElement.Initialize(listItemPrefab, listParentPrefab.transform, i, 5, option, text));
				i++;
			}
			
			StartCoroutine(MenuInput.SelectFromVerticalListCoroutine(items, this));
		}
	}
}
