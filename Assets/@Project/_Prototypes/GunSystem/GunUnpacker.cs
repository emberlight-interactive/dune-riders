using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.GunSystem {
	[RequireComponent(typeof(GunState))]
	public class GunUnpacker : MonoBehaviour
	{
		[SerializeField] Transform barrel;
		[SerializeField] float unpackingSpeed = 1f;
		GunState gunState;
		Vector3 barrelStartPosition;
		Vector3 barrelBackShiftedStartPosition;
		Quaternion barrelStartRotation;

		void Awake() {
			gunState = GetComponent<GunState>();
			barrelStartPosition = barrel.localPosition;
			barrelBackShiftedStartPosition = new Vector3(barrel.localPosition.x, barrel.localPosition.y, barrel.localPosition.z - 2);
			barrelStartRotation = barrel.localRotation;
		}

		public void StartUnpack() {
			if (CanTheGunBeUnpackedRightNow()) {
				SetGunStateToTransitioning();
				SetBarrelPositionToAPackedLocation();
				StartCoroutine(Unpack());
			}
		}

		IEnumerator Unpack() {
			ShowBarrel();

			while (true) {
				barrel.localPosition = Vector3.MoveTowards(barrel.localPosition, barrelStartPosition, 0.03f * unpackingSpeed);
				if (Vector3.Distance(barrel.localPosition, barrelStartPosition) == 0) {
					SetGunStateToReady();
					yield break;
				}
				yield return new WaitForFixedUpdate();
			}
		}

		void ShowBarrel() {
			barrel.gameObject.SetActive(true);
		}

		bool CanTheGunBeUnpackedRightNow() {
			return gunState.availableActions.canUnPack;
		}

		void SetGunStateToTransitioning() {
			gunState.state = GunState.State.Transitioning;
		}

		void SetGunStateToReady() {
			gunState.state = GunState.State.Ready;
		}

		void SetBarrelPositionToAPackedLocation() {
			barrel.localPosition = barrelBackShiftedStartPosition;
		}
	}
}
