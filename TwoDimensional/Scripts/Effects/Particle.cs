using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

	public class Particle : MonoBehaviour  {

		// Whether or not this gets created from a particle creator
		[SerializeField] bool independant;
		[SerializeField] float lifeTime = 1;
		[SerializeField] bool fade;
		[SerializeField] float grav;
		[SerializeField] Vector2 velocityMin;
		[SerializeField] Vector2 velocityMax;

		private Vector2 vel;

		private Image imageComponent;
		private SpriteRenderer spriteRenderer;
		private MeshRenderer meshRenderer;
		private Text textComponent;

		void Start() {
			if (independant) { 
				vel = new Vector2(
					UnityEngine.Random.Range(velocityMin.x, velocityMax.x),
					UnityEngine.Random.Range(velocityMin.y, velocityMax.y)
				);
				GetComponents();
				StartCoroutine(RunCoroutine());
			}
		}

		public static Particle Initialize(GameObject prefab, float lifeTime, Vector2? vel = null, bool fade = false, float grav = 0) {
			Particle particle = Instantiate(prefab).GetComponent<Particle>();

			particle.GetComponents();

			particle.lifeTime = lifeTime;
			particle.fade = fade;
			particle.vel = (vel != null) ? (Vector2)vel : Vector2.zero;
			particle.grav = grav;

			particle.StartCoroutine(particle.RunCoroutine());

			return particle;
		}

		public static Particle Initialize(GameObject prefab, Vector2 lifeTime, Vector2 velocityMin, Vector2 velocityMax, bool fade = false, float grave = 0) {
			throw new NotImplementedException();
		}

		private void GetComponents() {
			imageComponent = GetComponent<Image>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			meshRenderer = GetComponent<MeshRenderer>();
			textComponent = GetComponent<Text>();
		}

		private void ChangeColor(Color color) {
			if (imageComponent != null) {
				imageComponent.color = color;
			}
			if (spriteRenderer != null) {
				spriteRenderer.color = color;
			}
			if (meshRenderer != null) {
				meshRenderer.material.color = color;
			}
			if (textComponent != null) {
				textComponent.color = color;
			}
		}

		private IEnumerator RunCoroutine() {
			while(lifeTime > 0) {	
				while(GameManager.Main().IsPaused()) yield return null;

				if (fade) ChangeColor(new Color(1, 1, 1, lifeTime));
				
				vel.y -= grav * Time.deltaTime;
				transform.position += (Vector3)vel * Time.deltaTime;

				lifeTime -= 1 * Time.deltaTime;
				yield return null;
			}

			Destroy(gameObject);
		}
	}
}
