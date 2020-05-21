using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef
{
	/// <summary>
	/// A MonoBehaviour inherits from SingleInstance to ensure that only one instance of it can ever exist.
	/// This Provides a static reference to the instance of the MonoBehaviour.
	///</summary>
	public class SingleInstance<T> : MonoBehaviour
    {
		private static T instance;

		protected static T Instance
		{
			get
			{
				if (instance == null) 
				{
					Debug.Log($"No {typeof(T).Name} exists. Yet one was requested for.");
				}

				return instance;
			}
		}

		protected virtual void Awake()
        {
			if (instance != null && !instance.Equals(null))
				throw new Exception($"Only one {typeof(T).Name} may be set at a time.");

			instance = GetComponent<T>();
		}
	}
}