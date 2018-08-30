using System.Collections.Generic;
using UnityEngine;

namespace Zeef.GameManagement {
	
    [CreateAssetMenu(menuName ="SOs Container/Prompt")]
	public class PromptScriptablesContainer : ScriptableObject {
		public List<PromptScriptable> Prompts;
	}
}
