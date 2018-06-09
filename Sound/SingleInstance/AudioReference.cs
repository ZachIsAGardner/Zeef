using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zeef.GameManager;

// this holds references to all the songObjects.songs and sfx
namespace Zeef.Sound 
{
	[RequireComponent(typeof (SingleInstanceChild))]
	public class AudioReference : MonoBehaviour
	{
		[SerializeField] float musicVolume = 0.5f;
		[SerializeField] float soundEffectVolume = 1;

		[SerializeField] SongObjects songObjects;	    
		[SerializeField] SoundEffectObjects soundEffectsObjects;

		public static AudioReference Main() {
			return SingleInstance.Main().GetComponentInChildren<AudioReference>(); 
		}

		#region Music

		public float MusicVolume() 
		{
			return musicVolume;
		}

		public SongObject GetSong(SongID id) 
		{
			return songObjects.songs.First(s => s.id == id);
		}

		public SongObject GetSong(string name) 
		{
			return songObjects.songs.First(s => s.name == name);
		}

		#endregion

		#region SFX

		public float SoundEffectVolume() 
		{
			return soundEffectVolume;
		}

		public SoundEffectObject GetSoundEffect(SoundEffectID id) 
		{
			SoundEffectObject result = soundEffectsObjects.soundEffects.FirstOrDefault(s => s.id == id);
			return result;
		}

		public SoundEffectObject GetSoundEffect(string name) 
		{
			SoundEffectObject result = soundEffectsObjects.soundEffects.FirstOrDefault(s => s.name == name);
			return result;
		}

		#endregion	
	}
}