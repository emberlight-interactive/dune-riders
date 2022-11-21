using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class FastTravelIfThroughFloor : MonoBehaviour
	{
		[SerializeField] FastTravel fastTravel;
		[SerializeField] Transform fastTravelPos;
		[SerializeField] float yConsideredFalling = -300f;

		void Start() {
			InvokeRepeating(nameof(DetectAndHandleFalling), 0f, 2f);
		}

		void DetectAndHandleFalling() {
			if (transform.position.y < yConsideredFalling && !fastTravel.CurrentlyTeleporting) {
				var rb = GetComponent<Rigidbody>();
				if (rb != null) rb.velocity = Vector3.zero;

				fastTravel.entityToFastTravel = transform;
				fastTravel.positionToTravelTo = fastTravelPos;
				fastTravel.Teleport();
			}
		}
	}
}
