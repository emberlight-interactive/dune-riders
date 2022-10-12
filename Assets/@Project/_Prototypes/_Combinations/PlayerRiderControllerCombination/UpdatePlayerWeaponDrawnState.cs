using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.State;

namespace DuneRiders.PlayerRiderControllerCombination {
	[RequireComponent(typeof(PlayerHasDrawnWeaponUpdater))]
	public class UpdatePlayerWeaponDrawnState : MonoBehaviour
	{
		PlayerHasDrawnWeaponUpdater playerHasDrawnWeaponUpdater;

		void Awake() {
			playerHasDrawnWeaponUpdater = GetComponent<PlayerHasDrawnWeaponUpdater>();
		}

		void Start() {
			InvokeRepeating(nameof(UpdateIsPlayerWeaponDrawnState), 0f, 1.5f);
		}

		void UpdateIsPlayerWeaponDrawnState() {
			playerHasDrawnWeaponUpdater.isPlayerWeaponDrawn = CheckIfAnyGunPrefabsAreActive();
		}

		bool CheckIfAnyGunPrefabsAreActive() {
			for (int i = 0; i< gameObject.transform.childCount; i++) {
				if (gameObject.transform.GetChild(i).gameObject.activeInHierarchy) return true;
			}

			return false;
		}
	}
}
