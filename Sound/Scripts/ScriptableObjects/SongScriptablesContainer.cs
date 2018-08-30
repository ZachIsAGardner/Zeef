using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Sound {

	[CreateAssetMenu(menuName = "SOs Container/Song")]
	public class SongScriptablesContainer : ScriptableObject {
		public List<SongScriptable> Songs;
	}
}