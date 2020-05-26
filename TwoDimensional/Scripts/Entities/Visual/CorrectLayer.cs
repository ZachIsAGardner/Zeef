using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional 
{
	// For top down 2D sprites. Sprites south of another 
	// sprite will appear above the other sprite and vice-versa
	// (This goes on the parent gameobject of the sprite)
	[ExecuteInEditMode]
	public class CorrectLayer : MonoBehaviour 
	{
		public GameObject Bottom;
		private float? y;

		void Update() 
		{
			if (transform.position.y == y) return;

			transform.position = new Vector3(
				transform.position.x,
				transform.position.y,
				(Bottom != null) ? Bottom.transform.position.y / 10 : transform.position.y
			);

			y = transform.position.y;
		}
	}
}
