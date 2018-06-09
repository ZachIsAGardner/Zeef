// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using UnityEngine.UI;
// using ZeePerform.Data;

// public class CanvasTextBox : TextBox 
// {
//     public Text canvasText;
// 	public Text canvasSpeaker;

// 	protected override void AdditionalSetup() {
// 		Canvas canvas = FindObjectsOfType<Canvas>().First(c => c.tag == "OverworldCanvas");
// 		transform.SetParent(canvas.transform, false);
// 		GetComponent<RectTransform>().anchoredPosition = dialogueDB.textBoxCameraPosition;
// 	}

// 	protected override void ClearText() {
// 		canvasText.text = "";
// 	}

// 	protected override void AddText(string str) {
// 		canvasText.text += str;
// 	}

//     protected override void FillSpeaker(Section section) {
//         if (canvasSpeaker != null) {
// 			canvasSpeaker.text = (section.speaker != null) ? section.speaker : "";
// 		}
//     }

//     protected override int GetMaxLength()
//     {
//         return (forceLineLength <= 0) ? 500 : forceLineLength;
//     }
// }