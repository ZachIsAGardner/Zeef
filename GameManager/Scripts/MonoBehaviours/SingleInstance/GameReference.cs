using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zeef.GameManagement {

    // 'References' hold the actual game object and such. References should be the single source for these kinds of gameobjects'
    // Reference GameObjects never change their data at runtime
    [RequireComponent(typeof (SingleInstanceChild))]
    public class GameReference : MonoBehaviour {

        [SerializeField] EntityObjectsContainer entityObjects;
        [SerializeField] PromptObjectsContainer promptObjects;

        public static GameReference Main() {
            return SingleInstance.Main().GetComponentInChildren<GameReference>();
        }

        public GameObject GetEntity(EntitiesEnum id) {
            return entityObjects.entities.First(e => e.id == id).prefab;
        }

        public GameObject GetPrompt(PromptsEnum id) {
            return promptObjects.prompts.First(p => p.id == id).prefab;
		}
    }
}