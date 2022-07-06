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
			if (riderCurrentlyTargetting) {
				IncrementTurretBarrelTowardsTarget(riderCurrentlyTargetting.gameObject.transform);
			}
		}

		public void FireOnTarget(Rider rider, float yOffset = 0f) {
			if (riderCurrentlyTargetting == rider) return;
			if (riderCurrentlyTargetting == null) riderCurrentlyTargetting = rider;
			if (!riderCurrentlyTargetting.gameObject.activeSelf) {
				riderCurrentlyTargetting = null;
				return;
			}
		}

		void IncrementTurretBarrelTowardsTarget(Transform target) {
			Vector3 targetDirection = target.position - transform.position;
			float singleStep = turretTurnSpeed * Time.deltaTime;
			Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
			Debug.DrawRay(transform.position, newDirection, Color.red);

			var rotation = Quaternion.LookRotation(newDirection);
			var rotationInEulerAngles = rotation.eulerAngles;
			rotationInEulerAngles.x = 0;
			rotationInEulerAngles.z = 0;

			rotation.eulerAngles = rotationInEulerAngles;

			transform.rotation = rotation;

			var localRotation = transform.localRotation.eulerAngles;
			if (localRotation.y < 270 && localRotation.y > 180) {
				localRotation.y = 270;
			} else if (localRotation.y > 90 && localRotation.y < 180) {
				localRotation.y = 90;
			}

			var newLocalRotation = new Quaternion();
			newLocalRotation.eulerAngles = localRotation;
			transform.localRotation = newLocalRotation;
		}
	}
}
