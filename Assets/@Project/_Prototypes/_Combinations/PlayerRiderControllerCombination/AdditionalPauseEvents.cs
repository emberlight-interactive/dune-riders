using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;
using Autohand.Demo;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class AdditionalPauseEvents : MonoBehaviour
	{
		[SerializeField] Hand rightHand;
		[SerializeField] Hand leftHand;

		// todo: Adding new grabbables will not be added here which will mean future grababbles could break pause
		[SerializeField] List<Grabbable> grabbables = new List<Grabbable>();

		public void EnableGrabbing() {
			EnableGrabbables();
		}

		public void DisableGrabbing() {
			rightHand.ForceReleaseGrab();
			leftHand.ForceReleaseGrab();
			DisableGrabbables();
		}

		void DisableGrabbables() {
			foreach (var grabbable in grabbables) {
				grabbable.enabled = false;
			}
		}

		void EnableGrabbables() {
			foreach (var grabbable in grabbables) {
				grabbable.enabled = true;
			}
		}
	}
}
