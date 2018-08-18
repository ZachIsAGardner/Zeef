using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
// ---
using Zeef.TwoDimensional;

namespace Zeef.Perform {
	public class PerformanceTrigger : InteractableObject {

		[SerializeField] Performance performance;
		[SerializeField] bool onSceneStart;

		async void Start() {
			if (onSceneStart) await performance.ExecuteAsync();
		}

		public void ChangePerformance(Performance performance) {
			this.performance = performance;
		}

		protected override async Task TriggerActionAsync() {
			HidePrompt();

			triggered = true;
			await performance.ExecuteAsync();
			
			triggered = false;
		}
	}
}
