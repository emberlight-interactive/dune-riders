using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.State;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class SliderUpdatesRiderAICommand : MonoBehaviour
	{
		PlayerCommandState.PlayerCommandGlobalState globalCommandState;
		[SerializeField] PlayerCommandState.Command commandToSet;

		void Start() {
			globalCommandState = FindObjectOfType<PlayerCommandState.PlayerCommandGlobalState>();
		}

		public void UpdateRiderAICommand() {
			globalCommandState.command = commandToSet;
		}
	}
}
