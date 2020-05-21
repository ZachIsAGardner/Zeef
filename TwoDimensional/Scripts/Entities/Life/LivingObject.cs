using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zeef.GameManagement;

namespace Zeef.TwoDimensional 
{
	public class AfterGainHealthEventArgs 
	{
		public int Amount { get; private set; }

		public AfterGainHealthEventArgs(int amount) 
		{
			Amount = amount;
		}
	}

	public class DamageEventArgs 
	{
		public HitBox2D HitBox2D { get; private set; }

		public DamageEventArgs(HitBox2D hitBox) 
		{
			HitBox2D = hitBox;
		}
	}

	// Makes it so objects can live and die
	public class LivingObject : MonoBehaviour 
	{
		[SerializeField] int maxHealth = 1;
		public int MaxHealth { get { return maxHealth; } }

		public int Health = 1;

		[SerializeField] float invincibilityDuration = 0.5f;
		[SerializeField] float freezeDuration = 0.25f;

		[Required]	
		[SerializeField] private List<HurtBox2D> weakPoints;

		public float HealthPercentage { get { return (float)Health / (float)MaxHealth; } }
		public bool IsInvincible { get; private set; }
		public bool IsFrozen { get; private set ; }

		public event EventHandler<AfterGainHealthEventArgs> AfterGainHealth;
		public event EventHandler BeforeFreeze;
		public event EventHandler AfterFreeze;
		public event EventHandler BeforeInvincibility;
		public event EventHandler AfterInvincibility;
		public event EventHandler<DamageEventArgs> BeforeTakeDamage;
		public event EventHandler<DamageEventArgs> AfterSurviveTakeDamage;
		public event EventHandler BeforeDie;

		// ---	

		protected virtual void Start()
		{
			if (weakPoints == null)
				return;
				
			foreach (HurtBox2D hurtBox in weakPoints) {
				hurtBox.ExternalTriggerStay2D += OnExternalTriggerStay2D;
			}
		}

		public async void OnExternalTriggerStay2D(object source, ExternalTriggerStay2DEventArgs args) 
		{
			// Can't take damage if frozen, invincible, or the game isn't playing
			if (IsFrozen || IsInvincible || !GameState.IsPlaying)
				return;

			HitBox2D hitBox = args.Other.GetComponent<HitBox2D>();

			// if hitbox exists and it is not mine then i need to take damage
			if (hitBox != null && hitBox.Owner != gameObject) 
			{
				LivingObject livingObject = null;
				if (hitBox != null && hitBox.Owner != null) 
					livingObject = hitBox.Owner.GetComponentInChildren<LivingObject>();

				if (livingObject == null || livingObject != null && !livingObject.IsFrozen)
					await TakeDamageAsync(hitBox.Damage, hitBox);		
			}
		}

		// ---
		
		public virtual async Task DieAsync() 
		{
			OnBeforeDie();
			Destroy(gameObject);
		}

		public async virtual Task GainHealthAsync(int amount) 
		{
			Health += amount;
			if (Health > maxHealth)
				Health = maxHealth;

			OnAfterGainHealth(amount);
		}

		public async virtual Task TakeDamageAsync(int damage) 
		{
			await TakeDamageAsync(damage, null);
		}

		public async virtual Task TakeDamageAsync(int damage, HitBox2D hitBox) 
		{
			OnBeforeTakeDamage(hitBox);

			if (hitBox != null) hitBox.OnAfterLandedHit(gameObject);

			Health -= damage;
				
			if (Health <= 0) 
			{ 
				// await FreezeAsync();
				IsInvincible = true;
				await DieAsync();
			} 
			else 
			{
				OnAfterSurviveTakeDamage(hitBox);
				await FreezeAsync();
				await InvincibilityAsync();
			}
		}

		// ---

		private async Task FreezeAsync() 
		{
			IsFrozen = true;
			IsInvincible = true;

			OnBeforeFreeze();

			float timeElapsed = 0;
			while (timeElapsed < freezeDuration) 
			{
				if (GameState.IsPlaying) timeElapsed += Time.deltaTime;
				await new WaitForUpdate();
			}

			OnAfterFreeze();

			IsFrozen = false;
		}

		private async Task InvincibilityAsync() 
		{
			IsInvincible = true;

			OnBeforeInvincibility();

			float timeElapsed = 0;
			while (timeElapsed < invincibilityDuration) 
			{
				if (GameState.IsPlaying) timeElapsed += Time.deltaTime;
				await new WaitForUpdate();	
			}

			OnAfterInvincibility();

			IsInvincible = false;
		}

		// ---
		// Events

		protected virtual void OnAfterGainHealth(int amount)
		{
			if (AfterGainHealth != null)
				AfterGainHealth(this, new AfterGainHealthEventArgs(amount));
		}

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
