using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.State;

namespace DuneRiders.RiderAI {
	[RequireComponent(typeof(HealthStateUpdater))]
	public class HealthUpdaterInitializer : MonoBehaviour
	{
		void Awake() {
			var healthStateUpdater = GetComponent<HealthStateUpdater>();
			if (!healthStateUpdater) return;

			healthStateUpdater.HealthState = GetComponentInParent<HealthState>();
		}
	}
}
