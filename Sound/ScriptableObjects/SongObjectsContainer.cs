using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Sound 
{
	[CreateAssetMenu(menuName = "SOs Container/Song")]
	public class SongObjectsContainer : ScriptableObject 
	{
		public List<SongObject> songs;
	}
}