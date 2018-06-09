// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using UnityEngine.UI;
// using ZeePerform.Data;

// public class CanvasResponseBox : ResponseBox 
// {
//     public Text canvasText;
//     Canvas canvas;

//     public override void Initialize(List<Path> paths) {
//         canvas = FindObjectsOfType<Canvas>().First(c => c.tag == "OverworldCanvas");
//         transform.SetParent(canvas.transform);

//         base.Initialize(paths);
//     }

//     protected override void FillText(List<string> lines) 
//     {
//         canvasText.text = "";

// 		foreach (var line in lines)
// 		{
// 			canvasText.text += line;
// 			canvasText.text += "\n";
// 		}
// 	}
// }