using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.POISystem {
	public class POITestScript : MonoBehaviour
	{
		[Serializable]
		class POIs {
			public Vector3 position;
			public GameObject poi;
		}

		[SerializeField] List<POIs> pois;
		[SerializeField] List<GameObject> spawnedPois;

		void Start() {
			SpawnAllPOIs();
			StartCoroutine(ReSpawnPOIs());
		}

		void SpawnAllPOIs() {
			foreach (var poi in pois) {
				spawnedPois.Add(Instantiate(poi.poi, poi.position, Quaternion.identity));
			}
		}

		void DestroyAllPOIs() {
			foreach (var poi in spawnedPois) {
				Destroy(poi);
			}
		}

		IEnumerator ReSpawnPOIs() {
			yield return new WaitForSeconds(6f);
			DestroyAllPOIs();
			yield return new WaitForSeconds(0.5f);
			SpawnAllPOIs();
		}
	}
}
