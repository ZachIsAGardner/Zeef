using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// this holds references to all the songObjects.songs and sfx
namespace Zeef.Sound {

	public class AudioContent : SingleInstance<AudioContent> {

		[SerializeField] private List<SongScriptable> songScriptables;	    
		[SerializeField] private List<SoundEffectScriptable> soundEffectScriptables;
						
		public static SongScriptable GetSong(string name) => 
			GetInstance().songScriptables.First(s => s.name == name);
		
		public static SoundEffectScriptable GetSoundEffect(string name) =>
			GetInstance().soundEffectScriptables.First(s => s.name == name);	
	}
}