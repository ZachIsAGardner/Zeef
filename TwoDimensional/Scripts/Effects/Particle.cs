using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zeef.GameManager;

namespace Zeef.TwoDimensional {
	public class Particle : MonoBehaviour 
	{
		Game game;
		Image image;
		SpriteRenderer spriteRenderer;
		MeshRenderer meshRenderer;
		Text textRenderer;

		public float lifeTime = 1;
		public bool fade;
		public Vector2 vel;
		public float grav;
		public bool independant;

		void Start() {
			if (independant) {
				Initialize(lifeTime, vel, fade, grav);
			}
		}

		public Particle Initialize(float lifeTime, Vector2? vel = null, bool fade = false, float grav = 0) {
			this.lifeTime = lifeTime;
			this.fade = fade;
			this.vel = (vel != null) ? (Vector2)vel : Vector2.zero;
			this.grav = grav;

			GetComponents();

			StartCoroutine(Run());

			return this;
		}

		void GetComponents() {
			game = Game.Main();
			image = GetComponent<Image>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			meshRenderer = GetComponent<MeshRenderer>();
			textRenderer = GetComponent<Text>();
		}

		void ChangeColor(Color color) {
			if (image != null) {
				image.color = color;
			}
			if (spriteRenderer != null) {
				spriteRenderer.color = color;
			}
			if (meshRenderer != null) {
				meshRenderer.material.color = color;
			}
			if (textRenderer != null) {
				textRenderer.color = color;
			}
		}

		IEnumerator Run() {
			while(lifeTime > 0) {	
				while(game.Paused()) yield return null;

				if (fade) {
					ChangeColor(new Color(1, 1, 1, lifeTime));
				}

				vel.y -= grav * Time.deltaTime;
				transform.position += (Vector3)vel * Time.deltaTime;

				lifeTime -= 1 * Time.deltaTime;
				yield return null;
			}

			Destroy(gameObject);
		}
	}
}
