using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace DuneRiders.GunSystem {
	public class GunSwapController : MonoBehaviour
	{
		[System.Serializable]
		struct Gun {
			public GunState gunState;
			public GunPacker gunPacker;
			public GunUnpacker gunUnpacker;
		}

		[SerializeField] List<Gun> gunsToSwapBetween = new List<Gun>();
		[SerializeField] InputActionProperty gunTransitionButton;

		void Start() {
			gunTransitionButton.action.Enable();

			gunTransitionButton.action.performed += context =>
			{
				if (context.interaction is TapInteraction) {
					if (AreAnyGunGameObjectsActive()) {
						StartCoroutine(SwapWeapon());
						Debug.Log("Swap weapon");
					} else {
						StartCoroutine(UnpackFirstWeapon());
						Debug.Log("Unpack first weapon");
					}
				}

				if (context.interaction is HoldInteraction) {
					StartCoroutine(PackWeapon());
					Debug.Log("Pack weapon");
				}
			};
		}

		IEnumerator UnpackFirstWeapon() {
			var gun = gunsToSwapBetween[0];

			if (gun.gunState.availableActions.canActivate) {
				gun.gunState.gameObject.SetActive(true);
			}

			if (gun.gunState.availableActions.canUnPack) {
				gun.gunUnpacker.StartUnpack();
			}

			yield return null;
		}

		IEnumerator PackWeapon() {
			var activeGun = GetCurrentlyActiveGun();

			if (activeGun is Gun gun) {
				if (gun.gunState.availableActions.canPack) {
					gun.gunPacker.StartPack();

					while (!gun.gunState.availableActions.canDeactivate) {
						yield return null;
					}

					gun.gunState.gameObject.SetActive(false);
				}
			}
		}

		IEnumerator SwapWeapon() {
			var activeGun = GetCurrentlyActiveGun();

			if (activeGun is Gun gun) {
				if (gun.gunState.availableActions.canPack) {
					gun.gunPacker.StartPack();

					while (!gun.gunState.availableActions.canDeactivate) {
						yield return null;
					}

					gun.gunState.gameObject.SetActive(false);

					var nextGun = GetNextAvailableGunRelativeToCurrentGun(gun);

					if (nextGun.gunState.availableActions.canActivate) nextGun.gunState.gameObject.SetActive(true);
					if (nextGun.gunState.availableActions.canUnPack) nextGun.gunUnpacker.StartUnpack();
				}
			}
		}

		bool AreAnyGunGameObjectsActive() {
			foreach (var gun in gunsToSwapBetween) {
				if (gun.gunState.gameObject.activeSelf) return true;
			}

			return false;
		}

		Gun? GetCurrentlyActiveGun() {
			foreach (var gun in gunsToSwapBetween) {
				if (gun.gunState.gameObject.activeSelf) return gun;
			}

			return null;
		}

		Gun GetNextAvailableGunRelativeToCurrentGun(Gun gun) {
			var indexOfCurrentGun = gunsToSwapBetween.IndexOf(gun);
			if (gunsToSwapBetween.Count == (indexOfCurrentGun + 1)) return gunsToSwapBetween[0];
			else return gunsToSwapBetween[indexOfCurrentGun + 1];
		}
	}
}
