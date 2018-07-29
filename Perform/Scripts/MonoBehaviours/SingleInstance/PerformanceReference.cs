using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ---
using Zeef.GameManagement;

namespace Zeef.Perform 
{
	[RequireComponent(typeof (SingleInstanceChild))]
	public class PerformanceReference : MonoBehaviour 
	{
		// Options
		public float crawlSpeed = 0.1f;

		public Vector3 defaultTextBoxOffset = new Vector3(0, 128, -18);

		public Vector3 textBoxCameraPosition = new Vector3(0, 64, 1);
		public Vector2 textBoxCameraSize = new Vector2(256, 64);

		// GameObjects
		public GameObject textBoxPrefab;
		public GameObject responseBoxPrefab;
		public GameObject border;

		public static PerformanceReference Main() 
		{
			return SingleInstance.Main().GetComponentInChildren<PerformanceReference>();
		}
	}
}
