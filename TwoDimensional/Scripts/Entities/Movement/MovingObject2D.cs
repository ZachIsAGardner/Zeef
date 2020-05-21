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
