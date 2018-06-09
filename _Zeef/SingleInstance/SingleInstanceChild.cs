using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SingleInstanceChild : MonoBehaviour 
{
	void Update() 
	{
		if (GetComponentInParent<SingleInstance>() == null)
			throw new Exception("SingleInstanceChild must be placed on or within a SingleInstance gameobject");
	}
}
