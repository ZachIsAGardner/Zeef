using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {
	
	/// <summary>
	/// Direction an entity is facing.
	/// </summary>
	public enum FacingsEnum {
        Up,
        Down,
        Left,
        Right
    }

	/// <summary>
	/// Sets up basic movement and collision.
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(Collision2D))]
	public abstract class MovingObject2D : MonoBehaviour {

		// State

		/// <summary>
		/// Direction this moving object is facing.
		/// </summary>
		public FacingsEnum Facing { get; protected set; }

		/// <summary>
		/// The position this moving object was at on it's Start()
		/// </summary>
		public Vector3 StartPosition { get; protected set; }

		// Stats

		/// <summary>
		/// Velocity will be capped if exceding this vector.
		/// </summary>
		[SerializeField] protected Vector2 velMax = new Vector2(100, 100);

		[Range (0, 1)]
		[SerializeField] protected float acc = .01f;
		/// <summary>
		/// Float controlling the duration between current velocity and the desired velicity.
		/// 0: instant.
		/// 1: never.
		/// </summary>
		public float Acc { get { return acc; }}

		[SerializeField] protected float moveSpeed = 5;
		/// <summary>
		/// 0: instant.
		/// 1: never.
		/// </summary>
		public float MoveSpeed { get { return moveSpeed; }}

		[SerializeField] private bool active = true;
		/// <summary>
		/// Arbitrary boolean stating whether or not this moving object is active.
		/// </summary>
		public bool Active { get { return active; } set { active = value; } }

		// Components

		protected BoxCollider2D BoxCollider2D { get; private set; }
		/// <summary>
		/// Contains info about any current collisions.
		/// </summary>
		public Collision2D Collision { get; private set; }

		// -
		
		private Vector2 vel;
		/// <summary>
		/// Current velocity for this moving object.
		/// </summary>
		public Vector2 Vel { get { return vel; } }

		// ---
		// Lifecycle

		protected virtual void Start () {
			StartPosition = transform.position;

			Collision = this.GetComponentWithError<Collision2D>();
		}

		protected virtual void Update () {
			if (GameManager.IsPaused()) return;

			CalculateVelocity(ref vel);
			Collision.Move(vel * Time.deltaTime);
		}

		// ---
		// Collision and Velocity
		
		protected abstract void CalculateVelocity(ref Vector2 vel);

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

		public bool IsMoving() => (Mathf.Abs(vel.x) > 0.5f || Mathf.Abs(vel.y) > 0.5f);
		
		public bool IsMovingDown() => vel.y < 0;
	}
}
