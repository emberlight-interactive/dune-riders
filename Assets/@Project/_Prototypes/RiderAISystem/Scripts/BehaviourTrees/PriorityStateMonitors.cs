using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.State;

namespace DuneRiders.RiderAI {
	public class CommandStateMonitor : PriorityStateMonitor {
		PlayerCommandState playerCommandState;
		PlayerCommandState.Command cachedCommand;

		public CommandStateMonitor(PlayerCommandState playerCommandState) {
			this.playerCommandState = playerCommandState;
			cachedCommand = playerCommandState.command;
		}

		public override bool StateChanged() {
			if (cachedCommand != playerCommandState.command) {
				cachedCommand = playerCommandState.command;
				return true;
			} else {
				return false;
			}
		}
	}

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
