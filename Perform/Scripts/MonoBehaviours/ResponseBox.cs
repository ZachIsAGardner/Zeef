using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
// ---
using Zeef.Menu;
using ZeeUtil;

namespace Zeef.Perform {
	public class ResponseBox : UIElement 
	{
		public Text canvasText;

		public int Choice {get; protected set;}
		List<string> choices; 

		public static ResponseBox Initialize(GameObject go, List<Path> paths) {
			ResponseBox result = go.GetComponent<ResponseBox>();

			result.PopulateResponses(paths);
			return result;
		}

		public void Die() {
			Destroy(gameObject);
		}

		void PopulateResponses(List<Path> paths) {
			choices = SplitPathsToLines(paths);

			FillText();

			HighlightResponse(0);
		}

		void FillText() {
			canvasText.text = "";

			foreach (var choice in choices)
			{
				canvasText.text += choice;
				canvasText.text += "\n";
			}
		}

		#region Utility

		public void DecrementChoice() {
			Choice--;
			if (Choice < 0) {
				Choice = 0;
			} else {
				HighlightResponse(Choice);
			}
		}

		public void IncrementChoice() {
			Choice ++;
			if (Choice >= choices.Count) {
				Choice = choices.Count - 1;
			} else {
				HighlightResponse(Choice);
			}
		}

		public void HighlightResponse(int choice) {

			for (int i = 0; i < choices.Count; i++)
			{
				if (choice % choices.Count == i) {
					choices[i] = ">" + choices[i] + "<";		
				} else {
					choices[i] = choices[i].Trim(new Char[]{'<', '>'});
				}
			}
			FillText();
		}

		// Takes a list of paths/responses and splits them to lines
		List<string> SplitPathsToLines(List<Path> paths) {
			List<string> lines = new List<String>();

			foreach (Path path in paths)
			{
				lines.Add(path.name);
			}

			return lines;
		}

		#endregion
	}
}