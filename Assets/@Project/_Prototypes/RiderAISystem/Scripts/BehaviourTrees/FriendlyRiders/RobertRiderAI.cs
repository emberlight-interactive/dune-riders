using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Actioners;
using DuneRiders.RiderAI.State;
using DuneRiders.RiderAI.Traits;
using DuneRiders.AI;

namespace DuneRiders.RiderAI.BehaviourTrees {
	[RequireComponent(typeof(InCombatState))]
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(HealthState))]
	[RequireComponent(typeof(PlayerCommandState))]
	[RequireComponent(typeof(Rider))]
	public class RobertRiderAI : BehaviourTree // todo: Be mindful of RVO shoving riders off pathfinding meshes
	{
		[SerializeField] Actioner followAction;
		[SerializeField] Actioner haltAction;
		[SerializeField] Actioner gunnerAction;
		[SerializeField] Actioner chargeAction;
		[SerializeField] Actioner deathAction;
		Rider rider;
		HealthState healthState;
		InCombatState inCombatState;
		AllActiveRidersState allActiveRidersState;
		PlayerCommandState playerCommandState;
		protected override (System.Type, string, System.Object)[] priorityStates {
			get => new (System.Type, string, System.Object)[] {
				(typeof(PlayerCommandState), "command", playerCommandState),
				(typeof(HealthState), "health", healthState)
			};
		}

		void Awake()
		{
			healthState = GetComponent<HealthState>();
			inCombatState = GetComponent<InCombatState>();
			allActiveRidersState = GetComponent<AllActiveRidersState>();
			playerCommandState = GetComponent<PlayerCommandState>();
			rider = GetComponent<Rider>();
		}

		protected override void ProcessBehaviourTree() {
			if (RiderHasLostAllHealth()) {
				SetActionersActive(deathAction);
			} else if (AmIEngagedInCombat()) {
				if (IsCurrentCommand(PlayerCommandState.Command.Charge)) {
					SetActionersActive(new Actioner[] {chargeAction, gunnerAction});
				} else if (IsCurrentCommand(PlayerCommandState.Command.Halt)) {
					SetActionersActive(new Actioner[] {haltAction, gunnerAction});
				} else {
					SetActionersActive(new Actioner[] {followAction, gunnerAction});
				}
			} else if (IsCurrentCommand(PlayerCommandState.Command.Charge)) {
				if (DoAnyEnemyRidersExist()) { // todo: take into account our detection distance (add this to traits???) // what happens if one charges but not the others ???
					SetActionersActive(chargeAction);
				} else {
					SetActionersActive(followAction);
				}
			} else if (IsCurrentCommand(PlayerCommandState.Command.Halt)) {
				SetActionersActive(haltAction);
			} else {
				SetActionersActive(followAction);
			}
		}

		bool DoAnyEnemyRidersExist() {
			return allActiveRidersState.GetAllRidersOfAllegiance(rider.enemyAllegiance).Count > 0;
		}

		bool AmIEngagedInCombat() {
			return inCombatState.inCombat;
		}

		bool IsCurrentCommand(PlayerCommandState.Command command) {
			if (command == playerCommandState.command) return true;
			return false;
		}

		bool RiderHasLostAllHealth() {
			return healthState.health <= 0;
		}
	}
}