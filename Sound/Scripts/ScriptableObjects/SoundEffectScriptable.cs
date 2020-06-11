using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Sound 
{
	[CreateAssetMenu(menuName="Scriptables/Sound Effect")]
	public class SoundEffectScriptable : ScriptableObject 
	{		
		public List<AudioClip> Clips = new List<AudioClip>();

		[Range(0, 1)]
		public float Volume = 1;

		[Range(-3, 3)]
		public float Pitch = 1;
	}
}
