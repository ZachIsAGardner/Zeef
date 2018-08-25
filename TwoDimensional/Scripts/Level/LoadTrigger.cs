using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
// ---
using Zeef.TwoDimensional;
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

    [RequireComponent (typeof(BoxCollider2D))]
    public class LoadTrigger : InteractableObject {

        [SerializeField] private string scene;

        protected override async Task TriggerActionAsync() {
            await GameManager.LoadSceneAsync(scene);
        }
    }
}
