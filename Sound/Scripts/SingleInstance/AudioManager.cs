using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Sound {
	
	[RequireComponent (typeof(AudioSource))]
	public class AudioManager : SingleInstance<AudioManager> {

		[Range (0, 1)]
		[SerializeField] private float musicVolume = 0.5f;
		public static float MusicVolume { get { return GetInstance().musicVolume; } }

		[Range (0, 1)]
		[SerializeField] private float soundEffectVolume = 1;
		public static float SoundEffectVolume { get { return GetInstance().soundEffectVolume; } }

		private AudioSource audioSource;
		private SongScriptable currentSong;

		protected override void Awake() {
			base.Awake();

			audioSource = this.GetComponentWithError<AudioSource>();
			audioSource.volume = MusicVolume;
		}
		
		// ---
		// Music

		public static void ChangeSong(SongsEnum songID) {
			ChangeSong(AudioContent.GetSong(songID));
		}

		public static void ChangeSong(SongScriptable song) {
			// Stop if were already playing that song
			if (song == GetInstance().currentSong) return;

			GetInstance().currentSong = song;

			GetInstance().StopAllCoroutines();

			GetInstance().audioSource.Stop();

			if (song == null) return;

			GetInstance().audioSource.clip = song.Clip;
			GetInstance().audioSource.Play();

			GetInstance().audioSource.volume = MusicVolume;

			GetInstance().StartCoroutine(LoopSongCoroutine(song));
		}

		private static IEnumerator LoopSongCoroutine(SongScriptable song) {
			while (true) {
				if ((song.LoopTime.End > 0 && GetInstance().audioSource.time > song.LoopTime.End) 
				|| (GetInstance().audioSource.time > GetInstance().audioSource.clip.length - 0.025f)) {
					GetInstance().audioSource.time = song.LoopTime.Start;
				}
				
				yield return null;
			}
		}

		// ---
		// SFX

		public static void PlaySoundEffect(AudioSource source, SoundEffectScriptable obj) {
			if (obj == null) return;
			source.PlayOneShot(obj.Clip, SoundEffectVolume - (1 - obj.Volume));
		}

		public static void PlaySoundEffect(AudioSource source, SoundEffectsEnum id) {
			PlaySoundEffect(source, AudioContent.GetSoundEffect(id));
		}

		public static void PlaySoundEffect(AudioSource source, string name) {
			PlaySoundEffect(source, AudioContent.GetSoundEffect(name));
		}
	}
}