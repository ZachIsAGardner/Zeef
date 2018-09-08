using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional {

	// For top down 2D sprites. Sprites south of another 
	// sprite will appear above the other sprite and vice-versa
	// (This goes on the parent gameobject of the sprite)
	[ExecuteInEditMode]
	public class CorrectLayer : MonoBehaviour {

		private float? y;

		void Update() {
			if (transform.position.y == y) return;

			transform.localPosition = new Vector3(
				transform.position.x,
				transform.position.y,
				transform.position.y / 10
			);

			y = transform.position.y;
		}
	}
}
