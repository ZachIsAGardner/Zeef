using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional {

	[RequireComponent(typeof(BoxCollider2D))]
	public class HitBox2D : MonoBehaviour {

		public float Damage = 2;

		[HideInInspector] public bool Parented;
		[HideInInspector] public GameObject Owner;
		private Vector3 offset;

		// ---

		void Start() {
			if (Owner) offset = transform.position - Owner.transform.position;
			else if (transform.parent) Owner = transform.parent.gameObject;
			else Owner = gameObject;	
		}

		void Update() {
			if (Parented && Owner) 
				transform.position = Owner.transform.position + offset;	
		}

		public void LandedHit(GameObject victim) {
			throw new NotImplementedException();

			MovingObject2D attacker = Owner.GetComponent<MovingObject2D>();

			// if (attacker) attacker.LandedHit(victim);	
		}

		void OnTriggerEnter2D(Collider2D col) {
			if (col.tag == "Bounce") LandedHit(col.gameObject);	
		}
	}
}
