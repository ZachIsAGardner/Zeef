using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Sound {
	
	[RequireComponent (typeof(AudioSource))]
	public class AudioManager : MonoBehaviour {

		private static AudioManager audioManager;
		private static AudioManager GetAudioManager() {
			if (audioManager == null) 
				throw new Exception("No AudioManager exists. Yet one was requested for.");

			return audioManager;
		}

		[Range (0, 1)]
		[SerializeField] private float musicVolume = 0.5f;
		public static float MusicVolume { get { return GetAudioManager().musicVolume; } }

		[Range (0, 1)]
		[SerializeField] private float soundEffectVolume = 1;
		public static float SoundEffectVolume { get { return GetAudioManager().soundEffectVolume; } }

		private AudioSource audioSource;
		private SongScriptable currentSong;

		void Awake() {
			if (audioManager != null) throw new Exception("Only one AudioManager may exist at a time.");
			audioManager = this;

			audioSource = this.GetComponentWithError<AudioSource>();
			audioSource.volume = MusicVolume;
		}

		public static AudioManager Main() => audioManager;
		
		// ---
		// Music

		public static void ChangeSong(SongsEnum songID) {
			ChangeSong(AudioContent.GetSong(songID));
		}

		public static void ChangeSong(SongScriptable song) {
			// Stop if were already playing that song
			if (song == GetAudioManager().currentSong) return;

			GetAudioManager().currentSong = song;

			GetAudioManager().StopAllCoroutines();

			GetAudioManager().audioSource.Stop();

			if (song == null) return;

			GetAudioManager().audioSource.clip = song.Clip;
			GetAudioManager().audioSource.Play();

			GetAudioManager().audioSource.volume = MusicVolume;

			GetAudioManager().StartCoroutine(LoopSongCoroutine(song));
		}

		private static IEnumerator LoopSongCoroutine(SongScriptable song) {
			while (true) {
				if ((song.LoopTime.End > 0 && GetAudioManager().audioSource.time > song.LoopTime.End) 
				|| (GetAudioManager().audioSource.time > GetAudioManager().audioSource.clip.length - 0.025f)) {
					GetAudioManager().audioSource.time = song.LoopTime.Start;
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