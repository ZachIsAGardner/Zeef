using UnityEngine;

namespace Zeef.TwoDimensional {

    public class MovingObject2DTrigger : MonoBehaviour {

        [SerializeField] private MovingObject2D movingObject;

        void OnTriggerEnter2D(Collider2D col) {
            if (col.tag == TagConstants.Player) movingObject.Active = true; 
        }
    }
}