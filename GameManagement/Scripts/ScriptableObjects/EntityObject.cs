using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.GameManagement 
{
	[CreateAssetMenu(menuName ="SO/Entity")]
	public class EntityObject : ScriptableObject 
	{
		public EntitiesEnum id;
		public GameObject prefab;
	}
}
