using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional 
{
	[CreateAssetMenu(menuName = "ScriptableObjects/Sprites")]
	public class SpritesObject : ScriptableObject 
	{
		public SpritesID id;
		public List<Sprite> sprites;
	}
}
