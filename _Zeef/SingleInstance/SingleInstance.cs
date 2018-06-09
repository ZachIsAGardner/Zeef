using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleInstance : MonoBehaviour 
{
	static SingleInstance single;

	void Awake() 
	{
		EnforceSingleInstance();
	}

	public static SingleInstance Main() 
	{
		return single;
	}

	void EnforceSingleInstance() 
	{
		if (single != null) {
			Destroy(gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			single = this;
		}
	}
}
