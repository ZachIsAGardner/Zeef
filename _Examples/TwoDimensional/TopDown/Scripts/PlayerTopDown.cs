using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zeef.GameManagement;
using Zeef.Menu;

namespace Zeef.TwoDimensional.Example {

	[RequireComponent (typeof(LivingObject))]
    public class PlayerTopDown : MovingObject2D {

		[Required]
		[SerializeField] private SpriteRenderer spriteRenderer;
		private BarUI barUI;

		private LivingObject livingObject;

		protected override void Start() {
			base.Start();

			GameManager.BeforeLeaveScene += OnBeforeLeaveScene;

			// Get and setup LivingObject
			livingObject = GetComponent<LivingObject>();
			if (ExampleSession.PlayerHealth > 0) livingObject.Health = ExampleSession.PlayerHealth;
			livingObject.AfterSurviveTakeDamage += OnAfterSurviveTakeDamage;
			livingObject.BeforeDie += OnBeforeDie;

			// Get and setup BarUI
			barUI = FindObjectOfType<BarUI>(); // bad
			barUI.UpdateDisplayAsync(livingObject.HealthPercentage, 0);
		}

		public async void OnBeforeLeaveScene(object source, EventArgs args) {
			ExampleSession.UpdateData(livingObject.Health);
		}

		public async void OnAfterSurviveTakeDamage(object source, DamageEventArgs args) {
			await barUI.UpdateDisplayAsync(livingObject.HealthPercentage);
		}

		public async void OnBeforeDie(object source, EventArgs args) {
			barUI.UpdateDisplay(0);
			// await GameManager.LoadSceneAsync(
			// 	scene: "_GameOver", 
			// 	loadMode: LoadSceneMode.Additive,
			// 	transition: false
			// );
		}

		protected void OnDestroy() {
			GameManager.BeforeLeaveScene -= OnBeforeLeaveScene;
		}

		protected override void Update() {
			if (GameManager.IsLoading || livingObject.IsFrozen) return;
			base.Update();
			
			ChangeFacing();
		}

		// ---
		        
        protected override void CalculateVelocity(ref Vector2 vel) {
			int inputX = 0;
			if (ControlManager.GetInputHeld(ControlManager.Left)) inputX = -1;
			if (ControlManager.GetInputHeld(ControlManager.Right)) inputX = 1;

			int inputY = 0;
			if (ControlManager.GetInputHeld(ControlManager.Down)) inputY = -1;
			if (ControlManager.GetInputHeld(ControlManager.Up)) inputY = 1;

            vel.x = Mathf.Lerp(vel.x, inputX * moveSpeed, 1 - Mathf.Pow(acc, Time.deltaTime));
            vel.y = Mathf.Lerp(vel.y, inputY * moveSpeed, 1 - Mathf.Pow(acc, Time.deltaTime));
        }

        protected void ChangeFacing() {
			if (Vel.x < -0.1f) spriteRenderer.flipX = true;
			else if (Vel.x > 0.1f) spriteRenderer.flipX = false;
        }
    }
}