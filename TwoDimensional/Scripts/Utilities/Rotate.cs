using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional 
{
	public class Rotate : MonoBehaviour 
	{
		public Vector3 Eulers;

		void Update () 
		{
			transform.Rotate(Eulers * Time.deltaTime);		
		}
	}
}
