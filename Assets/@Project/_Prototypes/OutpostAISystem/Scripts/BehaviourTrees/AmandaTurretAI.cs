using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.OutpostAI.State;
using DuneRiders.AI;

namespace DuneRiders.OutpostAI.BehaviourTrees {
	[RequireComponent(typeof(InCombatState))]
	[RequireComponent(typeof(HealthState))]
	public class AmandaTurretAI : BehaviourTree
	{
		[SerializeField] Actioner gunnerAction;
		[SerializeField] Actioner deathAction;
		[SerializeField] Actioner idleAction;

		InCombatState inCombatState;
		HealthState healthState;

		PriorityStateMonitor[] _priorityStateMonitors;
		protected override PriorityStateMonitor[] priorityStateMonitors {
			get => _priorityStateMonitors;
		}

		void Awake() {
			healthState = GetComponent<HealthState>();
			inCombatState = GetComponent<InCombatState>();

			_priorityStateMonitors = new PriorityStateMonitor[] {
				new HealthStateMonitor(healthState),
			};
		}

		protected override void ProcessBehaviourTree() {
			if (TurretHasLostAllHealth()) {
				SetActionersActive(deathAction);
			} else if (AmIEngagedInCombat()) {
				SetActionersActive(gunnerAction);
			} else {
				SetActionersActive(idleAction);
			}
		}

		bool AmIEngagedInCombat() {
			return inCombatState.inCombat;
		}

		bool TurretHasLostAllHealth() {
			return healthState.health <= 0;
		}
	}
}
