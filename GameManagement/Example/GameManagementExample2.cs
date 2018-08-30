using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
// ---
using Zeef.Menu;

namespace Zeef.GameManagement {

    public class GameManagementExample2Package {

        public string Name { get; set; }

        public GameManagementExample2Package(string name) {
            Name = name;
        }
    }

	public class GameManagementExample2 : MonoBehaviour {

        [Required]
        [SerializeField] Text textComponent;

        void Start() {
            GameManagementExample2Package package = GameManager.OpenPackage<GameManagementExample2Package>();
            Debug.Log(package);
            Debug.Log(package.Name);
            textComponent.text = package.Name;
        }
	}
}