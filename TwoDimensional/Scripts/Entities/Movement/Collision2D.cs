using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is heavily influenced by Sebastian Lague's platformer repo
// Check out his stuff, he's a cool dude.
// https://github.com/SebLague/2DPlatformer-Tutorial
namespace Zeef.TwoDimensional {

	[RequireComponent(typeof(BoxCollider2D))]
	public class Collision2D : MonoBehaviour {

		[SerializeField] private LayerMask layerMask;	
		[SerializeField] private float skin = 0.1f;
		[SerializeField] private int rayCount = 4;

		private CollisionInfo collisions;
		public CollisionInfo Collisions { get { return collisions; } }

		private OriginInfo origins;
		private HorizontalSlopeInfo horizontalSlopeInfo;
		private VerticalSlopeInfo verticalSlopeInfo;

		// ---

		protected void GetRayOrigins() {
			BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
			Bounds bounds = boxCollider2D.bounds;
			bounds.Expand (skin * -2);

			origins.BottomLeft = new Vector2(boxCollider2D.bounds.min.x, boxCollider2D.bounds.min.y);
			origins.BottomRight = new Vector2(boxCollider2D.bounds.max.x, boxCollider2D.bounds.min.y);
			origins.TopLeft = new Vector2(boxCollider2D.bounds.min.x, boxCollider2D.bounds.max.y);
			origins.TopRight = new Vector2(boxCollider2D.bounds.max.x, boxCollider2D.bounds.max.y);
		}

		// ---

		/// <summary>
		/// Moves the entity. Provided the velocity to attempt to move the entity with. 
		/// Velocity will change based on collision.
		/// </summary>
		public void Move(Vector2 vel) {
			// Reset
			collisions.Reset();
			horizontalSlopeInfo.Reset();
			verticalSlopeInfo.Reset();
			
			GetRayOrigins();

			// Adjust vel based on collisions
			HorizontalCollisions(ref vel);
			if (vel.y != 0) VerticalCollisions(ref vel);

			// Move object with velocity
			transform.Translate(vel);
		}	

		/// <summary>
		/// Moves the entity while on a moving platform.
		/// </summary>
		public void Move(Vector2 vel, bool forceGrounded) {
			collisions.Reset();
			collisions.Down = forceGrounded;
			GetRayOrigins();
			HorizontalCollisions(ref vel);
			// if (vel.y != 0) {
			// 	VerticalCollisions(ref vel);
			// }
			transform.Translate(vel);
		}	

		// ---
		// Horizontal

		private void HorizontalAdjustForSlopeBottom(RaycastHit2D hit, ref Vector2 vel) {
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			float dir = Mathf.Sign(vel.x);

			if (slopeAngle == 90)
				return;

			vel.y = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(vel.x);
			vel.x = (Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(vel.x)) * dir;

			collisions.Down = true;
		}

		private void HorizontalAdjustForSlopeTop(RaycastHit2D hit, ref Vector2 vel) {

			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			float dir = Mathf.Sign(vel.x);

			if (slopeAngle == 90)
				return;

			if (vel.x > vel.y) {
				vel.y = -(Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(vel.x));
				vel.x = ((Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(vel.x)) * dir);
			}
			else 
			{
				float attemptX = (Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(vel.y));
				vel.y = (Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(vel.y));
				vel.x = attemptX;
			}

			collisions.Up = true;
		}

		void HorizontalCollisions(ref Vector2 vel) {
			float dir = Mathf.Sign(vel.x);
			float length = Mathf.Abs (vel.x) + skin;
			float spacing = GetComponent<BoxCollider2D>().bounds.size.y / (rayCount - 1);

			if (Mathf.Abs(vel.x) < skin) length = 2 * skin;
			
			for (int i = 0; i < rayCount; i ++) {

				Vector2 rayOrigin = (dir == -1) ? origins.BottomLeft : origins.BottomRight;
				rayOrigin += Vector2.up * (spacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * dir, length, layerMask);

				Debug.DrawRay(rayOrigin, Vector2.right * dir, Color.red);

				if (hit) {

					if (hit.distance == 0 || hit.collider.tag == "Through") continue;
					
					// Make sure not setting vel to 0 on left or right collision if using slope
					// check bottom raycast only
					if (i == 0) {
						// HorizontalAdjustForSlopeBottom(hit, ref vel);
					}
					// check top raycast only
					else if (i == rayCount - 1){
						// HorizontalAdjustForSlopeTop(hit, ref vel);
					}

					vel.x = (hit.distance - skin) * dir;
					length = hit.distance;

					collisions.Left = dir == -1;
					collisions.Right = dir == 1;
					collisions.col = hit.collider;
					
				}
			}
		}

		// ---
		// Vertical

		// void VerticalAdjustForSlope(RaycastHit2D hit, ref Vector2 vel) {
		// 	float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
		// 	float dir = Mathf.Sign(vel.y);

		// 	float distanceToSlopeStart = 0;

		// 	if (slopeAngle != verticalSlopeInfo.SlopeAngleOld) {
		// 		distanceToSlopeStart = hit.distance - skin;
		// 		vel.y -= distanceToSlopeStart * dir;
		// 	}

		// 	VerticalClimbSlope(ref vel, slopeAngle, hit.normal);
		// 	vel.y += distanceToSlopeStart * dir;
		// }

		// void VerticalClimbSlope(ref Vector2 vel, float slopeAngle, Vector2 slopeNormal) {
		// 	float moveDistance = Mathf.Abs(vel.y);
		// 	float climbAmountX = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance;

		// 	if (vel.x <= climbAmountX) {
		// 		vel.y = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (vel.y);
		// 		vel.x = climbAmountX;
		// 	}
		// }

		void VerticalCollisions(ref Vector2 vel) {
			float dir = Mathf.Sign(vel.y);
			float length = Mathf.Abs (vel.y) + skin;
			float spacing = GetComponent<BoxCollider2D>().bounds.size.x / (rayCount - 1);

			for (int i = 0; i < rayCount; i ++) {

				Vector2 rayOrigin = (dir == -1) ? origins.BottomLeft : origins.TopLeft;
				rayOrigin += Vector2.right * (spacing * i + vel.x);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * dir, length, layerMask);

				Debug.DrawRay(rayOrigin, Vector2.up * dir, Color.red);

				if (hit) {

					if (hit.collider.tag == "Through" && vel.y >= 0) continue;

					if (i == 0) 
						HorizontalAdjustForSlopeTop(hit, ref vel);

					vel.y = (hit.distance - skin) * dir;
					length = hit.distance;

					collisions.Down = dir == -1;
					collisions.Up = dir == 1;
					collisions.col = hit.collider;	
				}
			}
		}

		// ---
		// Structs

		public struct OriginInfo {
			public Vector2 BottomLeft, BottomRight, TopLeft, TopRight;
		}

		public struct CollisionInfo {
			public bool Up, Down, Left, Right;
			public Collider2D col;

			public void Reset() {
				col = null;
				Up = Down = false;
				Left = Right = false;
			
			}
		}

		private struct HorizontalSlopeInfo {
			public float SlopeAngle, SlopeAngleOld;
			public bool ClimbingSlope;

			public void Reset() {
				SlopeAngleOld = SlopeAngle;
				SlopeAngle = 0;

				ClimbingSlope = false;
			}
		}

		private struct VerticalSlopeInfo {
			public float SlopeAngle, SlopeAngleOld;
			public bool ClimbingSlope;

			public void Reset() {
				SlopeAngleOld = SlopeAngle;
				SlopeAngle = 0;

				ClimbingSlope = false;
			}
		}
	}
}
