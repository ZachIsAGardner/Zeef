using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

	public class LandedHitArgs {
		public GameObject Victim { get; private set; }

		public LandedHitArgs(GameObject victim) {
			Victim = victim;
		}
	}

	[RequireComponent(typeof(BoxCollider2D))]
	public class HitBox2D : MonoBehaviour {

		[SerializeField] int damage = 1;
		public int Damage { get { return damage; } }

		public GameObject Owner { get; private set; }
		// Parented hitboxes are attached to owner
		// ex)
		// 1) Sword swing would be parented
		// 2) Projectile would not be parented, but 
		// we still want to know who the owner is
		public bool Parented { get; private set; }

		public event EventHandler<LandedHitArgs> AfterLandedHit;

		// ---

		public static HitBox2D Initialize(HitBox2D prefab, GameObject owner, Vector2 position, Vector2 size, bool Parented) {
			HitBox2D instance = GameManager.SpawnActor(prefab.gameObject, position).GetComponent<HitBox2D>();

			if (Parented) instance.transform.parent = owner.transform;
			instance.transform.position = position;
			instance.transform.localScale = size;
			instance.Owner = owner;

			return instance;
		}

		public static HitBox2D Initialize(GameObject owner, int damage, Vector2 position, Vector2 size, bool Parented) {
			HitBox2D instance = GameManager.SpawnActor(position)
				.AddComponent<DrawBoxCollider2D>().gameObject
				.AddComponent<HitBox2D>();

			instance.name = "HitBox";
			instance.GetComponentWithError<BoxCollider2D>().isTrigger = true;

			if (Parented) instance.transform.parent = owner.transform;
			instance.transform.position = position;
			instance.transform.localScale = size;
			instance.Owner = owner;
			instance.damage = damage;

			return instance;
		}

		// ---
		// Events

		public virtual void OnAfterLandedHit(GameObject victim) {
			if (AfterLandedHit != null) 
				AfterLandedHit(this, new LandedHitArgs(victim));
		}
	}
}
