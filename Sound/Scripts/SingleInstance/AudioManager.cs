using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Zeef.Sound
{	
	[RequireComponent (typeof(AudioSource))]
	public class AudioManager : SingleInstance<AudioManager>
    {
		[Range (0, 1)]
		[SerializeField] private float musicVolume = 0.5f;
		public static float MusicVolume 
		{ 
			get => GetInstance().musicVolume; 
			set 
			{
				GetInstance().musicVolume = value; 
				GetInstance().audioSource.volume = value * GetInstance().currentSong.Volume;
			} 
		}

		[Range (0, 1)]
		[SerializeField] private float soundEffectVolume = 1;
		public static float SoundEffectVolume 
		{ 
			get => GetInstance().soundEffectVolume; 
			set => GetInstance().soundEffectVolume = value; 
		}

		private AudioSource audioSource;
		private SongScriptable currentSong;

		private Coroutine volumeSlideCoroutineInstance;

		protected override void Awake()
        {
			base.Awake();

			audioSource = this.GetComponentWithError<AudioSource>();
			audioSource.volume = MusicVolume;
		}
		
		// ---
		// Music

		public static void ChangeSong(string songName)
        {
			ChangeSong(AudioContent.GetSong(songName));
		}

		public static void ChangeSong(SongScriptable song)
        {
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

		private static IEnumerator LoopSongCoroutine(SongScriptable song)
        {
			while (true)
            {
				if ((song.LoopTime.End > 0 && GetInstance().audioSource.time > song.LoopTime.End) 
				    || (GetInstance().audioSource.time > GetInstance().audioSource.clip.length - 0.025f))
                {
					GetInstance().audioSource.time = song.LoopTime.Start;
				}
				
				yield return null;
			}
		}

		/// <summary>
		/// Changes the music audio source's pitch. Providing a time will slide from the current pitch to the provided pitch (0 being never, 1 being instant).
		/// </summary>
		public static void ChangeMusicPitch(float pitch, float time = 1)
        {
			if (time == 0 || time == 1)
				GetInstance().audioSource.pitch = pitch;
			else
				GetInstance().StartCoroutine(SlideMusicPitchCoroutine(pitch, time));
		}

		private static IEnumerator SlideMusicPitchCoroutine(float pitch, float time)
        {
			if (time <= 0) yield break;

			while(true)
            {
				GetInstance().audioSource.pitch =  Mathf.Lerp(GetInstance().audioSource.pitch, pitch, time);
				if (Mathf.Abs(GetInstance().audioSource.pitch - pitch) < 0.1f)
				{
					GetInstance().audioSource.pitch = pitch;
					break;
				}
				
				yield return new WaitForUpdate();
			}
		}

		/// <summary>
		/// Changes the music audio source's volume. Providing a time will slide from the current volume to the provided volume (0 being never, 1 being instant).
		/// </summary>
		public static void ChangeMusicVolume(float volume, float time = 0)
		{
			// Stop current coroutine if exists.
			if (GetInstance().volumeSlideCoroutineInstance != null)
				GetInstance().StopCoroutine(GetInstance().volumeSlideCoroutineInstance);

			if (time == 0 || time == 1)
			{
				MusicVolume = volume;
			}
			else
			{
				GetInstance().volumeSlideCoroutineInstance = 
					GetInstance().StartCoroutine(SlideMusicVolumeCoroutine(volume, time));
			}
		}

		private static IEnumerator SlideMusicVolumeCoroutine(float volume, float time)
        {
			if (time <= 0) 
				yield break;

			while(true)
            {
				MusicVolume = Mathf.Lerp(
					a: MusicVolume, 
					b: volume, 
					t: time
				);

				if (Mathf.Abs(MusicVolume - volume) < 0.05f)
				{
					MusicVolume = volume;
					break;
				}
				
				yield return new WaitForUpdate();
			}
		}

		// ---
		// SFX

		/// <summary>
		/// Play SoundEffectScriptable found in AudioContent with provided name.
		/// Will create a short lived GameObject with an AudioSource if no AudioSource is provided.
		/// </summary>
		public static void PlaySoundEffect(string sfxName, AudioSource audioSource = null)
		{
			PlaySoundEffect(AudioContent.GetSoundEffect(sfxName));
		}

		/// <summary>
		/// Play clip from SoundEffectScriptable. 
		/// Will create a short lived GameObject with an AudioSource if no AudioSource is provided.
		/// </summary>
		public static void PlaySoundEffect(SoundEffectScriptable sfx, AudioSource audioSource = null)
		{
			GetInstance().StartCoroutine(PlaySoundEffectCoroutine(sfx, audioSource));
		}

		/// <summary>
		/// Play SoundEffectScriptable found in AudioContent with provided name. 
		/// Will create a short lived GameObject with an AudioSource if no AudioSource is provided.
		/// Task returns once the clip has finished playing.
		/// </summary>
		public static async Task PlaySoundEffectAsync(string sfxName, AudioSource audioSource = null)
        {
            await PlaySoundEffectAsync(AudioContent.GetSoundEffect(sfxName));
        }

		/// <summary>
		/// Play clip from SoundEffectScriptable. 
		/// Will create a short lived GameObject with an AudioSource if no AudioSource is provided.
		/// Task returns once the clip has finished playing.
		/// </summary>
		public static async Task PlaySoundEffectAsync(SoundEffectScriptable sfx, AudioSource audioSource = null)
        {
			await PlaySoundEffectCoroutine(sfx, audioSource);
		}

		private static IEnumerator PlaySoundEffectCoroutine(SoundEffectScriptable sfx, AudioSource audioSource = null) 
		{
			bool wasAudioSourceProvided = audioSource != null;

			// AudioSource wasn't provided, let's create one.
			if (!wasAudioSourceProvided)
			{
				audioSource = new GameObject().AddComponent<AudioSource>();
				audioSource.transform.SetParent(GetInstance().transform);
			}

			audioSource.pitch = sfx.Pitch;

			// Play clip.
			audioSource.PlayOneShot(sfx.Clip, sfx.Volume * SoundEffectVolume);

			// Wait for length of clip.
			yield return new WaitForSeconds(sfx.Clip.length);

			// If we had to create a GameObject, we need to destroy it.
			if (!wasAudioSourceProvided)
				Destroy(audioSource.gameObject);
			else
				audioSource.pitch = 1;
		}
	}
}