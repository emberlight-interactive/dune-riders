using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders {
	public class GameLossReason : MonoBehaviour
	{
		GameManager gm;

		void Awake() {
			gm = FindObjectOfType<GameManager>();
		}

		public void SetPlayerOutOfFuelReason() {
			gm.gameLossReason = "Player ran out of fuel";
		}

		public void SetPlayerDiedReason() {
			gm.gameLossReason = "Player rider was destroyed";
		}

		public void SetVillageOutOfFuel() {
			gm.gameLossReason = "Home village ran out of fuel";
		}
	}
}
