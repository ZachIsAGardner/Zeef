﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeef.GameManagement;

namespace Zeef.TwoDimensional 
{
	public class LandedHitArgs 
	{
		public GameObject Victim { get; private set; }

		public LandedHitArgs(GameObject victim) 
		{
			Victim = victim;
		}
	}

	public class HitBox2D : MonoBehaviour 
	{
		public int Damage;
		public GameObject Owner;
		public List<string> InteractionTypes;

		// Parented hitboxes are attached to owner
		// ex)
		// 1) Sword swing would be parented
		// 2) Projectile would not be parented, but 
		// we still want to know who the owner is
		public bool IsParented { get; private set; }

		public event EventHandler<LandedHitArgs> AfterLandedHit;

		// ---

		/// <summary>
		/// Instantiate a copy of a HitBox2D prefab.
		/// </summary>
		public static HitBox2D Initialize(
			HitBox2D prefab, 
			GameObject owner, Vector2 position, 
			Vector2 size, bool isParented, 
			List<string> interactionTypes) 
		{
			HitBox2D instance = SpawnManager.Spawn(prefab.gameObject, position).GetComponent<HitBox2D>();

			if (isParented) instance.transform.parent = owner.transform;
			instance.transform.position = position;
			instance.transform.localScale = size;
			instance.Owner = owner;
			instance.InteractionTypes = interactionTypes;

			return instance;
		}

		/// <summary>
		/// Create a new GameObject and attach a HitBox2D component to it.
		/// </summary>
		public static HitBox2D Initialize(
			GameObject owner, int damage, 
			Vector2 position, Vector2 size, 
			bool isParented, List<string> interactionTypes = null,
			bool isSquare = true) 
		{
			HitBox2D instance = SpawnManager.Spawn(position)
				.AddComponent<DrawBoxCollider2D>().gameObject
				.AddComponent<HitBox2D>();

			if (isSquare) 
				instance.gameObject.AddComponent<BoxCollider2D>();
			else
				instance.gameObject.AddComponent<CircleCollider2D>();

			instance.name = "HitBox";
			
			instance.GetComponentWithError<Collider2D>().isTrigger = true;

			if (isParented) 
				instance.transform.parent = owner.transform;

			instance.transform.position = position;
			instance.transform.localScale = size;
			instance.Owner = owner;
			instance.Damage = damage;
			instance.InteractionTypes = interactionTypes;

			return instance;
		}

		// ---
		// Events

		public virtual void OnAfterLandedHit(GameObject victim) 
		{
			if (AfterLandedHit != null) 
				AfterLandedHit(this, new LandedHitArgs(victim));
		}
	}
}
