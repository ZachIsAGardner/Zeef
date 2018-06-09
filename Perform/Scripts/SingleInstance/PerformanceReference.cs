using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ---
using Zeef.GameManager;

namespace Zeef.Perform 
{
	[RequireComponent(typeof (SingleInstanceChild))]
	public class PerformanceReference : MonoBehaviour 
	{
		public float crawlSpeed = 0.1f;

		public GameObject textBoxPrefab;
		public GameObject responseBoxPrefab;
		public GameObject border;

		public Vector3 defaultTextBoxOffset = new Vector3(0, 128, -18);

		public Vector3 textBoxCameraPosition = new Vector3(0, 64, 1);
		public Vector2 textBoxCameraSize = new Vector2(256, 64);

		public static PerformanceReference Main() 
		{
			return SingleInstance.Main().GetComponentInChildren<PerformanceReference>();
		}
	}
}
