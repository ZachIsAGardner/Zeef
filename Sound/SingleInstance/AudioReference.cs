using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// this holds references to all the songObjects.songs and sfx
namespace Zeef.Sound 
{
	[RequireComponent(typeof (SingleInstanceChild))]
	public class AudioReference : MonoBehaviour
	{
		[SerializeField] private float musicVolume = 0.5f;
		[SerializeField] private float soundEffectVolume = 1;

		[SerializeField] private SongObjectsContainer songsObjectsContainer;	    
		[SerializeField] private SoundEffectObjectsContainer soundEffectObjectsContainer;

		public static AudioReference Main() => SingleInstance.Main().GetComponentInChildrenWithError<AudioReference>(); 
		
		#region Music

		public float MusicVolume() => musicVolume;
		

		public SongObject GetSong(SongsEnum id) => songsObjectsContainer.songs.First(s => s.id == id);
		

		public SongObject GetSong(string name) => songsObjectsContainer.songs.First(s => s.name == name);
		

		#endregion

		#region SFX

		public float SoundEffectVolume() => soundEffectVolume;
		

		public SoundEffectObject GetSoundEffect(SoundEffectsEnum id) 
		{
			SoundEffectObject result = soundEffectObjectsContainer.soundEffects.FirstOrDefault(s => s.id == id);
			return result;
		}

		public SoundEffectObject GetSoundEffect(string name) 
		{
			SoundEffectObject result = soundEffectObjectsContainer.soundEffects.FirstOrDefault(s => s.name == name);
			return result;
		}

		#endregion	
	}
}