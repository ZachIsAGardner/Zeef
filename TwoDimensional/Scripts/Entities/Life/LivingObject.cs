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
		[SerializeField] SpriteRenderer spriteRenderer;
		[SerializeField] MovingObject2D movingObject2D;
		[Required]
		[SerializeField] List<HurtBox2D> weakPoints;

		public float HealthPercentage { get { return (float)Health / (float)MaxHealth; } }
		public bool IsInvincible { get; private set; }
		public bool IsFrozen { get; private set ; }

		public event EventHandler<DamageEventArgs> BeforeTakeDamage;
		public event EventHandler<DamageEventArgs> AfterSurviveTakeDamage;
		public event EventHandler BeforeDie;

		// ---	

		void Start() {
			movingObject2D = movingObject2D ?? GetComponent<MovingObject2D>();
			foreach (HurtBox2D hurtBox in weakPoints) 
				hurtBox.ExternalTriggerStay2D += OnExternalTriggerStay2D;
		}

		public async void OnExternalTriggerStay2D(object source, ExternalTriggerStay2DEventArgs args) {
			if (IsFrozen || IsInvincible) return;

			HitBox2D hitBox = args.Other.GetComponent<HitBox2D>();

			// if hitbox exists and it is not mine then i need to take damage
			if (hitBox != null && hitBox.Owner != movingObject2D) 
				await TakeDamageAsync(hitBox, hitBox.Damage);		
		}

		// ---
		
		public void Die() {
			OnBeforeDie();
			Destroy(gameObject);
		}

		public async virtual Task TakeDamageAsync(HitBox2D hitBox, int damage) {

			StopAllCoroutines();
			Color color = spriteRenderer.color;
			spriteRenderer.color = new Color(color.r, color.g, color.b, 1);

			OnBeforeTakeDamage(hitBox);

			Health -= damage;
				
			if (Health <= 0) { 
				await FreezeAsync();
				Die();
			} else {
				OnAfterTakeDamage(hitBox);
				await FreezeAsync();
				await InvincibilityAsync();
			}
		}

		// ---

		private async Task FreezeAsync() {
			IsFrozen = true;
			IsInvincible = true;

			Color originalColor = spriteRenderer.color;
			spriteRenderer.color = Color.black;

			await new WaitForSeconds(freezeDuration);

			IsFrozen = false;
			spriteRenderer.color = originalColor;
		}

		private async Task InvincibilityAsync() {
			IsInvincible = true;

			StartCoroutine(BlinkCoroutine());
			await new WaitForSeconds(invincibilityDuration);

			IsInvincible = false;
		}

		private IEnumerator BlinkCoroutine() {

			Color color = spriteRenderer.color;

			while(IsInvincible) {
				spriteRenderer.color = (spriteRenderer.color.a == 1) 
					? new Color(color.r, color.g, color.b, 0) 
					: new Color(color.r, color.g, color.b, 1);

				yield return new WaitForSeconds(0.05f);
			}

			spriteRenderer.color = new Color(color.r, color.g, color.b, 1);
		}

		// ---
		// Events

		protected virtual void OnBeforeTakeDamage(HitBox2D hitBox) {
			if (BeforeTakeDamage != null) 
				BeforeTakeDamage(this, new DamageEventArgs(hitBox));
		}

		protected virtual void OnAfterTakeDamage(HitBox2D hitBox) {
			if (AfterSurviveTakeDamage != null) 
				AfterSurviveTakeDamage(this, new DamageEventArgs(hitBox));
		}

		protected virtual void OnBeforeDie() {
			if (BeforeDie != null) 
				BeforeDie(this, EventArgs.Empty);
		}
	}
}
