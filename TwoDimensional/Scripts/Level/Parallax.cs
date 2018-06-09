using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional {

	public class Parallax : MonoBehaviour {

		Vector3 startPos;
		public float distance = 30;

		void Start() {
			startPos = transform.position;
		}

		void SetPosition() {
			if (distance == 0) {
				return;
			}
			Vector3 offset = startPos - transform.parent.transform.position;
			transform.position = new Vector3(
				Camera.main.transform.position.x + (offset.x / distance),
				transform.localPosition.y,
				transform.localPosition.z
			);
		}

		void Update( ){
			SetPosition();
		}
	}

}
