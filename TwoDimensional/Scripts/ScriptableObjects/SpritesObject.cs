using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional 
{
	[CreateAssetMenu(menuName = "SO/Sprites")]
	public class SpritesObject : ScriptableObject 
	{
		public int ID;
		public List<Sprite> Sprites;
	}
}
