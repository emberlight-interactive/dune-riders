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

		public Vector3 GetPointedAtPosition() {
			var getStartingRaycastPosition = GetStartingRaycastPosition();

			Debug.DrawRay(getStartingRaycastPosition, barrelTip.forward * maxRange, Color.red, 1);

			RaycastHit hit;
			if (Physics.Raycast(getStartingRaycastPosition, barrelTip.forward, out hit, maxRange, layer)) {
				return hit.point;
			} else {
				return getStartingRaycastPosition + (barrelTip.forward * maxRange);
			}
		}

		Vector3 GetStartingRaycastPosition() {
			if (!applyOffsetToRaycastStart) return barrelTip.position;

			var offset = offsetDiffB.position - offsetDiffA.position;

			return barrelTip.position + offset;
		}
	}
}
