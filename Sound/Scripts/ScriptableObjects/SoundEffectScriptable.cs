using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Sound {

	[CreateAssetMenu(menuName="Scriptables/Sound Effect")]
	public class SoundEffectScriptable : ScriptableObject {		
		public AudioClip Clip;

		[Range(0, 1)]
		public float Volume = 1;
	}
}
