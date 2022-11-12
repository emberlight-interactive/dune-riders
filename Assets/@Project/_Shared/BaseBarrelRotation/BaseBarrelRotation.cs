using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.Shared {
	public class BaseBarrelRotation : MonoBehaviour
	{
		[Serializable]
		struct RotationAngleBounds {
			public float lowerBounds;
			public float upperBounds;
		}

		[SerializeField] Transform weaponBaseYPivot;
		[SerializeField] Transform weaponBarrelXPivot;
		[SerializeField] bool noYPivotBounds = false;
		[SerializeField] RotationAngleBounds weaponBaseYPivotBounds = new RotationAngleBounds() { lowerBounds = 270, upperBounds = 90};
		[SerializeField] RotationAngleBounds weaponBarrelXPivotBounds = new RotationAngleBounds() { lowerBounds = 15, upperBounds = 25};

		public float rotationSpeed = 1f;
		public Transform target;
		public Transform defaultTarget;

		void Update() {
			if (target != null) {
				IncrementTurretBarrelTowardsTarget(target.position);
				IncrementTurretBaseTowardsTarget(target.position);
			} else if (target == null && defaultTarget != null) {
				IncrementTurretBarrelTowardsTarget(defaultTarget.position);
				IncrementTurretBaseTowardsTarget(defaultTarget.position);
			}
		}

		void IncrementTurretBarrelTowardsTarget(Vector3 target) {
			float stepSpeed = rotationSpeed * Time.deltaTime;
			var targetDirection = (target - weaponBarrelXPivot.position).normalized;
			var targetRotation = Quaternion.LookRotation(targetDirection);
			var rotation = Quaternion.Slerp(weaponBarrelXPivot.rotation, targetRotation, stepSpeed);

			weaponBarrelXPivot.rotation = rotation;

			var angles = weaponBarrelXPivot.localEulerAngles;
			angles.z = 0;
			angles.y = 0;

			if (weaponBarrelXPivot.localEulerAngles.x >= 180 && weaponBarrelXPivot.localEulerAngles.x <= weaponBarrelXPivotBounds.lowerBounds) {
				angles.x = weaponBarrelXPivotBounds.lowerBounds;
			} else if (weaponBarrelXPivot.localEulerAngles.x < 180 && weaponBarrelXPivot.localEulerAngles.x >= weaponBarrelXPivotBounds.upperBounds) {
				angles.x = weaponBarrelXPivotBounds.upperBounds;
			}

			weaponBarrelXPivot.localEulerAngles = angles;
		}

		void IncrementTurretBaseTowardsTarget(Vector3 target) {
			float stepSpeed = rotationSpeed * Time.deltaTime;
			var targetDirection = (target - weaponBaseYPivot.position).normalized;
			var targetRotation = Quaternion.LookRotation(targetDirection);
			var rotation = Quaternion.Slerp(weaponBaseYPivot.rotation, targetRotation, stepSpeed);

			weaponBaseYPivot.rotation = rotation;

			if (noYPivotBounds) return;

			var angles = weaponBaseYPivot.localEulerAngles;
			angles.z = 0;
			angles.x = 0;

			if (weaponBaseYPivot.localEulerAngles.y >= 180 && weaponBaseYPivot.localEulerAngles.y <= weaponBaseYPivotBounds.lowerBounds) {
				angles.y = weaponBaseYPivotBounds.lowerBounds;
			} else if (weaponBaseYPivot.localEulerAngles.y < 180 && weaponBaseYPivot.localEulerAngles.y >= weaponBaseYPivotBounds.upperBounds) {
				angles.y = weaponBaseYPivotBounds.upperBounds;
			}

			weaponBaseYPivot.localEulerAngles = angles;
		}
	}
}
