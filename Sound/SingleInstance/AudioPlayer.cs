using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Sound {
	
	[RequireComponent(typeof (SingleInstanceChild))]
	public class AudioPlayer : MonoBehaviour {

		private AudioReference db;
		private AudioSource audioSource;

		private SongObject currentSong;

		void Start() {
			db = AudioReference.Main();
			audioSource = GetComponent<AudioSource>();

			audioSource.volume = db.MusicVolume();
		}

		public static AudioPlayer Main(){
			return SingleInstance.Main().GetComponentInChildren<AudioPlayer>();
		}

		#region Music 

		public void ChangeSong(SongObject song) {
			// stop if were already playing that song
			if (song == currentSong) return;

			currentSong = song;

			StopAllCoroutines();

			audioSource.time = 0;
			audioSource.Stop();

			if (song == null) return;

			audioSource.clip = song.clip;
			audioSource.Play();

			audioSource.volume = db.MusicVolume();

			StartCoroutine(LoopSong(song));
		}

		private IEnumerator LoopSong(SongObject song) {
			while (true) {
				if (song.loopTimes.max == 0) {
					// Debug.LogWarning($"loop end for '{song.name}' has not been set.");
					break;
				}
				if (audioSource.time > song.loopTimes.max) {
					audioSource.time = song.loopTimes.min;
				}
				yield return null;
			}
		}

		#endregion

		#region SFX

		public void PlaySoundEffect(AudioSource source, SoundEffectObject obj) {
			if (obj == null) return;
			source.PlayOneShot(obj.clip, AudioReference.Main().SoundEffectVolume() - (1 - obj.volume));
		}

		public void PlaySoundEffect(AudioSource source, SoundEffectID id) {
			PlaySoundEffect(source, AudioReference.Main().GetSoundEffect(id));
		}

		public void PlaySoundEffect(AudioSource source, string name) {
			PlaySoundEffect(source, AudioReference.Main().GetSoundEffect(name));
		}

		#endregion
	}
}