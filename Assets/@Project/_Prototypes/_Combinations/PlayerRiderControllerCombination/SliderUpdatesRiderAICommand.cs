using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.State;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class SliderUpdatesRiderAICommand : MonoBehaviour
	{
		PlayerCommandState.PlayerCommandGlobalState globalCommandState;
		[SerializeField] PlayerCommandState.Command commandToSet;

		[SerializeField, Header("Pulse Command (Run a command before the intended one)")] bool pulseCommand = false;
		[SerializeField] PlayerCommandState.Command commandToPulse;

		public void UpdateRiderAICommand() {
			if (globalCommandState == null) LinkToGlobalState();
			if (globalCommandState == null) return;

			if (pulseCommand) StartCoroutine(PulseCommand());
			else globalCommandState.command = commandToSet;
		}

		void LinkToGlobalState() {
			globalCommandState = FindObjectOfType<PlayerCommandState.PlayerCommandGlobalState>();
		}

		IEnumerator PulseCommand() {
			globalCommandState.command = commandToPulse;
			yield return null;
			globalCommandState.command = commandToSet;
		}
	}
}
