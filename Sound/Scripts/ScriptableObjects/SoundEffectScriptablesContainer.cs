using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Sound 
{
	[CreateAssetMenu(menuName = "SOs Container/Sound Effect")]
	public class SoundEffectScriptablesContainer : ScriptableObject 
	{
		public List<SoundEffectScriptable> SoundEffects;
	}
}