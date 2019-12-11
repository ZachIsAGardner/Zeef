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
		public static TextBoxUI TextBoxPrefab { get { return GetInstance().textBoxPrefab; }}

		[SerializeField] private VerticalMenuSelectUI responseBoxPrefab;
		public static VerticalMenuSelectUI ResponseBoxPrefab { get { return GetInstance().responseBoxPrefab; }}

		[SerializeField] private GameObject border;
		public static GameObject Border { get { return GetInstance().border; }}

        [SerializeField] private float defaultCrawlTime = 0.05f;
        public static float DefaultCrawlTime { get { return GetInstance().defaultCrawlTime; } }
    }
}
