using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Sound {

	[CreateAssetMenu(menuName="SO/Sound Effect")]
	public class SoundEffectScriptable : ScriptableObject {
		public SoundEffectsEnum ID;
		
		public AudioClip Clip;

		[Range(0, 1)]
		public float Volume = 1;
	}
}
