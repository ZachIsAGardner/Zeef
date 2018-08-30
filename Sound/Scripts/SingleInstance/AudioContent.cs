using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// this holds references to all the songObjects.songs and sfx
namespace Zeef.Sound {

	public class AudioContent : MonoBehaviour {

		private static AudioContent audioContent;

		[SerializeField] private SongScriptablesContainer songsScriptablesContainer;	    
		[SerializeField] private SoundEffectScriptablesContainer soundEffectScriptablesContainer;

		void Awake() {
			if (audioContent != null) throw new Exception("Only one AudioContent my exist at a time");
			audioContent = this;
		}
		
		// ---
		// Music
		
		public static SongScriptable GetSong(SongsEnum id) => audioContent.songsScriptablesContainer.Songs.First(s => s.ID == id);
		
		public static SongScriptable GetSong(string name) => audioContent.songsScriptablesContainer.Songs.First(s => s.name == name);
		
		// ---
		// SFX
		
		public static SoundEffectScriptable GetSoundEffect(SoundEffectsEnum id) =>
			audioContent.soundEffectScriptablesContainer.SoundEffects.FirstOrDefault(s => s.ID == id);

		public static SoundEffectScriptable GetSoundEffect(string name) =>
			audioContent.soundEffectScriptablesContainer.SoundEffects.FirstOrDefault(s => s.name == name);	
	}
}