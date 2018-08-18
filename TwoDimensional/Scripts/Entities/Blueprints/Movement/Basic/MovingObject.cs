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
		public Vector2 velMax = new Vector2(100, 100);
		public float groundAcc = .01f;
		public float moveSpeed = 5;

		// Used in enemy for before they are triggered
		protected bool active = true;

		//references
		protected GameManager Game { get; private set; }
		protected BoxCollider2D BoxCollider2D { get; private set; }
		protected Collision Collision { get; private set; }
		protected AudioSource AudioSource { get; private set; }
		protected SpriteRenderer Sprite { get; private set; }
		protected AnimatedSprite Animator { get; private set; }
		protected ZeeTimerHandler TimerHandler { get; private set; }
		
		Vector2 vel;

		bool queueRecoil;
		Vector2 recoilDir;

		protected virtual void Start () {
			StartPosition = transform.position;
			GetComponents();
			SetUpSound();
		}

		void SetUpSound() {
			AudioSource.volume = AudioManager.SoundEffectVolume;
		}

		void GetComponents() {
			Game = GameManager.Main();

			Collision = GetComponent<Collision>();
			Sprite = GetComponentInChildren<SpriteRenderer>();
			Animator = GetComponentInChildren<AnimatedSprite>();
			AudioSource = GetComponent<AudioSource>();
			TimerHandler = GetComponent<ZeeTimerHandler>();
		}

		protected virtual void Update () {
			if (!(Game.IsPlaying() || Game.InCutscene())) return;

			CalculateVelocity(ref vel);
			if (queueRecoil) {
				Recoil(recoilDir, ref vel);
				queueRecoil = false;
			}
			Collision.Move(vel * Time.deltaTime);
			CollisionVelocity(ref vel);	
		}

		#region State

		public bool FacingUp() {
			return Facing == FacingsEnum.Up;	
		}
		public bool FacingDown() {
			return Facing == FacingsEnum.Down;	
		}
		public bool FacingLeft() {
			return Facing == FacingsEnum.Left;	
		}
		public bool FacingRight() {
			return Facing == FacingsEnum.Right;	
		}

		public void Activate() {
			active = true;
		}
		public void DeActivate() {
			active = false;
		}

		#endregion

		#region HelperMethods

		protected bool AnyCollision() {
			if (Collision.collisions.up 
			|| Collision.collisions.down 
			|| Collision.collisions.left 
			|| Collision.collisions.right) {
				return true;
			} else {
				return false;
			}
		}

		public bool Moving() {
			return (Mathf.Abs(vel.x) > 0.1f || Mathf.Abs(vel.y) > 0.1f);
		}

		public bool MovingDown() {
			return vel.y < 0;
		}

		public virtual void LandedHit(GameObject victim) { }

		#endregion

		#region CollisionAndVelocity

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

		//is used for it's side effects but still returns a vector2. Confusing? yes.
		protected virtual void CalculateVelocity(ref Vector2 vel) { }

		protected virtual void LimitVelocity() {
			if (Mathf.Abs(vel.x) > Mathf.Abs(velMax.x)) {
				vel.x = velMax.x * -Mathf.Sign(velMax.x);
			}
			if (Mathf.Abs(vel.y) > Mathf.Abs(velMax.y)) {
				vel.y = velMax.y * Mathf.Sign(vel.y);
			}
		}

		#endregion
	}
}
