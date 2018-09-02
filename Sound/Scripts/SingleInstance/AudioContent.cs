using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// this holds references to all the songObjects.songs and sfx
namespace Zeef.Sound {

	public class AudioContent : SingleInstance<AudioContent> {

		[SerializeField] private SongScriptablesContainer songsScriptablesContainer;	    
		[SerializeField] private SoundEffectScriptablesContainer soundEffectScriptablesContainer;
		
		// ---
		// Music
		
		public static SongScriptable GetSong(int id) => GetInstance().songsScriptablesContainer.Songs.First(s => s.ID == id);
		
		public static SongScriptable GetSong(string name) => GetInstance().songsScriptablesContainer.Songs.First(s => s.name == name);
		
		// ---
		// SFX
		
		public static SoundEffectScriptable GetSoundEffect(int id) =>
			GetInstance().soundEffectScriptablesContainer.SoundEffects.FirstOrDefault(s => s.ID == id);

		public static SoundEffectScriptable GetSoundEffect(string name) =>
			GetInstance().soundEffectScriptablesContainer.SoundEffects.FirstOrDefault(s => s.name == name);	
	}
}