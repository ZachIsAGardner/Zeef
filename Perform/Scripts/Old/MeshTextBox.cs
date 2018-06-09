// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using ZeePerform.Data;

// public class MeshTextBox : TextBox
// {
//     public TextMesh textMesh;
//     public TextMesh speakerMesh;
//     protected SpriteRenderer border;

// 	protected override void AdditionalSetup() {
//         border = GetComponentInChildren<SpriteRenderer>();
// 	}

//     protected override void ClearText() {
// 		textMesh.text = "";
// 	}

// 	protected override void AddText(string str) {
// 		textMesh.text += str;
// 	}

//     protected override void FillSpeaker(Section section) {
//         if (speakerMesh != null) {
// 			speakerMesh.text = (section.speaker != null) ? section.speaker : "";
// 		}
//     }

//     protected override int GetMaxLength()
//     {
//         return (forceLineLength <= 0) ? (int)(border.size.x / 5) : forceLineLength;
//     }
// }