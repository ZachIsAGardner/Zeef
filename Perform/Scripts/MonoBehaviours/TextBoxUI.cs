using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
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
		[SerializeField] private TextMeshProUGUI textProComponent;

		private string displayedText = "";

		[SerializeField] private Text speakerComponent;
		[SerializeField] private TextMeshProUGUI speakerProComponent;

		// TODO: Figure out how to get line length from 
		// text component, rather than just guess and checking
		[SerializeField] private int forceLineLength = -1;

		/// <summary>
		/// Whether or not the text box is currently crawling text.
		/// </summary>
		public bool IsActive { get; private set; }

		public bool SkipToEnd;
		private int maxLineLength; // Max character length for a line of text
		private int toneInterval;

		[TextArea(3,100)]
		public string Text; // Text to be displayed
		public string Speaker; // Speaker to be displayed

		public int ToneIntervalMax = 3;

		public bool Auto; // Text box closes automatically once it has finished crawling the text
		public float CrawlTime; // Wait time between letters
		public SoundEffectScriptable Tone; // The noise made when the text is crawling
		public bool CloseWhenDone;
		public List<string> ProceedInputs = new List<string>() { "z" };

		protected virtual void TextFinishedCrawling() { }
		
		void Update()
        {
			if (ControlManager.GetInputPressed(ProceedInputs))
                SkipToEnd = true;	

			if (textProComponent)
				textProComponent.text = displayedText;
			else if (textComponent)
				textProComponent.text = displayedText;
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
			TextBoxUI instance = SpawnManager.SpawnCanvasElement(prefab.gameObject, 4).GetComponentWithError<TextBoxUI>();

			if (model.Text != null)
				instance.Text = model.Text;

			if (model.Speaker != null)
				instance.Speaker = model.Speaker ?? instance.Speaker;

			if (model.Auto != null)
				instance.Auto = model.Auto == true;
			
			if (model.CrawlTime != null)
				instance.CrawlTime = model.CrawlTime ?? instance.CrawlTime;

			if (model.Tone != null)
				instance.Tone = model.Tone;

			if (model.CloseWhenDone != null)
				instance.CloseWhenDone = model.CloseWhenDone == true;

			if (model.ToneIntervalMax != null)
				instance.ToneIntervalMax = model.ToneIntervalMax.Value;

			if (model.ProceedInputs != null)
				instance.ProceedInputs = model.ProceedInputs;

			return instance;
		}

		/// <summary>
		/// Provide new model and execute.
		/// </summary>
		public async Task ExecuteAsync(TextBoxUIFullModel model)
        {
			if (model.Text != null)
				Text = model.Text;

			if (model.Speaker != null)
				Speaker = model.Speaker;

			if (model.Auto != null)
				Auto = model.Auto == true;
			
			if (model.CrawlTime != null)
				CrawlTime = model.CrawlTime.Value;

			if (model.Tone)
				Tone = model.Tone;

			if (model.CloseWhenDone != null)
				CloseWhenDone = model.CloseWhenDone == true;

			if (model.ToneIntervalMax != null)
				ToneIntervalMax = model.ToneIntervalMax.Value;

			if (model.ProceedInputs != null)
				ProceedInputs = model.ProceedInputs;
	
			await ExecuteAsync();
		}

		/// <summary>
		/// Execute with existing data.
		/// </summary>
		public async Task ExecuteAsync()
        {
			SkipToEnd = false;

			if (speakerProComponent != null) 
				speakerProComponent.text = Speaker ?? "";
			else if (speakerComponent != null)
				speakerComponent.text = Speaker ?? "";

			if (CrawlTime > 0) 
				await DisplayTextAsync(new List<string>() { Text });
			else 
				displayedText = Text;	
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
			IsActive = true;
			displayedText = "";
		
			foreach (string line in lines)
            {
				if (SkipToEnd) break;

				foreach (char letter in line.ToCharArray())
                {
					if (SkipToEnd) 
						break;
					
					displayedText += letter;
					toneInterval--;

					if (Tone != null && toneInterval <= 0) 
					{
						AudioManager.PlaySoundEffect(Tone);
						toneInterval = ToneIntervalMax;
					}

					await WaitAsync(letter);
				}

				displayedText += "\n";
			}

			TextFinishedCrawling();
			IsActive = false;
			displayedText = Text;

			if (Auto) 
				await new WaitForUpdate();
			else 
				await ControlManager.WaitForInputPressedAsync(ProceedInputs);
			
			if (CloseWhenDone)
			{
				Close();
			}
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
					yield return new WaitForSeconds(CrawlTime * 5.5f);
					break;
				case ',':
				case ';':
					yield return new WaitForSeconds(CrawlTime * 3f);
					break;
				case ' ':
					break;
				default:
					yield return new WaitForSeconds(CrawlTime);
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