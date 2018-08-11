using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef {
	
	public class SingleInstance : MonoBehaviour  {

		static SingleInstance single;

		void Awake() {
			EnforceSingleInstance();
		}

		public static SingleInstance Main() {
			if (single == null) throw new Exception("No SingleInstance GameObject has been set");
			return single;
		}

		void EnforceSingleInstance() {
			if (single != null) {
				Debug.Log($"Enfore single instance, Destroy self. ({name})");
				Destroy(gameObject);
			} else {
				DontDestroyOnLoad(gameObject);
				single = this;
			}
		}
	}
}
