using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Actioners;

namespace DuneRiders.RiderAI.BehaviourTree {
	[RequireComponent(typeof(RiderAI.State.AllActiveRidersState))]
	public class RobertRiderAI : MonoBehaviour
	{
		[SerializeField] Actioner chargeAndAttackAction;
		Actioner currentlyActiveActioner;
		enum Command {Charge, Follow, Halt};

		void Start()
		{
			StartCoroutine(RunBehaviourTree());
		}

		IEnumerator RunBehaviourTree()
		{
			while (true) {
				if (EnemyIsInRange()) {
					if (IsCurrentCommand(Command.Charge)) {
						SetActionerActive(chargeAndAttackAction);
					} else if (IsCurrentCommand(Command.Halt)) {
						HaltAndAttack();
					} else {
						FollowPlayerAndAttack();
					}
				} else if (IsCurrentCommand(Command.Halt)) {
					Halt();
				} else {
					FollowPlayer();
				}

				yield return new WaitForSeconds(2.5f);
			}
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
			if (command == Command.Charge) return true;
			return false;
		}

		#endregion

		#region Actions

		void HaltAndAttack() {}
		void FollowPlayerAndAttack() {}
		void Halt() {}
		void FollowPlayer() {}

		#endregion
	}
}
