using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional 
{
	[CreateAssetMenu(menuName = "SO/Sprites")]
	public class SpritesObject : ScriptableObject 
	{
		public SpritesID id;
		public List<Sprite> sprites;
	}
}
