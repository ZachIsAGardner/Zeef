using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
// ---
using Zeef.Sound;
using ZeeUtil;

namespace Zeef.Perform {
	[RequireComponent (typeof (AudioSource))]
	public class TextBox : MonoBehaviour {

		public Text canvasText;
		public Text canvasSpeaker;

		protected PerformanceReference performanceRef;
		protected AudioPlayer songPlayer;
		protected AudioSource audioSource;

		public bool Crawling { get; protected set; }

		int maxLineLength;
		public int forceLineLength = -1;
		public float WaitTime { get; private set; }
		public SoundEffectID Tone { get; private set; }
		public bool Auto { get; private set; }

		public static TextBox Initialize(GameObject go, Section section, TextBoxOptions options = null) {
			TextBox result = go.GetComponent<TextBox>();
			result.Execute(section, options);
			return result;
		}

		public void Execute(Section section, TextBoxOptions options = null) {
			Setup(section, options);
			StartCoroutine(DigestText(SplitTextToLines(section.text)));
		}

		public void Die() {
			Destroy(gameObject);
		}

		#region Setup

		void Setup(Section section, TextBoxOptions options) {
			GetComponents();
			SetOptions(options);
			FillSpeaker(section);
			maxLineLength = GetMaxLength();
		}

		//get rid of this
		public void SetSpeed(int speed) {
			this.WaitTime = speed;
		}

		protected void GetComponents() {
			performanceRef = PerformanceReference.Main();
			songPlayer = AudioPlayer.Main();

			audioSource = GetComponent<AudioSource>();
		}


		void SetOptions(TextBoxOptions options = null) {
			options = options ?? TextBoxOptions.Default();
			
			WaitTime = options.Speed;
			Tone = options.Tone;
			Auto = options.Auto;
		}

		protected int GetMaxLength()
		{
			return (forceLineLength <= 0) ? 500 : forceLineLength;
		}

		#endregion

		#region Execution

		IEnumerator DigestText(List<string> lines) {
			ClearText();
		
			Crawling = true;


			int j = 0;
			while (j < lines.Count) {
				int i = 0;
				while (i < lines[j].Count())
				{
					char letter = lines[j][i];
					AddText(letter.ToString());


					if (WaitTime > 0) {
						AudioPlayer.Main().PlaySoundEffect(audioSource, Tone);
						yield return StartCoroutine(Crawl(letter));
					}
					
					i++;
				}
				AddText("\n");
				j++;
			}

			Crawling = false;

			yield return null;

			if (string.IsNullOrEmpty(lines.First())) Die();
		}

		void ClearText() {
			canvasText.text = "";
		}

		void AddText(string str) {
			canvasText.text += str;
		}

		void FillSpeaker(Section section) {
			if (canvasSpeaker != null) {
				canvasSpeaker.text = (section.speaker != null) ? section.speaker : "";
			}
		}

		IEnumerator Crawl(char letter) {
			switch (letter)
			{
				case '.':
				case '?':
				case '!':
					yield return new WaitForSeconds(WaitTime * 0.45f);
					break;
				case ',':
				case ';':
				case ':':
					yield return new WaitForSeconds(WaitTime * 0.25f);
					break;
				case ' ':
					break;
				default:
					yield return new WaitForSeconds(WaitTime * 0.05f);
					break;
			}
		}

		// takes a single line of text and splits it apporiately
		List<string> SplitTextToLines(string text) {
			string[] words = text.Split(' ');
			List<string> lines = new List<string>(){""};

			int j = 0;
			int i = 0;
			while (i < words.Length)
			{
				int lineLength = (lines[j] == "") 
					? words[i].Length 
					: (lines[j] + " " + words[i]).Length;

				if (lineLength >= maxLineLength) {
					lines.Add("");
					j++;
				}

				lines[j] += (lines[j] == "") ? words[i] : " " + words[i];
				i ++;
			}

			return lines;
		}

		#endregion
	}
}