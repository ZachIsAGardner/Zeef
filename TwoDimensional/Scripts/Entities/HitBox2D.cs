using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

	[RequireComponent(typeof(BoxCollider2D))]
	public class HitBox2D : MonoBehaviour {

		[SerializeField] int damage = 1;
		public int Damage { get { return damage; } }

		public MovingObject2D Owner { get; private set; }
		// Parented hitboxes are attached to owner
		// ex)
		// 1) Sword swing would be parented
		// 2) Projectile would not be parented, but 
		// we still want to know who the owner is
		private bool parented;

		// ---

		public static HitBox2D Initialize(MovingObject2D owner, int damage, Vector2 position, bool parented) {
			HitBox2D instance = GameManager.SpawnActor(position).GetComponentWithError<HitBox2D>();

			if (parented) instance.transform.parent = owner.transform;
			instance.transform.position = position;
			instance.damage = damage;

			return instance;
		}

		public void LandedHit(GameObject victim) {
			// owner.LandedHit(victim);
		}
	}
}
