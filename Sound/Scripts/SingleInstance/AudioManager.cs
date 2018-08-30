using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Sound {
	
	[RequireComponent (typeof(AudioSource))]
	public class AudioManager : MonoBehaviour {

		private static AudioManager audioPlayer;

		[Range (0, 1)]
		[SerializeField] private float musicVolume = 0.5f;
		public static float MusicVolume { get { return audioPlayer.musicVolume; } }

		[Range (0, 1)]
		[SerializeField] private float soundEffectVolume = 1;
		public static float SoundEffectVolume { get { return audioPlayer.soundEffectVolume; } }

		private AudioSource audioSource;
		private SongScriptable currentSong;

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

		public static void ChangeSong(SongScriptable song) {
			// Stop if were already playing that song
			if (song == audioPlayer.currentSong) return;

			audioPlayer.currentSong = song;

			audioPlayer.StopAllCoroutines();

			audioPlayer.audioSource.Stop();

			if (song == null) return;

			audioPlayer.audioSource.clip = song.Clip;
			audioPlayer.audioSource.Play();

			audioPlayer.audioSource.volume = MusicVolume;

			audioPlayer.StartCoroutine(LoopSongCoroutine(song));
		}

		private static IEnumerator LoopSongCoroutine(SongScriptable song) {
			while (true) {
				if ((song.LoopTime.End > 0 && audioPlayer.audioSource.time > song.LoopTime.End) 
				|| (audioPlayer.audioSource.time > audioPlayer.audioSource.clip.length - 0.025f)) {
					audioPlayer.audioSource.time = song.LoopTime.Start;
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