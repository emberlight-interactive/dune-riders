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
		[SerializeField] Actioner deathAction;
		HealthState healthState;
		Command currentCommand = Command.Charge;
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
					HaltAndAttack();
				} else {
					SetActionerActive(followPlayerAndAttackAction);
				}
			} else if (IsCurrentCommand(Command.Halt)) {
				Halt();
			} else {
				FollowPlayer();
			}
		}

		#region ConditionalChecks

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

		#endregion

		#region Actions

		void HaltAndAttack() {}
		void Halt() {}
		void FollowPlayer() {}

		#endregion
	}
}
