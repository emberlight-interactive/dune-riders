using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Actioners;
using DuneRiders.RiderAI.State;
using DuneRiders.RiderAI.Traits;
using DuneRiders.AI;

namespace DuneRiders.RiderAI.BehaviourTrees {
	[RequireComponent(typeof(HealthState))]
	[RequireComponent(typeof(InCombatState))]
	[RequireComponent(typeof(MoraleState))]
	public class JuliaRiderAI : BehaviourTree
	{
		[SerializeField] Actioner chargeAction;
		[SerializeField] Actioner gunnerAction;
		[SerializeField] Actioner traverseAction;
		[SerializeField] Actioner fleeAction;
		[SerializeField] Actioner deathAction;
		[SerializeField] Actioner despawnAction;
		HealthState healthState;
		InCombatState inCombatState;
		MoraleState moraleState;
		Player player;
		protected override (System.Type, string, System.Object)[] priorityStates {
			get => new (System.Type, string, System.Object)[] {
				(typeof(HealthState), "health", healthState)
			};
		}

		void Awake()
		{
			healthState = GetComponent<HealthState>();
			moraleState = GetComponent<MoraleState>();
			inCombatState = GetComponent<InCombatState>();
			player = FindObjectOfType<Player>();
		}

		protected override void ProcessBehaviourTree() {
			if (RiderHasLostAllHealth()) {
				SetActionersActive(deathAction);
			} else if (RiderIsPastMaxDistanceFromPlayer()) {
				SetActionersActive(despawnAction);
			} else if (RidersMoraleHasBeenDestroyed()) {
				SetActionersActive(fleeAction);
			// todo: Have we brutalized the player enough to continue traversing as we were (player company size reduction, is player fleeing)
			// Make sure code is set up for reengagement
			} else if (AmIEngagedInCombat()) {
				SetActionersActive(new Actioner[] {chargeAction, gunnerAction});
			} else {
				SetActionersActive(traverseAction);
			}
		}

		bool RidersMoraleHasBeenDestroyed() {
			return moraleState.morale == MoraleState.MoraleOptions.Broken;
		}

		bool RiderIsPastMaxDistanceFromPlayer() {
			if (!player) return false;
			return Vector3.Distance(transform.position, player.transform.position) > 1000;
		}

		bool AmIEngagedInCombat() {
			return inCombatState.inCombat;
		}

		bool RiderHasLostAllHealth() {
			return healthState.health <= 0;
		}
	}
}
