using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Zeef.Menu {

    public class AlertUI : MonoBehaviour {

        [SerializeField] Image imageComponent;
        [SerializeField] Text textComponent;

        public static AlertUI Initialize(AlertUI prefab, GameObject parent, string message) {
            AlertUI instance = Instantiate(prefab, parent.transform);

            instance.textComponent.text = message;

            return instance;
        }

        public async Task WaitForDismissalAsync() {
            await ControlManager.WaitForInputPressedAsync(ControlManager.Accept);
            Destroy(gameObject);
        }
    }
}