using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ---
using Zeef.TwoDimensional;

namespace Zeef.Perform {
	public class PerformanceTrigger : InteractableObject {

		public Performance performance;
		public bool onSceneStart;

		protected override void Start() {
			base.Start();
			if (onSceneStart) {
				StartPerformance();
			}
		}

		public void SetTriggered(bool boolean) {
			triggered = boolean;
		}

		protected override void TriggerAction() {
			// Delay so that first text box doesn't auto skip to end
			StartCoroutine(DelayStartPerformance());
			HidePrompt();
		}

		IEnumerator DelayStartPerformance() {
			yield return new WaitForSeconds(0.1f);
			StartPerformance();
		}

		void StartPerformance() {
			if (performance.performing) return;

			performance.StartPerformance(this);
		}

	}
}
