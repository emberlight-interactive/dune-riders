using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Actioners;
using DuneRiders.RiderAI.State;

namespace DuneRiders.RiderAI.BehaviourTree {
	[RequireComponent(typeof(HealthState))]
	[RequireComponent(typeof(InCombatState))]
	public class JuliaRiderAI : BehaviourTree
	{
		[SerializeField] Actioner chargeAndAttackAction;
		[SerializeField] Actioner traverseAction;
		[SerializeField] Actioner deathAction;
		HealthState healthState;
		InCombatState inCombatState;
		protected override (System.Type, string, System.Object)[] priorityStates {
			get => new (System.Type, string, System.Object)[] {
				(typeof(HealthState), "health", healthState)
			};
		}

		void Awake()
		{
			healthState = GetComponent<HealthState>();
			inCombatState = GetComponent<InCombatState>();
		}

		protected override void ProcessBehaviourTree() {
			if (RiderHasLostAllHealth()) {
				SetActionerActive(deathAction);
			} else if (RiderIsPastMaxDistanceFromPlayer()) {
				DespawnMyself();
			} else if (RiderHasLowHealth()) {
				Flee();
			} else if (AmIEngagedInCombat()) {
				SetActionerActive(chargeAndAttackAction);
			} else {
				SetActionerActive(traverseAction);
			}
		}

		bool RiderHasLowHealth() {
			return false;
		}

		bool RiderIsPastMaxDistanceFromPlayer() {
			return false;
		}

		bool AmIEngagedInCombat() {
			return inCombatState.inCombat;
		}

		bool RiderHasLostAllHealth() {
			return healthState.health <= 0;
		}



		void Flee() {}
		void DespawnMyself() {}
	}
}
