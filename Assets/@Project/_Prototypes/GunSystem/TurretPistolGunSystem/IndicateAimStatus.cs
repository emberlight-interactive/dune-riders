using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.GunSystem {
	public class IndicateAimStatus : MonoBehaviour
	{
		public enum AimStatus {
			FinishedAiming,
			AlmostFinishedAiming,
			Aiming,
		}

		AimStatus aimStatus = AimStatus.Aiming;

		[SerializeField] MeshRenderer indicator;
		[SerializeField] Material finishedAiming;
		[SerializeField] Material almostFinishedAiming;
		[SerializeField] Material aiming;

		public void UpdateAimStatus(AimStatus aimStatus) {
			if (this.aimStatus == aimStatus) return;
			this.aimStatus = aimStatus;

			switch(aimStatus) {
				case AimStatus.FinishedAiming:
					indicator.material = finishedAiming;
					return;
				case AimStatus.AlmostFinishedAiming:
					indicator.material = almostFinishedAiming;
					return;
				case AimStatus.Aiming:
					indicator.material = aiming;
					return;
				default:
					indicator.material = aiming;
					return;
			}
		}
	}
}
