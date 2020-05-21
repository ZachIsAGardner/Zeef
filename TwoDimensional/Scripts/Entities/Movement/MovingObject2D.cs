using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeef.GameManagement;

namespace Zeef.TwoDimensional 
{	
	/// <summary>
	/// Sets up basic movement and collision.
	/// </summary>
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(Collision2D))]
	public abstract class MovingObject2D : MonoBehaviour
	{
		// Stats

		[Header("Moving Object 2D Settings")]

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

		// ---

		/// <summary>
		/// Contains info about any current collisions.
		/// </summary>
		public Collision2D Collision { get; private set; }

        /// <summary>
        /// Current velocity for this moving object.
        /// </summary>
        [HideInInspector] public Vector2 Velocity;

        /// <summary>
        /// This value is added on top of Velocity when applying translations.
        /// </summary>
		[HideInInspector] public Vector2 VelocityOffset;

		// ---
		// Lifecycle

		protected virtual void Start () 
		{
			Collision = this.GetComponentWithError<Collision2D>();
		}

		protected virtual void Update () 
		{
			if (!GameState.IsPlaying) 
				return;

			Collision.Move((Velocity + VelocityOffset) * Time.deltaTime);            
		}

		// ---
		// Collision and Velocity

		private void LimitVelocity() 
		{
			if (Mathf.Abs(Velocity.x) > Mathf.Abs(velMax.x)) 
				Velocity.x = velMax.x * -Mathf.Sign(velMax.x);
			
			if (Mathf.Abs(Velocity.y) > Mathf.Abs(velMax.y)) 
				Velocity.y = velMax.y * Mathf.Sign(Velocity.y);	
		}

		// ---
		// Status

		public bool AnyCollisions() 
		{
			if (Collision.Collisions.Up || Collision.Collisions.Down 
			|| Collision.Collisions.Left || Collision.Collisions.Right) 
			{
				return true;
			} 
			else 
			{
				return false;
			}
		}
	}
}
