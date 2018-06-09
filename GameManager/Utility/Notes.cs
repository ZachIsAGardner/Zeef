using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.GameManager {
	public class Notes : MonoBehaviour {
		[TextArea(3, 10)]
		public string notes = "Helpful information about the GameObject this is attached to goes here.";
	}
}
