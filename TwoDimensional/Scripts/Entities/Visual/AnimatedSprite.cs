using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

	public class AnimationEvent {

		public int SpriteIndex;
		public Action Action;

		public AnimationEvent(int spriteIndex, Action newAction) {
			SpriteIndex = spriteIndex;
			Action = newAction;
		}

		public static bool IncludesString(string[] strings, string target) => strings.Any(s => s == target);
	}

	public class AnimationState {

		public string Name { get; private set; }
		// The range on the sprite sheet this state is mapped to
		public IntegerRange Range { get; private set; }
		public List<AnimationEvent> AnimationEvents { get; private set; }
		public bool Loop { get; private set; }
		public float Speed { get; private set; }
		public IntegerRange LoopRange { get; private set; }

		public AnimationState(string name, IntegerRange range, List<AnimationEvent> animationEvents = null, bool loop = true, float speed = 1, IntegerRange loopRange = null) {
			Name = name;
			Range = range;
			AnimationEvents = animationEvents;
			Loop = loop;
			Speed = speed;
			LoopRange = loopRange;
		}
	}

	// ---

	public abstract class AnimatedSprite<T> : MonoBehaviour {

		// References
		[Header("Visual Renderers (Pick one)")]
		[SerializeField] protected SpriteRenderer spriteRenderer;
		[SerializeField] protected Image imageRenderer;
		[SerializeField] protected MeshRenderer meshRenderer;

		[Header("Other")]
		[Required]
		[SerializeField] protected T advisor;

		// Art
		[Required]
		[SerializeField] private SpritesScriptable spritesScriptable;
		private Sprite[] sprites = new Sprite[]{};

		public bool IsPaused { get; set; }

		public AnimationState State { get; private set; }

		// Frame Rate
		private float tick;
		private float ticksPerFrame = 0.12f;

		private int spriteIndex;

		// Abstract
		protected abstract AnimationState GetAnimationState();

		// ---

		protected virtual void Start () {	
			// Get components
			spriteRenderer = spriteRenderer ?? GetComponent<SpriteRenderer>();
			imageRenderer = imageRenderer ?? GetComponent<Image>();
			meshRenderer = meshRenderer ?? GetComponent<MeshRenderer>();

			// Fill sprites
			sprites = (sprites == null || sprites.Length < 1) ? spritesScriptable?.Sprites.ToArray() : sprites;
		}

		void Update() {
			if (GameManager.IsPaused || sprites.IsNullOrEmpty() || IsPaused) return;

			AnimationState newState = GetAnimationState();
			if (State == null || newState.Name != State.Name) {
				State = newState;
				spriteIndex = State.Range.Min;
				tick = 0;

				ExecuteAnimationState();
			}

			// Update state without restarting
			if (State != null && newState.Name == State.Name) {
				State = newState;
			}

			Animate();
		}

		// ---

		public void SetSprites(Sprite[] sprites) {
			this.sprites = sprites;
		}

		public void GoToIndex(int spriteIndex) {
			this.spriteIndex = spriteIndex;
			ExecuteAnimationState();
		}

		// ---

		// Display sprite and execute events
		private void ExecuteAnimationState() {
				
			// Display sprite
			if (spriteRenderer != null) spriteRenderer.sprite = sprites[spriteIndex];
			else if (imageRenderer != null) imageRenderer.sprite = sprites[spriteIndex];
			else if (meshRenderer != null) meshRenderer.material.mainTexture = sprites[spriteIndex].texture;
			else throw new Exception("This GameObject does not have any visual components attached.");	

			// Execute events
			if (!State.AnimationEvents.IsNullOrEmpty())
				foreach (AnimationEvent animationEvent in State.AnimationEvents) 
					if (spriteIndex == animationEvent.SpriteIndex) animationEvent.Action();
		}

		// Increment sprite index
		private void Animate() {
			
			tick += State.Speed * Time.deltaTime;
			int oldSpriteIndex = spriteIndex;

			if (tick > ticksPerFrame) {
				spriteIndex += 1;
				tick = 0;

				// Index is greater than loop range
				if (State.LoopRange != null && spriteIndex > State.LoopRange.Max)
					spriteIndex = State.LoopRange.Min;

				// Index is greater than state range
				if (spriteIndex > State.Range.Max) {
					if (State.Loop) spriteIndex = State.Range.Min;
					else spriteIndex = State.Range.Max;	
				}
	
				// Index is greater than entire sprite sheet length
				if (spriteIndex > sprites.Length - 1) {
					if (State.Loop) spriteIndex = 0;
					else spriteIndex = sprites.Length - 1;	
				} 	

				// If our sprite index is still the same we 
				// don't really need to do anything, else we need to 
				// execute animation state
				if (oldSpriteIndex != spriteIndex) ExecuteAnimationState();
			}
		}
	}
}
