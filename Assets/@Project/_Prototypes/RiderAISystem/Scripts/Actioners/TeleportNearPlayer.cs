using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.Traits;
using Pathfinding;

namespace DuneRiders.RiderAI.Actioners {
	[RequireComponent(typeof(RichAI))]
	public class TeleportNearPlayer : Actioner
	{
		RichAI pathfinding;
		Player player;

		bool _currentlyActive = false;
		public override bool currentlyActive {
			get => _currentlyActive;
		}

		void Awake() {
			player = FindObjectOfType<Player>();
			pathfinding = GetComponent<RichAI>();
		}

		public override void StartAction()
		{
			TeleportMyselfNearPlayer();
		}

		public override void EndAction() {}


		void TeleportMyselfNearPlayer() {
			pathfinding.Teleport(player.transform.position - player.transform.forward * 20);
		}
	}
}
