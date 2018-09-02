using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ---
using Zeef.GameManagement;

namespace Zeef.TwoDimensional.Example {

    public class PlayerPlatformer : MovingObject2D {

		[SerializeField] SpriteRenderer spriteRenderer;

		[SerializeField] float gravity;
		[SerializeField] float maxJumpStrength;
		[SerializeField] float minJumpStrength;

		// ---

		protected override void Update() {
			if (GameManager.IsLoading()) return;
			base.Update();

			if (transform.position.y < -100)
				transform.position = StartPosition;
			
			ChangeFacing();
		}

        protected override void CalculateVelocity(ref Vector2 vel) {
			// Limit velocity
			if (Collision.Collisions.Down && vel.y < 0) vel.y = 0;
			if (Collision.Collisions.Up && vel.y > 0) vel.y = 0;
			if (Collision.Collisions.Left && vel.x < 0) vel.x = 0;					
			if (Collision.Collisions.Left && vel.x > 0) vel.x = 0;					

			// Apply velocity
			vel.y -= gravity * Time.deltaTime;

			if (ControlManager.GetInputHeld(ControlManager.Left))
				vel.x = vel.x.MoveOverTime(-moveSpeed, acc);
			else if (ControlManager.GetInputHeld(ControlManager.Right))
				vel.x = vel.x.MoveOverTime(moveSpeed, acc);
			else 
				vel.x = vel.x.MoveOverTime(0, acc);

			if (Collision.Collisions.Down && ControlManager.GetInputDown(ControlManager.Accept)) 
				vel.y = maxJumpStrength;

			if (ControlManager.GetInputUp(ControlManager.Accept) && vel.y > minJumpStrength) 
				vel.y = minJumpStrength;
        }

		protected void ChangeFacing() {
			if (Vel.x < -0.1f) spriteRenderer.flipX = true;
			else if (Vel.x > 0.1f) spriteRenderer.flipX = false;
        }
    }
}
