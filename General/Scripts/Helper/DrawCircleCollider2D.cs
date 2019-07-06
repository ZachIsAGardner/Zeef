using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef {

	public class DrawCircleCollider2D : MonoBehaviour {

		public Color Color = new Color(1, 1, 1, 0.5f);

		void OnDrawGizmos() {
			CircleCollider2D circle = GetComponent<CircleCollider2D>();

			float radius = 1;
			Vector3 offset = Vector3.zero;

			if (circle != null) {
				radius = circle.radius;
				offset = circle.offset;
			}

			Gizmos.color = Color;
			Gizmos.DrawSphere(transform.position + offset, radius);
		}
	}

}
