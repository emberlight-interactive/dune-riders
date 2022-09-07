using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using Pathfinding.RVO;

namespace DuneRiders.RiderAI.Actioners {
	public class Idle : Actioner
	{
		[SerializeField] bool preventPushing = false;

		public override bool currentlyActive {get => true;}

		public override void StartAction() {
			if (preventPushing) {
				GetComponent<RVOController>().enabled = false;
			}
		}

		public override void EndAction() {
			if (preventPushing) {
				GetComponent<RVOController>().enabled = true;
			}
		}
	}
}
