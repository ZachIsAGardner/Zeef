using System.Collections.Generic;
using UnityEngine;

namespace Zeef.GameManager 
{
    [CreateAssetMenu(menuName ="ScriptableObjectContainers/Prompt")]
	public class PromptObjects : ScriptableObject 
	{
		public List<PromptObject> prompts;
	}
}
