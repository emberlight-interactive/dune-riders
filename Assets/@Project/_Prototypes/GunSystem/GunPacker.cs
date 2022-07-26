using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.GunSystem {
	[RequireComponent(typeof(GunState))]
	public class GunPacker : MonoBehaviour
	{
		[SerializeField] Transform barrel;
		[SerializeField] Transform turretBase;
		[SerializeField] float retractSpeed = 2f;
		Vector3 barrelStartPosition;
		Quaternion barrelStartRotation;
		Quaternion turretBaseStartRotation;
		GunState gunState;

		void Awake() {
			gunState = GetComponent<GunState>();
			barrelStartPosition = barrel.localPosition;
			barrelStartRotation = barrel.localRotation;
			turretBaseStartRotation = turretBase.localRotation;
		}

		public void StartPack() {
			if (CanTheGunBePackedRightNow()) {
				SetGunStateToTransitioning();
				StartCoroutine(Pack());
			}
		}

		IEnumerator Pack() {
			foreach (var i in System.Linq.Enumerable.Range(0, 50)) {
				Vector3 retractDirection = -barrel.forward;
				retractDirection.y = 0;
				barrel.position += (retractDirection.normalized * Time.fixedDeltaTime) * retractSpeed;
				barrel.localRotation = Quaternion.RotateTowards(barrel.localRotation, barrelStartRotation, 20.0f * Time.fixedDeltaTime);
				turretBase.localRotation = Quaternion.RotateTowards(turretBase.localRotation, turretBaseStartRotation, 20.0f * Time.fixedDeltaTime);
				yield return new WaitForFixedUpdate();
			}

			HideBarrel();
			ResetPosition();
			SetGunStateToPacked();
		}

		void HideBarrel() {
			barrel.gameObject.SetActive(false);
		}

		void ResetPosition() {
			turretBase.localRotation = turretBaseStartRotation;
			barrel.localPosition = barrelStartPosition;
			barrel.localRotation = barrelStartRotation;
		}

		bool CanTheGunBePackedRightNow() {
			return gunState.availableActions.canPack;
		}

		void SetGunStateToTransitioning() {
			gunState.state = GunState.State.Transitioning;
		}

		void SetGunStateToPacked() {
			gunState.state = GunState.State.Packed;
		}
	}
}
