using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional.Example {

	public class ExampleSession : SingleInstance<ExampleSession> {
		
		[SerializeField] int playerHealth;
		public static int PlayerHealth { get { return GetInstance().playerHealth; } }

		[SerializeField] float playTime;
		public static float PlayTime { get { return GetInstance().playTime; } }

		public static void UpdateData(int playerHealth) {
			GetInstance().playerHealth = playerHealth;
		}

		void Update() {
			playTime += Time.deltaTime;
		}
	}
}