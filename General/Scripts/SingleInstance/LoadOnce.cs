using UnityEngine;

namespace Zeef 
{
	public class LoadOnce : MonoBehaviour 
	{	
		[Required]
		[SerializeField] private GameObject prefab;

		private static bool loaded;

		public void Awake() 
		{	
			if (!loaded) 
			{
				Instantiate(prefab);
				loaded = true;
			}
		}
	}
}
