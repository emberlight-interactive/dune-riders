using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.OutpostAI.State;

namespace DuneRiders.OutpostAI {
	public class HealthStateMonitor : PriorityStateMonitor {
		HealthState healthState;
		float cachedHealth;

		public HealthStateMonitor(HealthState healthState) {
			this.healthState = healthState;
			cachedHealth = healthState.health;
		}

		public override bool StateChanged() {
			if (cachedHealth != healthState.health) {
				cachedHealth = healthState.health;
				return true;
			} else {
				return false;
			}
		}
	}
}
