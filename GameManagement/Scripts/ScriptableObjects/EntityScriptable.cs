﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.GameManagement 
{
	[CreateAssetMenu(menuName ="SO/Entity")]
	public class EntityScriptable : ScriptableObject 
	{
		public int ID;
		public GameObject Prefab;
	}
}
