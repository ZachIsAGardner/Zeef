using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeef;
using Zeef.GameManagement;

public class TopDownExample : MonoBehaviour {

	[SerializeField] GameObject bearPrefab;
	[SerializeField] Transform bearSpawn;

	void Start () {
		GameManager.SpawnActor(bearPrefab, bearSpawn.position);
	}
}
