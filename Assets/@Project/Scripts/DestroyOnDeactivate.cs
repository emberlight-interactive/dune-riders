using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders {
	public class DestroyOnDeactivate : MonoBehaviour
	{
		[SerializeField] bool delayedDestroy = true;
		[SerializeField] float minDelayedDestroyTime = 300f;
		[SerializeField] float maxDelayedDestroyTime = 900f;

		void OnDisable() {
			if (delayedDestroy) {
				Destroy(gameObject, Random.Range(minDelayedDestroyTime, maxDelayedDestroyTime));
			} else {
				Destroy(gameObject);
			}
		}
	}
}
