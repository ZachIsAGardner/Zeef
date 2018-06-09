using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional {

	[RequireComponent(typeof(BoxCollider2D))]
	public class HitBox : MonoBehaviour 
	{
		public float damage = 2;

		Vector3 offset;
		[HideInInspector]
		public bool parented;
		[HideInInspector]
		public GameObject owner;
		[HideInInspector]
		public bool landedHit;

		void Start() {
			if (owner) {
				offset = transform.position - owner.transform.position;
			} else if (transform.parent) {
				owner = transform.parent.gameObject;
			} else {
				owner = gameObject;
			}
		}

		void Update() {
			if (parented && owner) {
				transform.position = owner.transform.position + offset;
			}
		}

		public void LandedHit(GameObject victim) {
			MovingObject attacker = owner.GetComponent<MovingObject>();
			landedHit = true;
			if (attacker) {
				attacker.LandedHit(victim);
			}
		}

		void OnTriggerEnter2D(Collider2D col) {
			if (col.tag == "Bounce") {
				LandedHit(col.gameObject);
			}
		}

	}

}
