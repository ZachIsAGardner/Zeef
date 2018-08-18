using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Zeef.Menu {

    public class ConfirmUI : MonoBehaviour {

        [SerializeField] Image imageComponent;
        [SerializeField] Text textComponent;

        public static ConfirmUI Initialize(ConfirmUI prefab, GameObject parent, string message) {
            ConfirmUI instance = Instantiate(prefab, parent.transform);

            instance.textComponent.text = message;

            return instance;
        }

        public async Task<bool> GetSelection() {
            throw new NotImplementedException();
        }
    }
}