using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.GunSystem {
	[RequireComponent(typeof(GunState))]
	public class GunUnpacker : MonoBehaviour
	{
		[SerializeField] Transform barrel;
		GunState gunState;

		void Awake() {
			gunState = GetComponent<GunState>();
		}

		public void StartUnpack() {
			if (CanTheGunBeUnpackedRightNow()) {
				SetGunStateToTransitioning();
				StartCoroutine(Unpack());
			}
		}

		IEnumerator Unpack() {
			while (true) {
				yield return new WaitForSeconds(0.5f);
				ImmediateUnpack();
				yield return new WaitForSeconds(0.5f);
				SetGunStateToReady();
				yield break;
				// yield return null;
			}
		}

		void ImmediateUnpack() {
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
	}
}
