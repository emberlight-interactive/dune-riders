using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Actioners;
using DuneRiders.RiderAI.State;

namespace DuneRiders.RiderAI.BehaviourTree {
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(HealthState))]
	public class RobertRiderAI : MonoBehaviour
	{
		[SerializeField] Actioner chargeAndAttackAction;
		[SerializeField] Actioner followPlayerAndAttackAction;
		[SerializeField] Actioner deathAction;
		Actioner currentlyActiveActioner;
		HealthState healthState;
		int lastHealthState;
		enum Command {Charge, Follow, Halt};
		Command currentCommand = Command.Charge;

		void Start()
		{
			healthState = GetComponent<HealthState>();
			StartCoroutine(RunBehaviourTree());
		}

		void FixedUpdate() {
			if (HaveUpdatesOccuredForPriorityStates()) {
				ImmediatelyComputeDecision();
			}
		}

		IEnumerator RunBehaviourTree()
		{
			while (true) {
				BehaviourTree();
				yield return new WaitForSeconds(2.5f);
			}
		}

		void BehaviourTree() {
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

		void ImmediatelyComputeDecision() {
			BehaviourTree();
		}

		bool HaveUpdatesOccuredForPriorityStates() {
			if (lastHealthState != healthState.health) {
				lastHealthState = healthState.health;
				return true;
			}

			return false;
		}

		void SetActionerActive(Actioner actioner) {
			if (currentlyActiveActioner == null) {
				currentlyActiveActioner = actioner;
				currentlyActiveActioner.StartAction();
			} else if (currentlyActiveActioner == actioner && !currentlyActiveActioner.currentlyActive) {
				currentlyActiveActioner.StartAction();
			} else if (currentlyActiveActioner != actioner) {
				if (currentlyActiveActioner.currentlyActive) currentlyActiveActioner.EndAction();

				currentlyActiveActioner = actioner;
				currentlyActiveActioner.StartAction();
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
