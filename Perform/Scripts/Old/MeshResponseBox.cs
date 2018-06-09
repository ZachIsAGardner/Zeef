// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using ZeePerform.Data;

// public class MeshResponseBox : ResponseBox 
// {
//     public TextMesh textMesh;
//     SpriteRenderer border;

//     public override void Initialize(List<Path> paths) 
//     {
//         border = GetComponent<SpriteRenderer>();

//         transform.parent = Camera.main.transform;
// 		transform.localPosition = Vector3.zero;

//         base.Initialize(paths);
//     }

//     protected override void FillText(List<string> lines) 
//     {
// 		foreach (var line in lines)
// 		{
// 			textMesh.text += line;
// 			textMesh.text += "\n";
// 		}
// 	}

//     protected override void AdjustResponseBox(int lineCount) {
// 		border.size = new Vector2(
// 			border.size.x,
// 			lineCount * 16
// 		);
// 	}
// }