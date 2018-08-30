using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.GameManagement {
	
	// Prompt
	[CreateAssetMenu(menuName ="SO/Prompt")]
	public class PromptScriptable : ScriptableObject {

		public PromptsEnum ID;
		public GameObject Prefab;
	}
}
