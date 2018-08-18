using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Sound 
{
	[CreateAssetMenu(menuName = "SOs Container/Sound Effect")]
	public class SoundEffectObjectsContainer : ScriptableObject 
	{
		public List<SoundEffectObject> soundEffects;
	}
}