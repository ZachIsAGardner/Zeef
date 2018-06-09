using UnityEngine;

namespace Zeef.TwoDimensional {

    public class EnemyTrigger : MonoBehaviour {

        void OnTriggerEnter2D(Collider2D col) {
            if (col.tag == "Player") {
                GetComponentInParent<MovingObject>().Activate();
            } 
        }

    }

}