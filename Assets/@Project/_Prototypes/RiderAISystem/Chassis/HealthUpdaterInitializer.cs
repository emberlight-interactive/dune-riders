using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.State;

namespace DuneRiders.RiderAI {
	[RequireComponent(typeof(DamageableRiderAI))]
	public class HealthUpdaterInitializer : MonoBehaviour
	{
		void Awake() {
			var damageableRiderAI = GetComponent<DamageableRiderAI>();
			if (!damageableRiderAI) return;

			damageableRiderAI.HealthState = GetComponentInParent<HealthState>();
		}
	}
}
