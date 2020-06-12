using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
			get => Instance.musicVolume; 
			set 
			{
				Instance.musicVolume = value; 
				Instance.audioSource.volume = value * Instance.currentSong.Volume;
			} 
		}

		[Range (0, 1)]
		[SerializeField] private float soundEffectVolume = 1;
		public static float SoundEffectVolume 
		{ 
			get => Instance.soundEffectVolume; 
			set => Instance.soundEffectVolume = value; 
		}

		private AudioSource audioSource;
		private SongScriptable currentSong;

		private Coroutine volumeSlideCoroutineInstance;

		private Dictionary<string, string> playedSoundEffects = new Dictionary<string, string>();

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
			if (song == Instance.currentSong) return;

			Instance.audioSource.pitch = 1;

			Instance.currentSong = song;

			Instance.StopAllCoroutines();

			Instance.audioSource.Stop();

			if (song == null) return;

			Instance.audioSource.clip = song.Clip;
			Instance.audioSource.Play();

			Instance.audioSource.volume = MusicVolume * song.Volume;

			Instance.StartCoroutine(LoopSongCoroutine(song));
		}

		private static IEnumerator LoopSongCoroutine(SongScriptable song)
        {
			while (true)
            {
				if ((song.LoopTime.End > 0 && Instance.audioSource.time > song.LoopTime.End) 
				    || (Instance.audioSource.time > Instance.audioSource.clip.length - 0.025f))
                {
					Instance.audioSource.time = song.LoopTime.Start;
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
				Instance.audioSource.pitch = pitch;
			else
				Instance.StartCoroutine(SlideMusicPitchCoroutine(pitch, time));
		}

		private static IEnumerator SlideMusicPitchCoroutine(float pitch, float time)
        {
			if (time <= 0) yield break;

			while(true)
            {
				Instance.audioSource.pitch =  Mathf.Lerp(Instance.audioSource.pitch, pitch, time);
				if (Mathf.Abs(Instance.audioSource.pitch - pitch) < 0.1f)
				{
					Instance.audioSource.pitch = pitch;
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
			if (Instance.volumeSlideCoroutineInstance != null)
				Instance.StopCoroutine(Instance.volumeSlideCoroutineInstance);

			if (time == 0 || time == 1)
			{
				MusicVolume = volume;
			}
			else
			{
				Instance.volumeSlideCoroutineInstance = 
					Instance.StartCoroutine(SlideMusicVolumeCoroutine(volume, time));
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
			Instance.StartCoroutine(PlaySoundEffectCoroutine(sfx, audioSource));
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
				audioSource.transform.SetParent(Instance.transform);
			}

			audioSource.pitch = sfx.Pitch;

			// Play clip.
			AudioClip clip = null;

			// Multiple variants
			if (sfx.Clips.Count > 1) 
			{
				// If there are multiple variants, we don't want to play the same one twice in a row.
				string lastPlayedVariant = "";
				Instance.playedSoundEffects.TryGetValue(sfx.name, out lastPlayedVariant);
				if (!String.IsNullOrWhiteSpace(lastPlayedVariant))
				{
					clip = sfx.Clips.Where(c => c.name != lastPlayedVariant).ToList().Random();
					Instance.playedSoundEffects.Remove(sfx.name);
				}
				else 
				{
					clip = sfx.Clips.Random();
				}

				Instance.playedSoundEffects.Add(sfx.name, clip.name);
			}
			// Just one
			else if (sfx.Clips.Count == 1)
			{
				clip = sfx.Clips[0];
			}
			else
			{
				yield break;
			}

			audioSource.PlayOneShot(clip, sfx.Volume * SoundEffectVolume);

			// Wait for length of clip.
			yield return new WaitForSeconds(clip.length);

			// If we had to create a GameObject, we need to destroy it.
			if (!wasAudioSourceProvided)
				Destroy(audioSource.gameObject);
			else
				audioSource.pitch = 1;
		}
	}
}