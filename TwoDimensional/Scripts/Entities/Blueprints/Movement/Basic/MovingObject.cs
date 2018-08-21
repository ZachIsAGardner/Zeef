using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeef.GameManagement;
using Zeef.Sound;

namespace Zeef.TwoDimensional {
	// Sets up basic movement and collision
	// Gets further implemented by platformer and topdown
	[RequireComponent(typeof(Collision))]
	[RequireComponent(typeof(AudioSource))]
	[RequireComponent(typeof(ZeeTimerHandler))]
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(BoxCollider2D))]
	public abstract class MovingObject : MonoBehaviour {

		//state
		public FacingsEnum Facing { get; protected set; }
		public Vector3 StartPosition { get; set; }

		// basic movement stats
		public Vector2 VelMax = new Vector2(100, 100);
		public float GroundAcc = .01f;
		public float MoveSpeed = 5;

		// Used in enemy for before they are triggered
		// TODO: revisit this
		public bool Active = true;

		//references
		protected BoxCollider2D BoxCollider2D { get; private set; }
		protected Collision Collision { get; private set; }
		protected AudioSource AudioSource { get; private set; }
		protected SpriteRenderer Sprite { get; private set; }
		protected AnimatedSprite Animator { get; private set; }
		protected ZeeTimerHandler TimerHandler { get; private set; }
		
		private Vector2 vel;

		private bool queueRecoil;
		private Vector2 recoilDir;

		// ---
		// Lifecycle

		protected virtual void Start () {
			StartPosition = transform.position;

			Collision = GetComponent<Collision>();
			Sprite = GetComponentInChildren<SpriteRenderer>();
			Animator = GetComponentInChildren<AnimatedSprite>();
			AudioSource = GetComponent<AudioSource>();
			TimerHandler = GetComponent<ZeeTimerHandler>();

			AudioSource.volume = AudioManager.SoundEffectVolume;
		}

		protected virtual void Update () {
			if (!(GameManager.IsPlaying() || GameManager.IsInCutscene())) return;

			CalculateVelocity(ref vel);
			if (queueRecoil) {
				Recoil(recoilDir, ref vel);
				queueRecoil = false;
			}
			Collision.Move(vel * Time.deltaTime);
			CollisionVelocity(ref vel);	
		}

		// ---
		// Collision and Velocity

		// Recoil on hit
		public virtual void QueueRecoil(Vector2 dir) {
			queueRecoil = true;
			recoilDir = dir;
		}

		protected virtual void Recoil(Vector2 dir, ref Vector2 vel) {
			vel = Vector2.zero;
		}

		protected virtual float CalculateAcceleration() {
			return 0;
		}

		// reset vel on collision
		protected virtual void CollisionVelocity(ref Vector2 vel) {
			if (Collision.collisions.up || Collision.collisions.down) {
				// vel.y = 0;
			}
			if (Collision.collisions.left || Collision.collisions.right) {
				// vel.x = 0;
			}
		}

		// TODO: is used for it's side effects but still returns a vector2. Confusing? yes.
		protected virtual void CalculateVelocity(ref Vector2 vel) {

		}

		protected virtual void LimitVelocity() {
			if (Mathf.Abs(vel.x) > Mathf.Abs(VelMax.x)) {
				vel.x = VelMax.x * -Mathf.Sign(VelMax.x);
			}
			if (Mathf.Abs(vel.y) > Mathf.Abs(VelMax.y)) {
				vel.y = VelMax.y * Mathf.Sign(vel.y);
			}
		}

		// ---
		// Status

		// TODO: These should not be on MovingObject
		public bool IsFacingUp() => Facing == FacingsEnum.Up;	
		
		public bool IsFacingDown() => Facing == FacingsEnum.Down;	
		
		public bool IsFacingLeft() => Facing == FacingsEnum.Left;	

		public bool IsFacingRight() => Facing == FacingsEnum.Right;	

		// TODO: why is this here?
		public virtual void LandedHit(GameObject victim) { }

		protected bool AnyCollision() {
			if (Collision.collisions.up || Collision.collisions.down 
			|| Collision.collisions.left || Collision.collisions.right) {
				return true;
			} else {
				return false;
			}
		}
			
		// TODO: Revisit this
		public bool IsMoving() => (Mathf.Abs(vel.x) > 0.1f || Mathf.Abs(vel.y) > 0.1f);
		
		// TODO: Revisit this
		public bool IsMovingDown() => vel.y < 0;
	}
}
