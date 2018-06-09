using System.Collections.Generic;
using UnityEngine;

namespace Zeef.GameManager 
{
	[CreateAssetMenu(menuName ="SOs Container/Entity")]
	public class EntityObjectsContainer : ScriptableObject 
	{
		public List<EntityObject> entities;
	}
}
