using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

	public class Particle : MonoBehaviour  {

		// Whether or not this gets created from a particle creator

		private float lifeTime = 1;
		private bool fadeOverTime;
		private bool fadeOnDestroy;
		private float grav;
		private Vector2 vel;

		private Color originalColor;

		[SerializeField] private Image imageComponent;
		[SerializeField] private SpriteRenderer spriteRenderer;
		[SerializeField] private MeshRenderer meshRenderer;
		[SerializeField] private Text textComponent;

		public static Particle Initialize(GameObject prefab, float lifeTime, Vector2? vel = null, bool fadeOverTime = false, float grav = 0, Transform parent = null, Vector2 pos = new Vector2(), bool fadeOnDestroy = false) {
			Particle instance = Instantiate(prefab, pos, Quaternion.identity, parent).GetComponent<Particle>();

			instance.GetComponents();

			instance.lifeTime = lifeTime;
			instance.fadeOverTime = fadeOverTime;
			instance.fadeOnDestroy = fadeOnDestroy;
			instance.vel = (vel != null) ? (Vector2)vel : Vector2.zero;
			instance.grav = grav;

			instance.StartCoroutine(instance.RunCoroutine());

			return instance;
		}
		public static Particle Initialize(GameObject prefab, Vector2 lifeTime, Vector2 velocityMin, Vector2 velocityMax, bool fade = false, float grav = 0) {
			throw new NotImplementedException();
		}

		private void GetComponents() {
			imageComponent = GetComponent<Image>();
			if (imageComponent != null) originalColor = imageComponent.color;

			spriteRenderer = GetComponent<SpriteRenderer>();
			if (spriteRenderer != null) originalColor = spriteRenderer.color;

			meshRenderer = GetComponent<MeshRenderer>();
			if (meshRenderer != null) originalColor = meshRenderer.material.color;

			textComponent = GetComponent<Text>();
			if (textComponent != null) originalColor = textComponent.color;
		}

		private void ChangeColor(Color color) {
			if (imageComponent != null) imageComponent.color = color;	
			if (spriteRenderer != null) spriteRenderer.color = color;	
			if (meshRenderer != null) meshRenderer.material.color = color;	
			if (textComponent != null) textComponent.color = color;	
		}

		private void ChangeColor(float alpha) {
			if (imageComponent != null) imageComponent.color = 
				new Color(originalColor.r, originalColor.g, originalColor.b, alpha);	

			if (spriteRenderer != null) spriteRenderer.color =	
				new Color(originalColor.r, originalColor.g, originalColor.b, alpha);	

			if (meshRenderer != null) meshRenderer.material.color =	
				new Color(originalColor.r, originalColor.g, originalColor.b, alpha);	

			if (textComponent != null) textComponent.color =	
				new Color(originalColor.r, originalColor.g, originalColor.b, alpha);	
		}

		private Color CurrentColor() {
			if (imageComponent != null) return imageComponent.color;
			if (spriteRenderer != null) return spriteRenderer.color;
			if (meshRenderer != null) return meshRenderer.material.color;
			if (textComponent != null) return textComponent.color;
			throw new Exception("Not visual renderer could be found");
		}

		private IEnumerator RunCoroutine() {
			while(lifeTime > 0) {	
				while(GameManager.IsPaused()) yield return null;

				if (fadeOverTime) ChangeColor(lifeTime);
				
				vel.y -= grav * Time.deltaTime;
				transform.position += (Vector3)vel * Time.deltaTime;

				lifeTime -= 1 * Time.deltaTime;
				yield return null;
			}

			if (fadeOnDestroy && !fadeOverTime) {
				float fadeTime = 0.25f;
				while (CurrentColor().a >= 0) {
					vel.y -= grav * Time.deltaTime;
					transform.position += (Vector3)vel * Time.deltaTime;

					fadeTime -= Time.deltaTime;
					ChangeColor(fadeTime);
					
					yield return null;
				}
			}

			Destroy(gameObject);
		}
	}
}
