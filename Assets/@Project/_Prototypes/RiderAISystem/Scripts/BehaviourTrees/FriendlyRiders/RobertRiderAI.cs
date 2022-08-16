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
	{ // todo: All states and actioners need to have enumerators started and stopped in onenable and disable events since they are pooled
		[SerializeField] Actioner followAction;
		[SerializeField] Actioner haltAction;
		[SerializeField] Actioner gunnerAction;
		[SerializeField] Actioner chargeAction;
		[SerializeField] Actioner deathAction;
		[SerializeField] Actioner leaveAction;
		[SerializeField] Actioner despawnAction;
		Rider rider;
		HealthState healthState;
		InCombatState inCombatState;
		AllActiveRidersState allActiveRidersState;
		PlayerCommandState playerCommandState;
		Player player;
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
			player = FindObjectOfType<Player>();
		}

		protected override void ProcessBehaviourTree() { // todo: Add a condition to respawn near player when an extreme distance away (typically when falling through the map)
			if (IsDisbanded()) {
				if (RiderIsPastMaxDistanceFromPlayer()) SetActionersActive(despawnAction);
				else SetActionersActive(leaveAction);
			} else if (RiderHasLostAllHealth()) {
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

		bool RiderIsPastMaxDistanceFromPlayer() {
			if (!player) return false;
			return Vector3.Distance(transform.position, player.transform.position) > 1000;
		}

		bool IsDisbanded() { // todo: Moving allegiance to mercenary for disbanding is gross and means combination scripts on friendly riders could still run on riders no longer associated with the player
			// todo: A new type of rider "mercenary" should be made for each type of rider and simply replaces the disbanded rider when the disband functionality is called. These mercenaries can stand in place of the hiring spots??
			return (rider.allegiance != Allegiance.Player);
		}
	}
}
