using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef {

	public class SingleInstance<T> : MonoBehaviour {

		protected static T instance;
		protected static T GetInstance() {
			if (instance == null) 
				throw new Exception($"No {typeof(T).Name} exists. Yet one was requested for.");
			else 
				return instance;
		}

		protected virtual void Awake() {
			if (instance != null && !instance.Equals(null))
				throw new Exception($"Only one {typeof(T).Name} may be set at a time.");
			instance = GetComponent<T>();
		}
	}
}