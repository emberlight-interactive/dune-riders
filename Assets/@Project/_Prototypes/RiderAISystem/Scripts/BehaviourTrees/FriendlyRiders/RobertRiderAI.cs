using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Actioners;
using DuneRiders.RiderAI.State;

namespace DuneRiders.RiderAI.BehaviourTree {
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(HealthState))]
	public class RobertRiderAI : BehaviourTree
	{
		enum Command {Charge, Follow, Halt};
		[SerializeField] Actioner chargeAndAttackAction;
		[SerializeField] Actioner followPlayerAndAttackAction;
		[SerializeField] Actioner followAction;
		[SerializeField] Actioner haltAction;
		[SerializeField] Actioner haltAndAttackAction;
		[SerializeField] Actioner deathAction;
		HealthState healthState;
		[SerializeField] Command currentCommand = Command.Halt; // todo: Add state that tracks player ??? Add to priority state
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
			} else if (EnemyIsInRange()) {
				if (IsCurrentCommand(Command.Charge)) {
					SetActionerActive(chargeAndAttackAction);
				} else if (IsCurrentCommand(Command.Halt)) {
					SetActionerActive(haltAndAttackAction);
				} else {
					SetActionerActive(followPlayerAndAttackAction);
				}
			} else if (IsCurrentCommand(Command.Halt)) {
				SetActionerActive(haltAction);
			} else {
				SetActionerActive(followAction);
			}
		}

		bool EnemyIsInRange() {
			return true;
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
