using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Actioners;
using DuneRiders.RiderAI.State;
using DuneRiders.RiderAI.Traits;
using DuneRiders.AI;

namespace DuneRiders.RiderAI.BehaviourTrees {
	[RequireComponent(typeof(Rider))]
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(HealthState))]
	[RequireComponent(typeof(PlayerCommandState))]
	[RequireComponent(typeof(EntitiesWithinGroupsDetectionRange))]
	[RequireComponent(typeof(PlayerHasDrawnWeapon))]
	public class RobertRiderAI : BehaviourTree
	{
		[SerializeField] Actioner followAction;
		[SerializeField] Actioner haltAction;
		[SerializeField] Actioner gunnerAction;
		[SerializeField] Actioner chargeAction;
		[SerializeField] Actioner deathAction;
		[SerializeField] Actioner teleportNearPlayerAction;
		Rider rider;
		HealthState healthState;
		AllActiveRidersState allActiveRidersState;
		PlayerCommandState playerCommandState;
		EntitiesWithinGroupsDetectionRange entitiesWithinGroupsDetectionRange;
		PlayerHasDrawnWeapon playerHasDrawnWeapon;
		Player player;

		PriorityStateMonitor[] _priorityStateMonitors;
		protected override PriorityStateMonitor[] priorityStateMonitors {
			get => _priorityStateMonitors;
		}

		void Awake()
		{
			rider = GetComponent<Rider>();
			healthState = GetComponent<HealthState>();
			allActiveRidersState = GetComponent<AllActiveRidersState>();
			playerCommandState = GetComponent<PlayerCommandState>();
			entitiesWithinGroupsDetectionRange = GetComponent<EntitiesWithinGroupsDetectionRange>();
			playerHasDrawnWeapon = GetComponent<PlayerHasDrawnWeapon>();
			player = FindObjectOfType<Player>();

			_priorityStateMonitors = new PriorityStateMonitor[] {
				new HealthStateMonitor(healthState),
				new CommandStateMonitor(playerCommandState),
			};
		}

		protected override void ProcessBehaviourTree() {
			if (RiderHasLostAllHealth()) {
				SetActionersActive(deathAction);
			} else if (RiderIsPastMaxDistanceFromPlayer() && !IsCurrentCommand(PlayerCommandState.Command.Halt)) {
				SetActionersActive(teleportNearPlayerAction);
			} else if (IsCurrentCommand(PlayerCommandState.Command.Charge)) {
				if (AreAnyEnemiesInDetectionRange()) {
					SetActionersActive(GenerateActionerList(chargeAction, gunnerAction));
				} else {
					SetActionersActive(followAction);
				}
			} else if (IsCurrentCommand(PlayerCommandState.Command.Halt)) {
				if (AreAnyEnemiesInDetectionRange() && IsPlayerWeaponDrawn()) {
					SetActionersActive(GenerateActionerList(haltAction, gunnerAction));
				} else if (AreAnyEnemiesInThreatRange()) {
					SetActionersActive(GenerateActionerList(haltAction, gunnerAction));
				} else {
					SetActionersActive(haltAction);
				}
			} else if (IsCurrentCommand(PlayerCommandState.Command.Follow)) {
				if (AreAnyEnemiesInDetectionRange() && IsPlayerWeaponDrawn()) {
					SetActionersActive(GenerateActionerList(followAction, gunnerAction));
				} else if (AreAnyEnemiesInThreatRange()) {
					SetActionersActive(GenerateActionerList(followAction, gunnerAction));
				} else {
					SetActionersActive(followAction);
				}
			} else {
				SetActionersActive(followAction);
			}
		}

		bool DoAnyEnemyRidersExist() {
			return allActiveRidersState.GetAllRidersOfAllegiance(rider.enemyAllegiance).Count > 0;
		}

		bool IsPlayerWeaponDrawn() {
			return playerHasDrawnWeapon.isPlayerWeaponDrawn;
		}

		bool AreAnyEnemiesInDetectionRange() {
			return entitiesWithinGroupsDetectionRange.areAnyEnemyEntitiesWithinDetectionRange;
		}

		bool AreAnyEnemiesInThreatRange() {
			return entitiesWithinGroupsDetectionRange.areAnyEnemyEntitiesWithinThreatRange;
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
			return Vector3.Distance(transform.position, player.transform.position) > 1200;
		}
	}
}
