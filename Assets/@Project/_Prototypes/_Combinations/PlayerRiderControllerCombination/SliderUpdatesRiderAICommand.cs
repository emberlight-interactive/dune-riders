using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.State;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class SliderUpdatesRiderAICommand : MonoBehaviour
	{
		PlayerCommandState.PlayerCommandGlobalState globalCommandState;
		[SerializeField] PlayerCommandState.Command commandToSet;

		public void UpdateRiderAICommand() {
			if (globalCommandState == null) LinkToGlobalState();

			globalCommandState.command = commandToSet;
		}

		void LinkToGlobalState() {
			globalCommandState = FindObjectOfType<PlayerCommandState.PlayerCommandGlobalState>();
		}
	}
}
