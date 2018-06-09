using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.GameManager 
{
	[CreateAssetMenu(menuName ="ScriptableObjects/Entity")]
	public class EntityObject : ScriptableObject 
	{
		public EntityID id;
		public GameObject prefab;
	}
}
