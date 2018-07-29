using System.Collections.Generic;
using UnityEngine;

namespace Zeef.GameManagement 
{
	[CreateAssetMenu(menuName ="SOs Container/Entity")]
	public class EntityObjectsContainer : ScriptableObject 
	{
		public List<EntityObject> entities;
	}
}
