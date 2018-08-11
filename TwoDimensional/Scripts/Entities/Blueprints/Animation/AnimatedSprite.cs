using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {


	public abstract class AnimatedSprite : MonoBehaviour {
		public class AnimationEvent {
			public string[] states;
			public int frame;
			public Action action;

			public AnimationEvent(string[] newStates, int newFrame, Action newAction) {
				states = newStates;
				frame = newFrame;
				action = newAction;
			}
		}

		// References
		private GameManager game;
		[SerializeField] private SpriteRenderer spriteRenderer;
		[SerializeField] private Image imageRenderer;
		[SerializeField] private MeshRenderer meshRenderer;

		// Art
		[SerializeField] private SpritesObject spritesObject;
		private Sprite[] sprites = new Sprite[]{};

		// Frame Rate
		private float tick;
		private float ticksPerFrame = 1;

		// Current Sprite
		private string animationState;
		private int spriteIdx = 1;
		private int[] range = new int[1];
		private int frame;
		protected bool loop = true;

		private string forcedState = null;

		// Animation Events
		private AnimationEvent[] events = new AnimationEvent[]{};
		private int executedFrame = -1;
		private string executedState = "";

		protected abstract void GetAdvisor();
		protected abstract string GetAnimationState();
		protected abstract int[] ParseAnimationState(string state);
		protected abstract AnimationEvent[] GetAnimationEvents();

		protected virtual void Start () {
			game = GameManager.Main();

			spriteRenderer = spriteRenderer ?? GetComponent<SpriteRenderer>();
			imageRenderer = imageRenderer ?? GetComponent<Image>();
			meshRenderer = meshRenderer ?? GetComponent<MeshRenderer>();
			GetAdvisor();

			if (sprites.Length == 0  && spritesObject != null) sprites = spritesObject.sprites.ToArray();

			events = GetAnimationEvents();

			range = new int[] {0, 0};
		}

		void Update() {
			if (!game.IsPaused()) {
				EvaluateState();
				ExecuteAnimationEvents();
				RenderSprite();
			}
		}

		// --

		public void ForceState(string state) {
			forcedState = state;
		}

		public void SetSprites(Sprite[] sprites) {
			this.sprites = sprites;
		}

		private void EvaluateState() {
			string newState = forcedState ?? GetAnimationState();
			if (newState != animationState) {
				loop = true;
				ChangeSpeed(1);
				ChangeState(ParseAnimationState(newState), newState);
			}
		}

		// ---

		private void RenderSprite() {
			if (sprites.Length < 1) return;

			tick += 10 * Time.deltaTime;

			if (tick > ticksPerFrame) {
				spriteIdx += 1;
				tick = 0;
			}

			if (spriteIdx > range[1]) {
				if (loop) spriteIdx = range[0];
				else spriteIdx = range[1];	
			}
			
			if (spriteIdx > sprites.Length - 1) {
				if (loop) spriteIdx = 0;
				else spriteIdx = sprites.Length - 1;	
			} 
			
			frame = spriteIdx - range[0];

			// Change image on first present renderer
			if (spriteRenderer != null) spriteRenderer.sprite = sprites[spriteIdx];
			else if (imageRenderer != null) imageRenderer.sprite = sprites[spriteIdx];
			else if (meshRenderer != null) meshRenderer.material.mainTexture = sprites[spriteIdx].texture;
			else throw new Exception("This component doesn't have any sort of visual renderer");	
		}

		protected void ChangeSpeed(float speed) {
			ticksPerFrame = speed;
		}

		protected void ChangeState(int[] newRange, string name = "na", int offset = 0, float newTick = 0) {
			range = newRange;
			tick = newTick;
			spriteIdx = range[0] + offset;
			frame = offset;
			animationState = name;
		}

		// ---
		// Event

		private void ExecuteAnimationEvents() {
			if (events == null) {
				return;
			}
			for (int i = 0; i < events.Length; i++){
				AnimationEvent item = events[i];
				if (IncludesString(item.states, animationState) && frame == item.frame) {
					if (executedFrame != frame) {
						item.action();
						executedFrame = frame;
						executedState = animationState;
					}
				}
				if (frame != executedFrame || animationState != executedState) {
					executedFrame = -1;
				}
			}
		}

		private bool IncludesString(string[] arr, string target) {
			foreach (var el in arr) {
				if (el == target) {
					return true;
				}
			}
			return false;
		}
	}
}
