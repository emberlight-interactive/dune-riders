using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Actioners;
using DuneRiders.RiderAI.State;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAI.BehaviourTree {
	[RequireComponent(typeof(InCombatState))]
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(HealthState))]
	[RequireComponent(typeof(Rider))]
	public class RobertRiderAI : BehaviourTree
	{
		enum Command {Charge, Follow, Halt};
		[SerializeField] Actioner followAction;
		[SerializeField] Actioner haltAction;
		[SerializeField] Actioner gunnerAction;
		[SerializeField] Actioner chargeAction;
		[SerializeField] Actioner deathAction;
		Rider rider;
		HealthState healthState;
		InCombatState inCombatState;
		AllActiveRidersState allActiveRidersState;
		[SerializeField] Command currentCommand = Command.Halt; // todo: Add state that tracks player ??? Add to priority state
		protected override (System.Type, string, System.Object)[] priorityStates {
			get => new (System.Type, string, System.Object)[] {
				(typeof(HealthState), "health", healthState)
			};
		}

		void Awake()
		{
			healthState = GetComponent<HealthState>();
			inCombatState = GetComponent<InCombatState>();
			allActiveRidersState = GetComponent<AllActiveRidersState>();
			rider = GetComponent<Rider>();
		}

		protected override void ProcessBehaviourTree() {
			if (RiderHasLostAllHealth()) {
				SetActionersActive(deathAction);
			} else if (AmIEngagedInCombat()) {
				if (IsCurrentCommand(Command.Charge)) {
					SetActionersActive(new Actioner[] {chargeAction, gunnerAction});
				} else if (IsCurrentCommand(Command.Halt)) {
					SetActionersActive(new Actioner[] {haltAction, gunnerAction});
				} else {
					SetActionersActive(new Actioner[] {followAction, gunnerAction});
				}
			} else if (IsCurrentCommand(Command.Charge)) {
				if (DoAnyEnemyRidersExist()) {
					SetActionersActive(chargeAction);
				} else {
					SetActionersActive(followAction);
				}
			} else if (IsCurrentCommand(Command.Halt)) {
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

		bool IsCurrentCommand(Command command) {
			if (command == currentCommand) return true;
			return false;
		}

		bool RiderHasLostAllHealth() {
			return healthState.health <= 0;
		}
	}
}
