using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

	[SerializeField] Vector3 eulers;

	void Update () {
		transform.Rotate(eulers * Time.deltaTime);		
	}
}
