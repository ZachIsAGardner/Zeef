using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeef.GameManagement;
using Zeef.Sound;

namespace Zeef.TwoDimensional {

	public enum FacingsEnum {
        Up,
        Down,
        Left,
        Right
    }

	// Sets up basic movement and collision
	[RequireComponent(typeof(AudioSource))]
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(Collision2D))]
	public abstract class MovingObject2D : MonoBehaviour {

		// State

		public FacingsEnum Facing { get; protected set; }
		// The position this MonoBehaviour was at on Start()
		public Vector3 StartPosition { get; protected set; }

		// Status

		[SerializeField] protected Vector2 velMax = new Vector2(100, 100);
		[Range (0, 1)]
		[SerializeField] protected float acc = .01f;
		[SerializeField] protected float moveSpeed = 5;

		[SerializeField] private bool active = true;
		public bool Active { get { return active; } set { active = value; } }

		// Components

		protected BoxCollider2D BoxCollider2D { get; private set; }
		protected Collision2D Collision { get; private set; }
		
		private Vector2 vel;
		public Vector2 Vel { get { return vel; } }

		// ---
		// Lifecycle

		protected virtual void Start () {
			StartPosition = transform.position;

			Collision = GetComponent<Collision2D>();
		}

		protected virtual void Update () {
			if (GameManager.IsPaused()) return;

			CalculateVelocity(ref vel);
			Collision.Move(vel * Time.deltaTime);
		}

		// ---
		// Collision and Velocity
		
		protected abstract void CalculateVelocity(ref Vector2 Vel);

		private void LimitVelocity() {
			if (Mathf.Abs(vel.x) > Mathf.Abs(velMax.x)) 
				vel.x = velMax.x * -Mathf.Sign(velMax.x);
			
			if (Mathf.Abs(vel.y) > Mathf.Abs(velMax.y)) 
				vel.y = velMax.y * Mathf.Sign(vel.y);	
		}

		// ---
		// Status

		public bool IsFacingUp() => Facing == FacingsEnum.Up;	
		
		public bool IsFacingDown() => Facing == FacingsEnum.Down;	
		
		public bool IsFacingLeft() => Facing == FacingsEnum.Left;	

		public bool IsFacingRight() => Facing == FacingsEnum.Right;	

		public bool AnyCollisions() {
			if (Collision.Collisions.Up || Collision.Collisions.Down 
			|| Collision.Collisions.Left || Collision.Collisions.Right) {
				return true;
			} else {
				return false;
			}
		}

		public bool IsMoving() => (Mathf.Abs(vel.x) > 0.1f || Mathf.Abs(vel.y) > 0.1f);
		
		public bool IsMovingDown() => vel.y < 0;
	}
}
