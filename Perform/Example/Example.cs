using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Perform.Example {

    public class Example : MonoBehaviour {

		[SerializeField] private Performance performance;

		async void Start() {
			await performance.ExecuteAsync();
		}
	}
}
