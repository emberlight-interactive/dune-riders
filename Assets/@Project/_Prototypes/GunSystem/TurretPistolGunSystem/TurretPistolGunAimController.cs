using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DuneRiders.GunSystem {
	[RequireComponent(typeof(GunState))]
	public class TurretPistolGunAimController : MonoBehaviour
	{
		[Serializable]
		struct RotationAngleBounds {
			public float lowerBounds;
			public float upperBounds;
		}

		[SerializeField] float aimSensitivity = 1f;
		[SerializeField] Transform weaponBaseYPivot;
		[SerializeField] Transform weaponBarrelXPivot;
		[SerializeField] PositionCurrentlyPointingAt turretPistol;
		[SerializeField] IndicateAimStatus indicateAimStatus;
		[SerializeField] RotationAngleBounds weaponBaseYPivotBounds = new RotationAngleBounds() { lowerBounds = 90, upperBounds = 90};
		[SerializeField] RotationAngleBounds weaponBarrelXPivotBounds = new RotationAngleBounds() { lowerBounds = 15, upperBounds = 25};
		GunState gunState;

		void Start() {
			gunState = GetComponent<GunState>();
		}

		void Update() {
			if (IsTheGunInAnAimableState()) {
				var target = turretPistol.GetPointedAtPosition();
				IncrementTurretBarrelTowardsTarget(target);
				IncrementTurretBaseTowardsTarget(target);
				UpdateAimStatusIndicator(target);
			}
		}

		bool IsTheGunInAnAimableState() {
			return gunState.availableActions.canAim;
		}

		void IncrementTurretBarrelTowardsTarget(Vector3 target) {
			float stepSpeed = aimSensitivity * Time.deltaTime;
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
			float stepSpeed = aimSensitivity * Time.deltaTime;
			var targetDirection = (target - weaponBaseYPivot.position).normalized;
			var targetRotation = Quaternion.LookRotation(targetDirection);
			var rotation = Quaternion.Slerp(weaponBaseYPivot.rotation, targetRotation, stepSpeed);

			weaponBaseYPivot.rotation = rotation;

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

		float RotationDifference(Vector3 target) {
			var targetDirection = (target - weaponBarrelXPivot.position).normalized;
			var targetRotation = Quaternion.LookRotation(targetDirection);
			var rotationDifference = Quaternion.Angle(weaponBarrelXPivot.rotation, targetRotation);
			return rotationDifference;
		}

		void UpdateAimStatusIndicator(Vector3 target) {
			var rotationDifference = RotationDifference(target);

			if (rotationDifference <= 1f) {
				indicateAimStatus.UpdateAimStatus(IndicateAimStatus.AimStatus.FinishedAiming);
			} else if (rotationDifference <= 20f) {
				indicateAimStatus.UpdateAimStatus(IndicateAimStatus.AimStatus.AlmostFinishedAiming);
			} else {
				indicateAimStatus.UpdateAimStatus(IndicateAimStatus.AimStatus.Aiming);
			}
		}
	}
}
