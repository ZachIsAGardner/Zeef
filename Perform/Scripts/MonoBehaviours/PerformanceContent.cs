using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ---
using Zeef.GameManagement;
using Zeef.Menu;

namespace Zeef.Perform {

	public class PerformanceContent : MonoBehaviour {

		private static PerformanceContent performanceContent;

		[SerializeField] private TextBoxUI textBoxPrefab;
		public static TextBoxUI TextBoxPrefab { get { return performanceContent.textBoxPrefab; }}

		[SerializeField] private VerticalMenuSelectUI responseBoxPrefab;
		public static VerticalMenuSelectUI ResponseBoxPrefab { get { return performanceContent.responseBoxPrefab; }}

		[SerializeField] private GameObject border;
		public static GameObject Border { get { return performanceContent.border; }}

		void Start() {
			if (performanceContent != null) throw new Exception("Only one PerfomanceContent may exist.");
			performanceContent = this;
		}
	}
}
