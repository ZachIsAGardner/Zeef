using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// this holds references to all the songObjects.songs and sfx
namespace Zeef.Sound {

	public class AudioContent : MonoBehaviour {

		private static AudioContent audioContent;

		[SerializeField] private SongObjectsContainer songsObjectsContainer;	    
		[SerializeField] private SoundEffectObjectsContainer soundEffectObjectsContainer;

		void Awake() {
			if (audioContent != null) throw new Exception("Only one AudioContent my exist at a time");
			audioContent = this;
		}
		
		// ---
		// Music
		
		public static SongObject GetSong(SongsEnum id) => audioContent.songsObjectsContainer.songs.First(s => s.id == id);
		
		public static SongObject GetSong(string name) => audioContent.songsObjectsContainer.songs.First(s => s.name == name);
		
		// ---
		// SFX
		
		public static SoundEffectObject GetSoundEffect(SoundEffectsEnum id) =>
			audioContent.soundEffectObjectsContainer.soundEffects.FirstOrDefault(s => s.id == id);

		public static SoundEffectObject GetSoundEffect(string name) =>
			audioContent.soundEffectObjectsContainer.soundEffects.FirstOrDefault(s => s.name == name);	
	}
}