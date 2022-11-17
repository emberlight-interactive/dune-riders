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

		public void EnableGrabbing() {
			ToggleAllGrabbables(true);
		}

		public void DisableGrabbing() {
			DisableHand(rightHand);
			DisableHand(leftHand);

			ToggleAllGrabbables(false);
		}

		void DisableHand(Hand hand) {
			hand.Release();
			hand.ForceReleaseGrab();
			hand.Unsqueeze();
			hand.RelaxHand();
		}

		void ToggleAllGrabbables(bool on) {
			var grabbables = FindObjectsOfType<Grabbable>();

			foreach (var grabbable in grabbables) {
				var colliders = grabbable.GetComponentsInChildren<Collider>();
				foreach (var collider in colliders) {
					collider.enabled = on;
				}
			}
		}
	}
}
