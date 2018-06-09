using System.Collections.Generic;
using UnityEngine;

namespace Zeef.GameManager 
{
	[CreateAssetMenu(menuName ="ScriptableObjectContainers/Entity")]
	public class EntityObjects : ScriptableObject 
	{
		public List<EntityObject> entities;
	}
}
