using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.RiderAI.State {
	using PlayerDrawnWeapon = PlayerHasDrawnWeapon.PlayerHasDrawnWeaponGlobalState;
	public class PlayerHasDrawnWeaponUpdater : MonoBehaviour
	{
		PlayerDrawnWeapon playerDrawnWeapon;
		public bool isPlayerWeaponDrawn {
			set {
				if (playerDrawnWeapon != null) playerDrawnWeapon.isPlayerWeaponDrawn = value;
			}
		}

		public void AttachMyself(PlayerDrawnWeapon playerDrawnWeapon) {
			this.playerDrawnWeapon = playerDrawnWeapon;
		}
	}
}
