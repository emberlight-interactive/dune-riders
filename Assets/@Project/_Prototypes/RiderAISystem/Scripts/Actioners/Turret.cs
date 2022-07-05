using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.RiderAI.Actioners {
	public class Turret : MonoBehaviour
	{
		Rider riderCurrentlyTargetting;
		public float turretTurnSpeed = 1;

		void Update()
		{
			if (!riderCurrentlyTargetting) return;

			// Determine which direction to rotate towards
			Vector3 targetDirection = riderCurrentlyTargetting.gameObject.transform.position - transform.position;

			// The step size is equal to speed times frame time.
			float singleStep = turretTurnSpeed * Time.deltaTime;

			// Rotate the forward vector towards the target direction by one step
			Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

			// Draw a ray pointing at our target in
			Debug.DrawRay(transform.position, newDirection, Color.red);

			// Calculate a rotation a step closer to the target and applies rotation to this object
			transform.rotation = Quaternion.LookRotation(newDirection);
		}

		public void FireOnTarget(Rider rider, float yOffset = 0f) {
			if (riderCurrentlyTargetting == rider) return;
			if (riderCurrentlyTargetting == null) riderCurrentlyTargetting = rider;
			if (!riderCurrentlyTargetting.gameObject.activeSelf) {
				riderCurrentlyTargetting = null;
				return;
			}
		}
	}
}
