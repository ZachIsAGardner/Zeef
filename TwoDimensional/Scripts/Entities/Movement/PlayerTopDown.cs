using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

    public class PlayerTopDown : MovingObject2D {

		[Required]
		[SerializeField] private SpriteRenderer spriteRenderer;

		protected override void Update() {
			if (GameManager.IsLoading()) return;
			base.Update();
			ChangeFacing();
		}
		        
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