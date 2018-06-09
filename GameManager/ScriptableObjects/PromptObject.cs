using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.GameManager 
{
	[CreateAssetMenu(menuName ="ScriptableObjects/Prompt")]
	public class PromptObject : ScriptableObject {
		public PromptID id;
		public GameObject prefab;
	}
}
