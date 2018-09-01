using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zeef.Persistence.Example {

	[System.Serializable]
	public class SaveDataExample {
		public int Count { get; set; }

		public SaveDataExample(int count) {
			Count = count;
		}
	}

	public class PersistenceExample : MonoBehaviour {

		[SerializeField] Text textComponent;
		int count;

		void Start() {
			textComponent.text = $"Count: {count}";
		}

		void Update() {
			if (Input.GetKeyDown("a")) { 
				count++;
				textComponent.text = $"Count: {count}";
			}

			if (Input.GetKeyDown("l")) {
				SaveDataExample saveData = SaverLoader.TryLoad<SaveDataExample>(PersistenceManager.FileName);
				if (saveData != null) {
					count = saveData.Count;
					textComponent.text = $"Count: {count}";
				} else {
					count = 0;
					textComponent.text = "No SaveData found.";
				}
			}

			if (Input.GetKeyDown("s"))
				SaverLoader.Save(new SaveDataExample(count), PersistenceManager.FileName);

			if (Input.GetKeyDown("d"))
				SaverLoader.Delete(PersistenceManager.FileName);
			
		}
	}
}
