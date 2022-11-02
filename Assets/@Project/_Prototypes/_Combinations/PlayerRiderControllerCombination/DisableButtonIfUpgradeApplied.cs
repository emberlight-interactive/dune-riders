using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuneRiders.PlayerRiderControllerCombination {
	[RequireComponent(typeof(Button))]
	public class DisableButtonIfUpgradeApplied : MonoBehaviour
	{
		[SerializeField] PlayerTurretUpgrader playerTurretUpgrader;
		[SerializeField] PlayerTurretUpgrader.UpgradeType appliedUpgrade;

		Button button;

		void Awake() {
			button = GetComponent<Button>();
		}

		void OnEnable() {
			if (playerTurretUpgrader.HasUpgradeBeenApplied(appliedUpgrade)) {
				button.gameObject.SetActive(false);
			}
		}
	}
}
