using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional {

	// this goes on the parent gameobject of the sprite
	public class CorrectLayer : MonoBehaviour {

		void Update() {
			transform.position = new Vector3(
				transform.position.x,
				transform.position.y,
				GetZPosition()
			);
		}

		float GetZPosition() {
			// Get z from y
			// as y goes down z goes down
			return transform.position.y;
		}
	}

}
