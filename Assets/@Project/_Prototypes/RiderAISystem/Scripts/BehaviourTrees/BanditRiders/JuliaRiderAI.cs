using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Actioners;
using DuneRiders.RiderAI.State;

namespace DuneRiders.RiderAI.BehaviourTree {
	/// todo: Add a parent class for behaviour trees (or specifically rider ai trees)
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(HealthState))]
	public class JuliaRiderAI : BehaviourTree
	{
		[SerializeField] Actioner chargeAndAttackAction;
		[SerializeField] Actioner deathAction;
		HealthState healthState;
		protected override (System.Type, string, System.Object)[] priorityStates {
			get => new (System.Type, string, System.Object)[] {
				(typeof(HealthState), "health", healthState)
			};
		}

		void Awake()
		{
			healthState = GetComponent<HealthState>();
		}

		protected override void ProcessBehaviourTree() {
			if (RiderHasLostAllHealth()) {
				SetActionerActive(deathAction);
			} else if (RiderHasLowHealth()) {
				Flee();
			} else if (EnemyIsInRange()) {
				SetActionerActive(chargeAndAttackAction);
			} else {
				Traverse();
			}
		}

		#region ConditionalChecks

		bool EnemyIsInRange() {
			return true;
		}

		bool RiderHasLostAllHealth() {
			return healthState.health <= 0;
		}

		bool RiderHasLowHealth() {
			return false;
		}

		#endregion

		#region Actions

		void Flee() {}
		void Traverse() {}

		#endregion
	}
}
