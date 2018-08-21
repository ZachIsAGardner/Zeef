using UnityEngine;

namespace Zeef.TwoDimensional {

    public class MovingObjectTrigger : MonoBehaviour {

        [SerializeField] private MovingObject movingObject;

        void OnTriggerEnter2D(Collider2D col) {
            if (col.tag == "Player") movingObject.Active = true; 
        }
    }
}