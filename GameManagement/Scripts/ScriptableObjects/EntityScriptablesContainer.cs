using System.Collections.Generic;
using UnityEngine;

namespace Zeef.GameManagement 
{
	[CreateAssetMenu(menuName ="SOs Container/Entity")]
	public class EntityScriptablesContainer : ScriptableObject 
	{
		public List<EntityScriptable> Entities;
	}
}
