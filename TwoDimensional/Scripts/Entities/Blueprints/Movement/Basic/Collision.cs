using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is heavily influenced by Sebastian Lague's platformer repo
// https://github.com/SebLague/2DPlatformer-Tutorial
namespace Zeef.TwoDimensional {
	public class Collision : RaycastController {

		public LayerMask mask;	

		HorizontalSlopeInfo horizontalSlopeInfo;
		VerticalSlopeInfo verticalSlopeInfo;
		public CollisionInfo collisions;

		private float smoothing;

		public struct CollisionInfo {
			public bool up, down, left, right;
			public Collider2D col;

			public void Reset() {
				col = null;
				up = down = false;
				left = right = false;
			
			}
		}

		struct HorizontalSlopeInfo {
			public float slopeAngle, slopeAngleOld;
			public bool climbingSlope;

			public void Reset() {
				slopeAngleOld = slopeAngle;
				slopeAngle = 0;

				climbingSlope = false;
			}
		}

		struct VerticalSlopeInfo {
			public float slopeAngle, slopeAngleOld;
			public bool climbingSlope;

			public void Reset() {
				slopeAngleOld = slopeAngle;
				slopeAngle = 0;

				climbingSlope = false;
			}
		}

		public void Move(Vector2 vel) {
			collisions.Reset();
			horizontalSlopeInfo.Reset();
			verticalSlopeInfo.Reset();

			GetRayOrigins();
			HorizontalCollisions(ref vel);
			if (vel.y != 0) {
				VerticalCollisions(ref vel);
			}
			transform.Translate(vel);
		}	
		// For moving platform
		public void Move(Vector2 vel, bool forceGrounded) {
			collisions.Reset();
			collisions.down = forceGrounded;
			GetRayOrigins();
			HorizontalCollisions(ref vel);
			if (vel.y != 0) {
				VerticalCollisions(ref vel);
			}
			transform.Translate(vel);
		}	

		#region Horizontal

		#region Slope

		void HorizontalAdjustForSlope(RaycastHit2D hit, ref Vector2 vel) {
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			float dir = Mathf.Sign(vel.x);

			// check for descending slope
			float distanceToSlopeStart = 0;

			if (slopeAngle != horizontalSlopeInfo.slopeAngleOld) {
				distanceToSlopeStart = hit.distance - skin;
				vel.x -= distanceToSlopeStart * dir;
			}

			HorizontalClimbSlope(ref vel, slopeAngle, hit.normal);
			vel.x += distanceToSlopeStart * dir;
		}

		void HorizontalClimbSlope(ref Vector2 vel, float slopeAngle, Vector2 slopeNormal) {
			float moveDistance = Mathf.Abs(vel.x);
			float climbAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

			if (vel.y <= climbAmountY) {
				vel.y = climbAmountY;
				vel.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (vel.x);
				collisions.down = true;
				horizontalSlopeInfo.climbingSlope = true;
			}
		}

		#endregion

		void HorizontalCollisions(ref Vector2 vel) {
			float dir = Mathf.Sign(vel.x);
			float length = Mathf.Abs (vel.x) + skin;
			float spacing = col.bounds.size.y / (rayCount - 1);

			if (Mathf.Abs(vel.x) < skin) {
				length = 2 * skin;
			}

			for (int i = 0; i < rayCount; i ++) {

				Vector2 rayOrigin = (dir == -1) ? origins.bottomLeft : origins.bottomRight;
				rayOrigin += Vector2.up * (spacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * dir, length, mask);

				Debug.DrawRay(rayOrigin, Vector2.right * dir, Color.red);

				if (hit) {

					if (hit.distance == 0 || hit.collider.tag == "Through") {
						continue;
					}
					// Make sure not setting vel to 0 on left or right collision if using slope
					// if (i == 0) {
					// 	HorizontalAdjustForSlope(hit, ref vel);
					// }

					vel.x = (hit.distance - skin) * dir;
					length = hit.distance;

					collisions.left = dir == -1;
					collisions.right = dir == 1;
					collisions.col = hit.collider;
					
				}
			}
		}

		#endregion

		#region Vertical

		#region Slope

		void VerticalAdjustForSlope(RaycastHit2D hit, ref Vector2 vel) {
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			float dir = Mathf.Sign(vel.y);

			float distanceToSlopeStart = 0;

			if (slopeAngle != verticalSlopeInfo.slopeAngleOld) {
				distanceToSlopeStart = hit.distance - skin;
				vel.y -= distanceToSlopeStart * dir;
			}

			VerticalClimbSlope(ref vel, slopeAngle, hit.normal);
			vel.y += distanceToSlopeStart * dir;
		}

		void VerticalClimbSlope(ref Vector2 vel, float slopeAngle, Vector2 slopeNormal) {
			float moveDistance = Mathf.Abs(vel.y);
			float climbAmountX = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance;

			if (vel.x <= climbAmountX) {
				vel.y = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (vel.y);
				vel.x = climbAmountX;
			}
		}

		#endregion

		void VerticalCollisions(ref Vector2 vel) {
			float dir = Mathf.Sign(vel.y);
			float length = Mathf.Abs (vel.y) + skin;
			float spacing = col.bounds.size.x / (rayCount - 1);

			for (int i = 0; i < rayCount; i ++) {

				Vector2 rayOrigin = (dir == -1) ? origins.bottomLeft : origins.topLeft;
				rayOrigin += Vector2.right * (spacing * i + vel.x);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * dir, length, mask);

				Debug.DrawRay(rayOrigin, Vector2.up * dir, Color.red);

				if (hit) {

					if (hit.collider.tag == "Through" && vel.y >= 0) {
						continue;
					}

					// if (i == 0) {
					// 	VerticalAdjustForSlope(hit, ref vel);
					// }

					vel.y = (hit.distance - skin) * dir;
					length = hit.distance;

					collisions.down = dir == -1;
					collisions.up = dir == 1;
					collisions.col = hit.collider;
					
				}
			}
		}
		#endregion
	}
}
