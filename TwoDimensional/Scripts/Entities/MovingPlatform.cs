using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zeef.TwoDimensional {

	// This is heavily influenced by Sebastian Lague's platformer repo
	// https://github.com/SebLague/2DPlatformer-Tutorial
	public class MovingPlatform : RaycastController {

		public LayerMask passengerMask;

		List<PassengerMovement> passengerMovements;
		Transform[] waypoints;
		Transform platform;
		int currentPoint;

		protected override void Start() {
			platform = GetComponentsInChildren<Transform>().First((p) => p.name == "platform");
			col = platform.GetComponent<BoxCollider2D>();

			waypoints = GetComponentsInChildren<Transform>().Where((w) => w.name.Contains("point")).ToArray();

		}

		void Update () {
			GetRayOrigins();

			Loop();
			MovePassengers();
		}

		void Loop() {
			if (waypoints.Length <= 0) {
				return;
			}
			Vector3 oldPosition = platform.position;
			platform.position = Vector3.MoveTowards(platform.position, waypoints[currentPoint].position, 1);

			Vector3 vel = platform.position - oldPosition;
			FindPassengers(vel);

			if (platform.position == waypoints[currentPoint].position) {
				currentPoint = (currentPoint >= waypoints.Length - 1) ? 0 : currentPoint + 1;
			}
		}

		void MovePassengers() {
			foreach (var movement in passengerMovements)
			{
				movement.transform.gameObject.GetComponent<Collision>().Move(movement.velocity, true);
			}
		}

		void FindPassengers(Vector3 vel) {
			HashSet<Transform> movedPassengers = new HashSet<Transform>();
			passengerMovements = new List<PassengerMovement>();

			Vector3 vel2 = Vector3.up;

			float dir = Mathf.Sign(vel2.y);
			float length = Mathf.Abs (vel2.y) + skin;
			float spacing = col.bounds.size.x / (rayCount - 1);

			for (int i = 0; i < rayCount; i ++) {

				Vector2 rayOrigin = (dir == -1) ? origins.bottomLeft : origins.topLeft;
				rayOrigin += Vector2.right * (spacing * i + vel2.x);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * dir, length, passengerMask);

				Debug.DrawRay(rayOrigin, Vector2.up * dir * 10, Color.red);

				if (hit) {
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);
						passengerMovements.Add(new PassengerMovement(hit.transform, vel, true, false));
					}
				}
			}
		}

		struct PassengerMovement {
			public Transform transform;
			public Vector3 velocity;
			public bool onPlatform;
			public bool moveBeforePlatform;

			public PassengerMovement(Transform newTransform, Vector3 newVelocity, bool newOnPlatform, bool newMoveBeforePlatform) {
				transform = newTransform;
				velocity = newVelocity;
				onPlatform = newOnPlatform;
				moveBeforePlatform = newMoveBeforePlatform;
			}
		}
	}

}