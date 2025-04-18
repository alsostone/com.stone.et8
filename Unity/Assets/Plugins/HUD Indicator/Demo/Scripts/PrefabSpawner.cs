using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour {

    [SerializeField] private GameObject prefab = null;


	private void OnEnable() {
		StartCoroutine(SpawnPrefabs());
	}

	private IEnumerator SpawnPrefabs() {
		while(true) {

			Vector3 pos = transform.position + Random.insideUnitSphere;
			pos.z = transform.position.z;
			GameObject go = Instantiate(prefab, pos, Quaternion.identity);

			Destroy(go, 10f);

			yield return new WaitForSeconds(1f);
		}
	}

}