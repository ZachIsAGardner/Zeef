using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
 
namespace Zeef.GameManager {

	[ExecuteInEditMode]
	public class Spawn : MonoBehaviour {
		public int ID { get; private set; }

		void Update() 
		{
			GetID();
		}

		void GetID() {
			// weird
			// get parent to get siblings
			List<Spawn> spawns = gameObject.transform.parent
				.GetComponentsInChildren<Spawn>().ToList();

			int i = 0;
			spawns.ForEach(s => {
				if (s == this) {
					ID = i;
					name = $"Spawn {i}";
					return;
				}
				i++;
			});
		}
	}

}
