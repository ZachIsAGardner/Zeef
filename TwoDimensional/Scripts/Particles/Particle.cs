﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

	public class Particle : MonoBehaviour  {

		[Header("Visual Renderers (Pick One)")]
		[SerializeField] private Image imageComponent;
		[SerializeField] private SpriteRenderer spriteRenderer;
		[SerializeField] private MeshRenderer meshRenderer;
		[SerializeField] private Text textComponent;

		private float lifeTime = 1;
		private bool fadeOverTime;
		private bool fadeOnDestroy;
		private Vector2 vel;
		private float grav;
		private Vector3 rotationVel;
		private Vector3 rotationOffset;

		private Color originalColor;
		
		public static Particle Initialize(GameObject prefab, float lifeTime, 
			Vector2? vel = null, bool fadeOverTime = false, 
			float grav = 0, Transform parent = null, 
			Vector3 pos = new Vector3(), bool fadeOnDestroy = false, 
			Vector3? rotationVel = null, Vector3? rotationOffset = null,
			Color? color = null) {
			
			// Ensure parent
			if (parent == null) {
				GameObject particles = GameObject.FindGameObjectWithTag(TagConstants.ParticlesFolder);
				if (particles == null) 
				{
					particles = new GameObject();
					particles.name = "_Particles";
					particles.tag = TagConstants.ParticlesFolder;

					parent = particles.transform;
				}
				parent = particles.transform;
			} 

			Particle instance = Instantiate(prefab, pos, Quaternion.identity, parent).GetComponent<Particle>();

			instance.GetComponents();

			if (color.HasValue) instance.ChangeColor(color.Value);

			instance.lifeTime = lifeTime;
			instance.fadeOverTime = fadeOverTime;
			instance.fadeOnDestroy = fadeOnDestroy;
			instance.vel = (vel.HasValue) ? vel.Value : Vector2.zero;
			instance.rotationVel = (rotationVel.HasValue) ? rotationVel.Value : Vector3.zero;
			instance.grav = grav;
			if (rotationOffset.HasValue) instance.transform.Rotate(rotationOffset.Value);

			instance.StartCoroutine(instance.RunCoroutine());

			return instance;
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
				while(GameManager.IsPaused) yield return null;

				if (fadeOverTime) ChangeColor(lifeTime);
				
				vel.y -= grav * Time.deltaTime;
				transform.position += (Vector3)vel * Time.deltaTime;
				transform.Rotate(rotationVel, Space.Self);

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
