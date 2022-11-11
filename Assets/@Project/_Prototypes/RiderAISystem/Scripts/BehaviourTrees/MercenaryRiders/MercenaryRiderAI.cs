using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.Traits;
using DuneRiders.RiderAI.State;

namespace DuneRiders.RiderAI.BehaviourTrees {
	[RequireComponent(typeof(IsParkedState))]
	public class MercenaryRiderAI : BehaviourTree
	{
		[SerializeField] Actioner idleAction;
		[SerializeField] Actioner leaveAction;
		[SerializeField] Actioner despawnAction;
		Player player;
		IsParkedState isParkedState;

		PriorityStateMonitor[] _priorityStateMonitors;
		protected override PriorityStateMonitor[] priorityStateMonitors {
			get => _priorityStateMonitors;
		}

		void Awake() {
			player = FindObjectOfType<Player>();
			isParkedState = GetComponent<IsParkedState>();

			_priorityStateMonitors = new PriorityStateMonitor[] {};
		}

		protected override void ProcessBehaviourTree() {
			if (IsParked()) {
				SetActionersActive(idleAction);
			} else {
				if (RiderIsPastMaxDistanceFromPlayer()) SetActionersActive(despawnAction);
				else SetActionersActive(leaveAction);
			}
		}

		bool IsParked() {
			return isParkedState.isParked;
		}

		bool RiderIsPastMaxDistanceFromPlayer() {
			if (!player) return false;
			return Vector3.Distance(transform.position, player.transform.position) > 1000;
		}
	}
}
