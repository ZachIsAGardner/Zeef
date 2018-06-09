using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional {

	public class HitBoxHandler : MonoBehaviour {

		// public Object prefHitBox;
		// [HideInInspector]
		// public List<HitBox> hitBoxes = new List<HitBox>();

		// ZeeTimerHandler timerHandler;

		// void Awake() {
		// 	timerHandler = GetComponent<ZeeTimerHandler>();
		// }

		// public HitBox CreateHitBox(Vector3 pos, Vector2 size = default(Vector2), float lifetime = 0.25f, bool parented = false, float damage = 25) {
		// 	if (size == Vector2.zero) {
		// 		size = new Vector2(64, 64);
		// 	}
		// 	HitBox hitBox = Instantiate(prefHitBox) as HitBox;

		// 	hitBox.owner = this.gameObject;
		// 	hitBox.transform.position = pos;
		// 	hitBox.damage = damage;
		// 	hitBox.GetComponent<BoxCollider2D>().size = size;
		// 	hitBox.parented = parented;
		// 	hitBox.transform.parent = GameObject.Find("MAIN").transform;
		// 	hitBoxes.Add(hitBox);
		
		// 	timerHandler.AddTimer(() => {
		// 		hitBoxes.RemoveAt(hitBoxes.Count - 1);
		// 		Destroy(hitBox.gameObject);
		// 	}, lifetime);
		

		// 	return hitBox;
		// }
	}

}
