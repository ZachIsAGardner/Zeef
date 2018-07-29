using System.Collections.Generic;
using UnityEngine;

namespace Zeef.GameManagement 
{
    [CreateAssetMenu(menuName ="SOs Container/Prompt")]
	public class PromptObjectsContainer : ScriptableObject 
	{
		public List<PromptObject> prompts;
	}
}
