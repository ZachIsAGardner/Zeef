using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Zeef.TwoDimensional {

	public class DamageEventArgs {
		public HitBox2D HitBox2D { get; private set; }

		public DamageEventArgs(HitBox2D hitBox) {
			HitBox2D = hitBox;
		}
	}

	// Makes it so objects can live and die
	[RequireComponent (typeof(BoxCollider2D))]
	public class LivingObject : MonoBehaviour {

		[SerializeField] int maxHealth = 1;
		public int MaxHealth { get { return maxHealth; } }

		public int Health = 1;

		[SerializeField] float invincibilityDuration = 0.5f;
		[SerializeField] float freezeDuration = 0.25f;

		[Required]	
		[SerializeField] List<HurtBox2D> weakPoints;

		public float HealthPercentage { get { return (float)Health / (float)MaxHealth; } }
		public bool IsInvincible { get; private set; }
		public bool IsFrozen { get; private set ; }

		public event EventHandler BeforeFreeze;
		public event EventHandler AfterFreeze;
		public event EventHandler BeforeInvincibility;
		public event EventHandler AfterInvincibility;
		public event EventHandler<DamageEventArgs> BeforeTakeDamage;
		public event EventHandler<DamageEventArgs> AfterSurviveTakeDamage;
		public event EventHandler BeforeDie;

		// ---	

		void Start() {
			foreach (HurtBox2D hurtBox in weakPoints) 
				hurtBox.ExternalTriggerStay2D += OnExternalTriggerStay2D;
		}

		public async void OnExternalTriggerStay2D(object source, ExternalTriggerStay2DEventArgs args) {
			if (IsFrozen || IsInvincible) return;

			HitBox2D hitBox = args.Other.GetComponent<HitBox2D>();

			// if hitbox exists and it is not mine then i need to take damage
			if (hitBox != null && hitBox.Owner != gameObject) {
				await TakeDamageAsync(hitBox.Damage, hitBox);		
			}
		}

		// ---
		
		public void Die() {
			OnBeforeDie();
			Destroy(gameObject);
		}

		public async virtual Task TakeDamageAsync(int damage) {
			await TakeDamageAsync(damage, null);
		}

		public async virtual Task TakeDamageAsync(int damage, HitBox2D hitBox) {
			OnBeforeTakeDamage(hitBox);

			if (hitBox != null) hitBox.OnAfterLandedHit(gameObject);

			Health -= damage;
				
			if (Health <= 0) { 
				await FreezeAsync();
				Die();
			} else {
				OnAfterSurviveTakeDamage(hitBox);
				await FreezeAsync();
				await InvincibilityAsync();
			}
		}

		// ---

		private async Task FreezeAsync() {
			IsFrozen = true;
			IsInvincible = true;

			OnBeforeFreeze();

			await new WaitForSeconds(freezeDuration);

			OnAfterFreeze();

			IsFrozen = false;
		}

		private async Task InvincibilityAsync() {
			IsInvincible = true;

			OnBeforeInvincibility();

			await new WaitForSeconds(invincibilityDuration);

			OnAfterInvincibility();

			IsInvincible = false;
		}

		// ---
		// Events

		protected virtual void OnBeforeTakeDamage(HitBox2D hitBox) {
			if (BeforeTakeDamage != null) 
				BeforeTakeDamage(this, new DamageEventArgs(hitBox));
		}

		protected virtual void OnAfterSurviveTakeDamage(HitBox2D hitBox) {
			if (AfterSurviveTakeDamage != null) 
				AfterSurviveTakeDamage(this, new DamageEventArgs(hitBox));
		}

		protected virtual void OnBeforeFreeze() {
			if (BeforeFreeze != null) 
				BeforeFreeze(this, EventArgs.Empty);
		}

		protected virtual void OnAfterFreeze() {
			if (AfterFreeze != null) 
				AfterFreeze(this, EventArgs.Empty);
		}

		protected virtual void OnBeforeInvincibility() {
			if (BeforeInvincibility != null) 
				BeforeInvincibility(this, EventArgs.Empty);
		}

		protected virtual void OnAfterInvincibility() {
			if (AfterInvincibility != null) 
				AfterInvincibility(this, EventArgs.Empty);
		}

		protected virtual void OnBeforeDie() {
			if (BeforeDie != null) 
				BeforeDie(this, EventArgs.Empty);
		}
	}
}
