using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional 
{
	[CreateAssetMenu(menuName = "SO/Sprites")]
	public class SpritesObject : ScriptableObject 
	{
		public SpritesEnum id;
		public List<Sprite> sprites;
	}
}
