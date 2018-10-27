using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

		public static void ChangeSong(string songName) {
			ChangeSong(AudioContent.GetSong(songName));
		}

		public static void ChangeSong(SongScriptable song) {
			// Stop if were already playing that song
			if (song == GetInstance().currentSong) return;

			GetInstance().audioSource.pitch = 1;

			GetInstance().currentSong = song;

			GetInstance().StopAllCoroutines();

			GetInstance().audioSource.Stop();

			if (song == null) return;

			GetInstance().audioSource.clip = song.Clip;
			GetInstance().audioSource.Play();

			GetInstance().audioSource.volume = MusicVolume * song.Volume;

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

		public static void SlidePitch(float pitch, float time) {
			GetInstance().StartCoroutine(SlidePitchCoroutine(pitch, time));
		}

		private static IEnumerator SlidePitchCoroutine(float pitch, float time) {
			if (time <= 0) yield break;

			while(true) {
				GetInstance().audioSource.pitch =  Mathf.Lerp(GetInstance().audioSource.pitch, pitch, time);
				if (Mathf.Abs(GetInstance().audioSource.pitch - pitch) < -0.1f)
					break;
				
				yield return new WaitForUpdate();
			}
		}

		// ---
		// SFX

		public static void PlaySoundEffect(AudioSource source, SoundEffectScriptable sfx) {
			if (sfx == null) return;
			source.PlayOneShot(sfx.Clip, SoundEffectVolume * sfx.Volume);
		}

		public static void PlaySoundEffect(AudioSource source, string sfxName) {
			PlaySoundEffect(source, AudioContent.GetSoundEffect(sfxName));
		}
	}
}