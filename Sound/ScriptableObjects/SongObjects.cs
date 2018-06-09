using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Sound 
{
	[CreateAssetMenu(menuName = "ScriptableObjectContainers/Song")]
	public class SongObjects : ScriptableObject 
	{
		public List<SongObject> songs;
	}
}