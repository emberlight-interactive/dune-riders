using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.GunSystem {
	public class PositionCurrentlyPointingAt : MonoBehaviour
	{
		[SerializeField] float maxRange = 700f;
		[SerializeField] Transform barrelTip;
		[SerializeField] LayerMask layer;

		[SerializeField] bool applyOffsetToRaycastStart = false;
		[SerializeField] Transform offsetDiffA;
		[SerializeField] Transform offsetDiffB;

		[SerializeField] bool matchRotation;
		[SerializeField] Transform transformRotationToMatch;

		public Vector3 GetPointedAtPosition() {
			var startingRaycastPosition = GetStartingRaycastPosition();
			var startingRaycastForwardVector = GetStartingRaycastForwardVector();

			Debug.DrawRay(startingRaycastPosition, startingRaycastForwardVector * maxRange, Color.red, 1);

			RaycastHit hit;
			if (Physics.Raycast(startingRaycastPosition, startingRaycastForwardVector, out hit, maxRange, layer)) {
				return hit.point;
			} else {
				return startingRaycastPosition + (startingRaycastForwardVector * maxRange);
			}
		}

		Vector3 GetStartingRaycastPosition() {
			if (!applyOffsetToRaycastStart) return barrelTip.position;

			var offset = offsetDiffB.position - offsetDiffA.position;

			return barrelTip.position + offset;
		}

		Vector3 GetStartingRaycastForwardVector() {
			if (matchRotation) {
				return transformRotationToMatch.rotation * barrelTip.forward;
			} else {
				return barrelTip.forward;
			}
		}
	}
}
