using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.GunSystem;
using TMPro;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class WeaponDisplayUpdater : MonoBehaviour
	{
		[SerializeField] GunState cannon;
		[SerializeField] GunState missiles;
		[SerializeField] GunState machineGun;
		[SerializeField] TextMeshProUGUI gunNameText;
		[SerializeField] TextMeshProUGUI gunStateText;
		[SerializeField] GameObject gunStateBackground;

		void Start() {
			gunNameText = GetComponent<TextMeshProUGUI>();
		}

		void Update() {
			if (cannon.gameObject.activeSelf) {
				UpdateDisplay("Cannon", cannon);
			} else if (missiles.gameObject.activeSelf) {
				UpdateDisplay("Missiles", missiles);
			} else if (machineGun.gameObject.activeSelf) {
				UpdateDisplay("Turret", machineGun);
			} else {
				DisableDisplay();
			}
		}

		void UpdateDisplay(string weaponName, GunState state) {
			gunNameText.SetText(weaponName);
			gunStateText.SetText(GunStateToString(state));
			gunStateBackground.SetActive(true);
		}

		void DisableDisplay() {
			gunNameText.SetText("No Weapon");
			gunStateText.SetText("");
			gunStateBackground.SetActive(false);
		}

		string GunStateToString(GunState gunState) {
			switch (gunState.state) {
				case GunState.State.Ready:
					return "Ready";
				case GunState.State.Reloading:
					return "Reloading...";
				case GunState.State.Firing:
					return "Firing";
				default:
					return "...";
			}
		}
	}
}
