using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.GunSystem {
	[RequireComponent(typeof(GunState))]
	public class GunPacker : MonoBehaviour
	{
		[SerializeField] Transform barrel;
		GunState gunState;

		void Awake() {
			gunState = GetComponent<GunState>();
		}

		public void StartPack() {
			if (CanTheGunBePackedRightNow()) {
				SetGunStateToTransitioning();
				StartCoroutine(Pack());
			}
		}

		IEnumerator Pack() {
			while (true) {
				yield return new WaitForSeconds(0.5f);
				ImmediatePack();
				yield return new WaitForSeconds(0.5f);
				SetGunStateToPacked();
				yield break;
				// yield return null;
			}
		}

		void ImmediatePack() {
			barrel.gameObject.SetActive(false);
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
