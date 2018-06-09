using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Sound {
	[CreateAssetMenu(menuName="ScriptableObjects/SoundEffect")]
	public class SoundEffectObject : ScriptableObject {
		public SoundEffectID id;
		public AudioClip clip;
		[Range(0, 1)]
		public float volume = 1;
	}
}
