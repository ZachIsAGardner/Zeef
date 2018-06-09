using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.GameManager {
	// Fulfulls the passed in action in however many seconds
	public class ZeeTimer {
		public bool completed;
		float actionTime = 0;

		Action action;

		public ZeeTimer(Action newAction, float newActionTime) {
			action = newAction;
			actionTime = newActionTime;
		}

		// completed might not be needed
		public bool Run() {
			actionTime -= 1 * Time.deltaTime;
			if (actionTime <= 0 && !completed) {
				action();
				completed = true;
				return true;
			}
			return false;
		}
	}
}
