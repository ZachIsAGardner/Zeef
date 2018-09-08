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
		public bool Parented { get; private set; }

		// ---

		public static HitBox2D Initialize(MovingObject2D owner, int damage, Vector2 position, Vector2 size, bool Parented) {
			HitBox2D instance = GameManager.SpawnActor(position)
				.AddComponent<DrawBoxCollider2D>().gameObject
				.AddComponent<HitBox2D>();

			instance.GetComponentWithError<BoxCollider2D>().isTrigger = true;

			if (Parented) instance.transform.parent = owner.transform;
			instance.transform.position = position;
			instance.transform.localScale = size;
			instance.Owner = owner;
			instance.damage = damage;

			return instance;
		}

		public void LandedHit(GameObject victim) {
			// owner.LandedHit(victim);
		}
	}
}
