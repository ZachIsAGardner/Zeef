using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using Zeef;
using Zeef.Menu;

namespace Zeef.Menu.Example {

	public class TestData {
		public string Name { get; set; }
		public int AmountOfDogs { get; set; }

		public TestData(string name, int amountOfDogs) {
			Name = name;
			AmountOfDogs = amountOfDogs;
		}
	}

	public class Example : MonoBehaviour {

		[Required]
		[SerializeField] AlertUI alertUIPrefab;

		[Required]
		[SerializeField] VerticalMenuSelectUI verticalMenuSelectPrefab;

		[Required]
		[SerializeField] PagedVerticalMenuSelectUI pagedVerticalMenuSelectPrefab;

		[Required]
		[SerializeField] MatrixMenuSelectUI matrixMenuSelectPrefab;

		[Required]
		[SerializeField] HorizontalMenuSelectUI horizontalMenuSelectPrefab;

		[Required]
		[SerializeField] OptionsEnum options;
		
		public enum OptionsEnum {
			Vertical,
			PagedVertical,
			Matrix,
			Horizontal
		}

		async void Start() {
			// Set up example data
			List<TestData> datas = new List<TestData>() {
				new TestData("Pebbles", 2),
				new TestData("Jack", 1),
				new TestData("Zoe", 1),
				new TestData("Mo", 0),
				new TestData("Strange Old Man", 2),
				new TestData("Dog Theif", 501),
				new TestData("Dirty Woman", 7),
				new TestData("Lazy Girl", 0),
				new TestData("Cowboy Man", 0)
			};

			RectTransform canvas = FindObjectOfType<Canvas>().GetComponent<RectTransform>();

			switch (options) {
				case OptionsEnum.Vertical:
					await CreateVerticalAsync(datas, canvas);
					break;
				case OptionsEnum.PagedVertical:
					await CreatePagedVerticalAsync(datas, canvas);
					break;
				case OptionsEnum.Horizontal:
					await CreateHorizontalAsync(datas, canvas);
					break;
				case OptionsEnum.Matrix:
					await CreateMatrixAsync(datas, canvas);
					break;
				default:
					throw new Exception("Invalid option");
			}
		}

		private async Task CreateVerticalAsync(List<TestData> datas, RectTransform parent) {
			// Create models for the list items from our list of test data
			List<MenuItemUIModel> models = datas.Select(d => new MenuItemUIModel(d, d.Name)).ToList();

			// Get selection from user
			TestData selected = (TestData)await VerticalMenuSelectUI
				.Initialize(verticalMenuSelectPrefab, parent, models)
				.GetSelectionAsync();

			// Broadcast selection
			await BroadcastSelectionAsync(selected, parent);
		}

		private async Task CreatePagedVerticalAsync(List<TestData> datas, RectTransform parent) {
			List<MenuItemUIModel> models = datas.Select(d => new MenuItemUIModel(d, d.Name)).ToList();

			TestData selected = (TestData)await PagedVerticalMenuSelectUI
				.Initialize(pagedVerticalMenuSelectPrefab, parent, models, 5)
				.GetSelectionAsync();

			await BroadcastSelectionAsync(selected, parent);
		}

		private async Task CreateHorizontalAsync(List<TestData> datas, RectTransform parent) {
			List<MenuItemUIModel> models = datas.Select(d => new MenuItemUIModel(d, d.Name)).ToList();

			TestData selected = (TestData)await HorizontalMenuSelectUI
				.Initialize(horizontalMenuSelectPrefab, parent, models)
				.GetSelectionAsync();

			await BroadcastSelectionAsync(selected, parent);
		}

		private async Task CreateMatrixAsync(List<TestData> datas, RectTransform parent) {

			List<List<MenuItemUIModel>> models = new List<List<MenuItemUIModel>>();
			models.Add(datas.TryGetRange(0, 5).Select(d => new MenuItemUIModel(d, d.Name)).ToList());
			models.Add(datas.TryGetRange(5, 5).Select(d => new MenuItemUIModel(d, d.Name)).ToList());
			models.Add(datas.TryGetRange(10, 5).Select(d => new MenuItemUIModel(d, d.Name)).ToList());

			TestData selected = (TestData)await MatrixMenuSelectUI
				.Initialize(matrixMenuSelectPrefab, parent, models)
				.GetSelectionAsync();

			await BroadcastSelectionAsync(selected, parent);
		}

		private async Task BroadcastSelectionAsync(TestData selected, RectTransform parent) {
			if (selected != null) {
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
