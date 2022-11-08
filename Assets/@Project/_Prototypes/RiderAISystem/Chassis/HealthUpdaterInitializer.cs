using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.State;

namespace DuneRiders.RiderAI {
	[RequireComponent(typeof(DamageableRiderAI))]
	public class HealthUpdaterInitializer : MonoBehaviour
	{
		DamageableRiderAI damageableRiderAI;
		bool shouldDamageableComponentComeWithMeToTheKamakazyGrave = false;

		void Awake() {
			damageableRiderAI = GetComponent<DamageableRiderAI>();
			if (!damageableRiderAI) return;

			var healthState = GetComponentInParent<HealthState>();
			if (healthState) {
				damageableRiderAI.HealthState = healthState;
			} else {
				shouldDamageableComponentComeWithMeToTheKamakazyGrave = true;
				Destroy(this);
			}
		}

		void OnDestroy() {
			if (shouldDamageableComponentComeWithMeToTheKamakazyGrave && damageableRiderAI != null) Destroy(damageableRiderAI);
		}
	}
}
