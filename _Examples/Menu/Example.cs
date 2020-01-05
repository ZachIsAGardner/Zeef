using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace Zeef.Menu.Example
{
	public class TestData
    {
		public string Name { get; set; }
		public int AmountOfDogs { get; set; }

		public TestData(string name, int amountOfDogs)
        {
			Name = name;
			AmountOfDogs = amountOfDogs;
		}
	}

	public class Example : MonoBehaviour
    {
		[Required]
		[SerializeField] AlertUI alertUIPrefab;

        [Required]
        [SerializeField] PagedVerticalMenuSelectUI pagedVerticalMenuSelectPrefab;

		async void Start()
        {
			// Set up example data
			List<TestData> datas = new List<TestData>()
            {
				new TestData("Pebbles", 2),
				new TestData("Jack", 1),
				new TestData("Zoe", 1),
				new TestData("Dog Theif", 501),
				new TestData("Dirty Woman", 7),
				new TestData("Lazy Girl", 0),
				new TestData("Cowboy Man", 0)
			};

			RectTransform canvas = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
            
			await CreatePagedVerticalAsync(datas, canvas);
		}

		private async Task CreatePagedVerticalAsync(List<TestData> datas, RectTransform parent)
        {
			List<MenuItemUIModel> models = datas.Select(d => new MenuItemUIModel(d, d.Name)).ToList();

			TestData selected = (TestData)await PagedVerticalMenuSelectUI
				.Initialize(pagedVerticalMenuSelectPrefab, parent, models, 5)
				.GetSelectionAsync();

			await BroadcastSelectionAsync(selected, parent);
		}

		private async Task BroadcastSelectionAsync(TestData selected, RectTransform parent)
        {
			if (selected != null)
            {
				await AlertUI
					.Initialize(alertUIPrefab, parent.gameObject, $"You have selected {selected.Name}. They have {selected.AmountOfDogs} dogs.")
					.WaitForDismissalAsync();
			} else {
				await AlertUI
					.Initialize(alertUIPrefab, parent.gameObject, "You have cancelled the selection.")
					.WaitForDismissalAsync();
			}
		}
	}
}
