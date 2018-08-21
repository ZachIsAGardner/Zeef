using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.GameManagement 
{
	[CreateAssetMenu(menuName ="SO/Prompt")]
	public class PromptObject : ScriptableObject {
		public PromptsEnum id;
		public GameObject prefab;
	}
}
