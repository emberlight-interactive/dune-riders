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
	[RequireComponent(typeof(PlayerFleeingState))]
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
		PlayerFleeingState playerFleeingState;
		Player player;

		PriorityStateMonitor[] _priorityStateMonitors;
		protected override PriorityStateMonitor[] priorityStateMonitors {
			get => _priorityStateMonitors;
		}

		void Awake()
		{
			healthState = GetComponent<HealthState>();
			moraleState = GetComponent<MoraleState>();
			inCombatState = GetComponent<InCombatState>();
			playerFleeingState = GetComponent<PlayerFleeingState>();
			player = FindObjectOfType<Player>();

			_priorityStateMonitors = new PriorityStateMonitor[] {
				new HealthStateMonitor(healthState),
			};
		}

		protected override void ProcessBehaviourTree() {
			if (RiderHasLostAllHealth()) {
				SetActionersActive(deathAction);
			} else if (RiderIsPastMaxDistanceFromPlayer()) {
				SetActionersActive(despawnAction);
			} else if (RidersMoraleHasBeenDestroyed()) {
				SetActionersActive(fleeAction);
			} else if (IsPlayerPartyFleeing()) {
				SetActionersActive(traverseAction);
			} else if (AmIEngagedInCombat()) {
				SetActionersActive(GenerateActionerList(chargeAction, gunnerAction));
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

		bool IsPlayerPartyFleeing() {
			return playerFleeingState.PlayerFleeing;
		}
	}
}
