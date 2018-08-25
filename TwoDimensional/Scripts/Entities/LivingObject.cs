using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional {

	// Makes it so objects can live and die
	[RequireComponent (typeof(MovingObject2D))]
	public class LivingObject : MonoBehaviour {

		[SerializeField]
		SpriteRenderer sprite;
		MovingObject2D movingObject;

		// Freeze frames upon getting hit
		float frozenTime = 0.25f;
		protected bool frozen;

		public float inviTime = 0.25f;
		public bool invincible { get; private set; }

		public float maxHealth = 1;
		float health;

		public string[] weakTo = new string[]{"Hurt"};

		public string hurtSound = "hit_blunt";
		public GameObject hurtParticle;

		protected void Start() {
			health = maxHealth;
			GetComponents();
		}

		void GetComponents() {
			movingObject = GetComponent<MovingObject2D>();
			sprite = sprite ?? GetComponentInChildren<SpriteRenderer>();
		}
		
		protected void Update () {
			if (health <= 0) Die();
		}

		public virtual void Die() {
			Destroy(gameObject);
		}

		public void TakeDamage(HitBox2D hitBox, float damage, float bleed = 0) {
			health -= damage;

			// movingObject.QueueRecoil(hitBox.transform.position);
			
			sprite.material = Resources.Load<Material>("Materials/sprite_hurt");

			frozen = true;
			StartCoroutine(UnFreeze(frozenTime));

			invincible = true;
			StartCoroutine(EndInvincibility(inviTime));

			StartCoroutine(Blink());
		}

		Vector2 GetDirectionToHitBox(Transform trans) {
			return Vector2.zero;
		}

		IEnumerator UnFreeze(float wait) {
			yield return new WaitForSeconds(wait);
			sprite.material = Resources.Load<Material>("Materials/sprite");
			frozen = false;
		}

		IEnumerator EndInvincibility(float wait) {
			yield return new WaitForSeconds(wait);
			sprite.color = Color.white;
			invincible = false;
		}

		IEnumerator Blink() {
			while (invincible) {
				sprite.color = (sprite.color.a == 1) ? new Color(1,1,1,0) : new Color(1,1,1,1);
				yield return new WaitForSeconds(0.1f);
			}
			sprite.color = new Color(1,1,1,1);
		}

		protected void CreateHitEffect() {
			if (!hurtParticle) return;
			GameObject particleInstance = Instantiate(hurtParticle, transform);
			particleInstance.transform.localPosition = Vector3.back;
		}

		#region DetectHit

		bool ValidHit(HitBox2D hitBox) {
			if (!hitBox) throw new Exception("Could not find component 'HitBox' on gameobject.");
			// Not my hitbox
			return hitBox.Owner != this.gameObject;
		}

		protected virtual void OnTriggerStay2D(Collider2D col) {
			HitBox2D hitBox = col.GetComponent<HitBox2D>();

			if (col.tag == "Kill") {
				Die();
				return;
			}

			if (invincible) {
				return;
			}

			foreach (string weakness in weakTo) {
				if (col.tag == weakness) {

					if (ValidHit(hitBox)) {
						CreateHitEffect();
						TakeDamage(hitBox, hitBox.Damage);
					}
					return;
				}
			}
		}

		#endregion

	}

}
