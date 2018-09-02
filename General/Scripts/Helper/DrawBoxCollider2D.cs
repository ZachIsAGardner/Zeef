using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef {

	public class DrawBoxCollider2D : MonoBehaviour {

		[SerializeField] Color color = new Color(1, 1, 1, 0.5f);

		void OnDrawGizmos() {
			Vector3 size = Vector3.one;
			Vector3 offset = Vector3.zero;
			BoxCollider2D box = GetComponent<BoxCollider2D>();

			if (box != null) {
				size = box.bounds.size;
				offset = box.offset;
			}

			Gizmos.color = color;
			Gizmos.DrawCube(transform.position + offset, size);
		}
	}

}
