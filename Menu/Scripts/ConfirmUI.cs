using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Zeef.Menu {

    // Are you sure about the thing?
    // |Yes| No
    public class ConfirmUI : MonoBehaviour {

        [SerializeField] Image imageComponent;
        [SerializeField] Text textComponent;

        public static ConfirmUI Initialize(ConfirmUI prefab, GameObject parent, string message) {
            ConfirmUI instance = Instantiate(prefab, parent.transform);

            instance.textComponent.text = message;

            return instance;
        }

        public Task<bool> GetSelection() {
            throw new NotImplementedException();
        }
    }
}