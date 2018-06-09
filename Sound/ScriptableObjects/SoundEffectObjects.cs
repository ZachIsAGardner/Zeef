using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Sound 
{
	[CreateAssetMenu(menuName = "ScriptableObjectContainers/SoundEffect")]
	public class SoundEffectObjects : ScriptableObject 
	{
		public List<SoundEffectObject> soundEffects;
	}
}