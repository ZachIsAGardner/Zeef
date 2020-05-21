using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ---
using Zeef.GameManagement;
using Zeef.Menu;

namespace Zeef.Perform
{
	public class PerformanceContent : SingleInstance<PerformanceContent>
    {
		[SerializeField] private TextBoxUI textBoxPrefab;
		public static TextBoxUI TextBoxPrefab { get { return Instance.textBoxPrefab; }}

		[SerializeField] private LinearMenuSelect responseBoxPrefab;
		public static LinearMenuSelect ResponseBoxPrefab { get { return Instance.responseBoxPrefab; }}

		[SerializeField] private GameObject border;
		public static GameObject Border { get { return Instance.border; }}

        [SerializeField] private float defaultCrawlTime = 0.05f;
        public static float DefaultCrawlTime { get { return Instance.defaultCrawlTime; } }
    }
}
