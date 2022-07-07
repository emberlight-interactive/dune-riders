using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Actioners;
using DuneRiders.RiderAI.State;

namespace DuneRiders.RiderAI.BehaviourTree {
	/// todo: Add a parent class for behaviour trees (or specifically rider ai trees)
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(HealthState))]
	public class JuliaRiderAI : MonoBehaviour
	{
		[SerializeField] Actioner chargeAndAttackAction;
		[SerializeField] Actioner deathAction;
		Actioner currentlyActiveActioner;
		HealthState healthState;
		int lastHealthState;

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
			} else if (RiderHasLowHealth()) {
				Flee();
			} else if (EnemyIsInRange()) {
				SetActionerActive(chargeAndAttackAction);
			} else {
				Traverse();
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
