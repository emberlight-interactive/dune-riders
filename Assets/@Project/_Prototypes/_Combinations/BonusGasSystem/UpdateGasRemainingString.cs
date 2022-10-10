using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DuneRiders.BonusGasSystem {
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class UpdateGasRemainingString : MonoBehaviour
	{
		TextMeshProUGUI textMeshProUGUI;

		void Awake() {
			var textMeshProUGUI = GetComponent<TextMeshProUGUI>();
			var gm = FindObjectOfType<GameManager>();

			textMeshProUGUI.text = $"With a fuel bonus. Remaining ({gm.maxAllowedGasBonuses - gm.gasBonusesUsed})";
		}
	}
}
