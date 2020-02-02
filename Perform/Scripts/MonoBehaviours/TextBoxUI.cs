using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zeef.GameManagement;
// ---
using Zeef.Sound;

namespace Zeef.Perform
{
	[RequireComponent (typeof (AudioSource))]
	public class TextBoxUI : MonoBehaviour
    {
		[SerializeField] private Text textComponent;
		[SerializeField] private Text speakerComponent;
		// TODO: Figure out how to get line length from 
		// text component, rather than just guess and checking
		[SerializeField] private int forceLineLength = -1;

		private string text; // Text to be displayed
		private string speaker; // Speaker to be displayed

		private bool skipToEnd;
		
		private int maxLineLength; // Max character length for a line of text

		private bool auto; // Text box closes automatically once it has finished crawling the text
		private float crawlTime; // Wait time between letters
		private SoundEffectScriptable tone; // The noise made when the text is crawling
		private bool closeWhenDone;
		
		void Update()
        {
			if (ControlManager.GetInputPressed(ControlManager.Accept))
                skipToEnd = true;	
		}

		/// <summary>
		/// Create a new TextBox.
		/// <param name="prefab">The prefab to make a copy of.</param>
		/// <param name="position">The position in which to spawn the new Text Box.</param>
		/// <param name="model">The model in which to generate the data from.</param>
		/// </summary>
		public static TextBoxUI Initialize(
            TextBoxUI prefab,
            Vector2 position,
            TextBoxUIFullModel model
        )
        {
			TextBoxUI instance = GameManager.SpawnCanvasElement(prefab.gameObject).GetComponentWithError<TextBoxUI>();

			instance.text = model.Text;
			instance.speaker = model.Speaker;

			instance.auto = model.Auto == true;
			instance.crawlTime = model.CrawlTime ?? 0;
			instance.tone = model.Tone;
			instance.closeWhenDone = model.CloseWhenDone == true;

			return instance;
		}

		/// <summary>
		/// Provide new model and execute.
		/// </summary>
		public async Task ExecuteAsync(TextBoxUIFullModel model)
        {
			text = model.Text;
			speaker = model.Speaker;

			auto = model.Auto == true;
			crawlTime = model.CrawlTime ?? 0;
			tone = model.Tone;
			closeWhenDone = model.CloseWhenDone == true;
	
			await ExecuteAsync();
		}

		/// <summary>
		/// Execute with existing data.
		/// </summary>
		public async Task ExecuteAsync()
        {
			skipToEnd = false;

			maxLineLength = (forceLineLength > 0) ? forceLineLength : 500;

			if (speakerComponent != null) 
				speakerComponent.text = speaker ?? "";

			if (crawlTime > 0) 
				await DisplayTextAsync(SplitTextToLines(text, maxLineLength));
			else 
				textComponent.text = text;	
		}

		/// <summary>
		/// Close this Text Box.
		/// </summary>
		public void Close()
        {
			Destroy(gameObject);
		}

		// ---

		private async Task DisplayTextAsync(List<string> lines)
        {
			textComponent.text = "";
		
			foreach (string line in lines)
            {
				if (skipToEnd) break;

				foreach (char letter in line.ToCharArray())
                {
					if (skipToEnd) break;
					
					textComponent.text += letter;

					if (tone != null) AudioManager.PlaySoundEffect(tone);
					await WaitAsync(letter);
				}

				textComponent.text += "\n";
			}

			textComponent.text = text;

			if (auto) 
				await new WaitForUpdate();
			else 
				await ControlManager.WaitForInputPressedAsync(ControlManager.Accept);
			
			if (closeWhenDone)
				Close();
		}

		// Wait an amount of time based on character
		private IEnumerator WaitAsync(char letter)
        {
			switch (letter)
            {
				case '.':
				case '?':
				case '!':
				case ':':
					yield return new WaitForSeconds(crawlTime * 5.5f);
					break;
				case ',':
				case ';':
					yield return new WaitForSeconds(crawlTime * 3f);
					break;
				case ' ':
					break;
				default:
					yield return new WaitForSeconds(crawlTime);
					break;
			}
		}

		// ---

		// takes a single line of text and splits it apporiately
		private List<string> SplitTextToLines(string text, int lineLength)
        {
			List<string> words = text.Split(' ').ToList();
			List<string> lines = new List<string>(){""};

			foreach (string word in words)
            {
				
				// Line length if we were to add the current word
				int affectedLineLength = (lines.Last() == "") 
					? word.Length 
					: (lines.Last() + " " + word).Length;

				// If the possible line length is longer than our max length add a new line
				if (affectedLineLength >= lineLength) lines.Add("");
				
				// Add a space if this line has content already
				lines[lines.Count - 1] += (lines[lines.Count - 1] == "") ? word : " " + word;
			}

			return lines;
		}
	}
}