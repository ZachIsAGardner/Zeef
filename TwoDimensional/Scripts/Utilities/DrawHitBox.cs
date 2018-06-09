using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional {

	public class DrawHitBox : MonoBehaviour {

		public Color color = new Color(1, 1, 1, 0.5f);
		public Vector2 defaultSize = new Vector2(1,1);

		void OnDrawGizmos() {
			Vector3 size = Vector3.one;
			Vector3 offset = Vector3.zero;
			BoxCollider2D box = GetComponent<BoxCollider2D>();

			if (box != null) {
				size = box.bounds.size;
				offset = box.offset;
			} else {
				size = defaultSize;
			}

			Gizmos.color = color;
			Gizmos.DrawCube(transform.position + offset, size);
		}
	}

}
