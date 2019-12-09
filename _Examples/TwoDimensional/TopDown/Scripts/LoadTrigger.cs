using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
// ---
using Zeef.GameManagement;

namespace Zeef.TwoDimensional.Example {

    [RequireComponent (typeof(BoxCollider2D))]
    public class LoadTrigger : MonoBehaviour {

        [Required]
        [SerializeField] private string scene;

        [SerializeField] private int spawnID;

        private async void OnTriggerEnter2D(Collider2D col) {
            if (col.tag == TagConstant.Player)
                await GameManager.LoadSceneAsync(
                    scene: scene,
                    package: new SceneSetupPackage(spawnID)
                );
        }
    }
}
