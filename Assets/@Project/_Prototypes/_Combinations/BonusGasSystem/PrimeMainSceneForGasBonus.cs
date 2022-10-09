using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.BonusGasSystem {
	public class PrimeMainSceneForGasBonus : MonoBehaviour
	{
		GameManager gameManager;

		void Awake() {
			gameManager = FindObjectOfType<GameManager>();
		}

		public void ApplyGasBonusOnMainSceneLoad() { gameManager.applyGasBonusOnMainSceneLoad = true; }
	}
}
