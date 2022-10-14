using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace DuneRiders.GunSystem {
	public class TurretPistolGunSwapController : MonoBehaviour
	{
		[System.Serializable]
		struct Gun {
			public GunState gunState;
			public GunPacker gunPacker;
			public GunUnpacker gunUnpacker;
		}

		[SerializeField] List<Gun> gunsToSwapBetween = new List<Gun>();
		[SerializeField] InputActionProperty gunTransitionButton;
		[HideInInspector] public bool turretPistolIsHolstered = false;

		void Start() {
			gunTransitionButton.action.Enable();

			gunTransitionButton.action.performed += context =>
			{
				if (!turretPistolIsHolstered && context.interaction is TapInteraction) {
					ToggleWeapons();
				}
			};

			InvokeRepeating(nameof(DetectAndPackWeaponsWhenGunIsHolstered), 0f, 1f);
			InvokeRepeating(nameof(DetectAndUnPackWeaponsWhenGunIsUnHolstered), 0f, 1f);
		}

		public void ToggleWeapons() {
			if (AreAnyGunGameObjectsActive()) {
				if (gameObject.activeInHierarchy) StartCoroutine(SwapWeapon());
			} else {
				if (gameObject.activeInHierarchy) StartCoroutine(UnpackFirstWeapon());
			}
		}

		public void WeaponIsUnHolstered() {
			turretPistolIsHolstered = false;
		}

		public void WeaponIsHolstered() {
			turretPistolIsHolstered = true;
		}

		void DetectAndPackWeaponsWhenGunIsHolstered() {
			var activeGun = GetCurrentlyActiveGun();
			if (turretPistolIsHolstered && activeGun != null) StartCoroutine(PackWeapon());
		}

		void DetectAndUnPackWeaponsWhenGunIsUnHolstered() {
			var activeGun = GetCurrentlyActiveGun();
			if (!turretPistolIsHolstered && activeGun == null) ToggleWeapons();
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
