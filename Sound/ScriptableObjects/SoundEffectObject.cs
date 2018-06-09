using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Sound {
	[CreateAssetMenu(menuName="SO/Sound Effect")]
	public class SoundEffectObject : ScriptableObject {
		public SoundEffectID id;
		public AudioClip clip;
		[Range(0, 1)]
		public float volume = 1;
	}
}
