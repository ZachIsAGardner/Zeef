using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Sound {
	
	[RequireComponent (typeof(AudioSource))]
	public class AudioManager : MonoBehaviour {

		private static AudioManager audioPlayer;

		[SerializeField] private float musicVolume = 0.5f;
		public static float MusicVolume { get { return audioPlayer.musicVolume; } }

		[SerializeField] private float soundEffectVolume = 1;
		public static float SoundEffectVolume { get { return audioPlayer.soundEffectVolume; } }

		private AudioSource audioSource;
		private SongObject currentSong;

		void Awake() {
			if (audioPlayer != null) throw new Exception("Only one audio player may exist at a time.");
			audioPlayer = this;

			audioSource = this.GetComponentWithError<AudioSource>();
			audioSource.volume = MusicVolume;
		}

		public static AudioManager Main() => audioPlayer;
		
		// ---
		// Music

		public static void ChangeSong(SongsEnum songID) {
			ChangeSong(AudioContent.GetSong(songID));
		}

		public static void ChangeSong(SongObject song) {
			// stop if were already playing that song
			if (song == audioPlayer.currentSong) return;

			audioPlayer.currentSong = song;

			audioPlayer.StopAllCoroutines();

			audioPlayer.audioSource.time = 0;
			audioPlayer.audioSource.Stop();

			if (song == null) return;

			audioPlayer.audioSource.clip = song.clip;
			audioPlayer.audioSource.Play();

			audioPlayer.audioSource.volume = MusicVolume;

			audioPlayer.StartCoroutine(LoopSongCoroutine(song));
		}

		private static IEnumerator LoopSongCoroutine(SongObject song) {
			while (true) {
				if (song.loopTimes.Max == 0) {
					// Debug.LogWarning($"loop end for '{song.name}' has not been set.");
					break;
				}
				if (audioPlayer.audioSource.time > song.loopTimes.Max) {
					audioPlayer.audioSource.time = song.loopTimes.Min;
				}
				yield return null;
			}
		}

		// ---
		// SFX

		public static void PlaySoundEffect(AudioSource source, SoundEffectObject obj) {
			if (obj == null) return;
			source.PlayOneShot(obj.clip, SoundEffectVolume - (1 - obj.volume));
		}

		public static void PlaySoundEffect(AudioSource source, SoundEffectsEnum id) {
			PlaySoundEffect(source, AudioContent.GetSoundEffect(id));
		}

		public static void PlaySoundEffect(AudioSource source, string name) {
			PlaySoundEffect(source, AudioContent.GetSoundEffect(name));
		}
	}
}