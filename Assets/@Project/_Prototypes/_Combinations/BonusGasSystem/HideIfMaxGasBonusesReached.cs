using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.BonusGasSystem {
	public class HideIfMaxGasBonusesReached : MonoBehaviour
	{
		void Awake() {
			var gm = FindObjectOfType<GameManager>();
			if (gm.gasBonusesUsed == gm.maxAllowedGasBonuses) gameObject.SetActive(false);
		}
	}
}
