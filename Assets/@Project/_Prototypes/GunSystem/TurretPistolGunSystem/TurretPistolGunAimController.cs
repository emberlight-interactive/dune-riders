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
			float stepSpeed = GetAimSensitivity(target) * Time.deltaTime;
			var targetDirection = (target - weaponBarrelXPivot.position).normalized;
			var targetRotation = Quaternion.LookRotation(targetDirection);
			var rotation = Quaternion.Slerp(weaponBarrelXPivot.rotation, targetRotation, stepSpeed);

			var rotationInEulerAngles = rotation.eulerAngles;
			rotationInEulerAngles.z = weaponBarrelXPivot.rotation.eulerAngles.z;
			rotationInEulerAngles.y = weaponBarrelXPivot.rotation.eulerAngles.y;

			if (rotationInEulerAngles.x >= 180 && rotationInEulerAngles.x <= weaponBarrelXPivotBounds.lowerBounds) {
				rotationInEulerAngles.x = weaponBarrelXPivotBounds.lowerBounds;
			} else if (rotationInEulerAngles.x < 180 && rotationInEulerAngles.x >= weaponBarrelXPivotBounds.upperBounds) {
				rotationInEulerAngles.x = weaponBarrelXPivotBounds.upperBounds;
			}

			rotation.eulerAngles = rotationInEulerAngles;
			weaponBarrelXPivot.rotation = rotation;
		}

		void IncrementTurretBaseTowardsTarget(Vector3 target) {
			float stepSpeed = GetAimSensitivity(target) * Time.deltaTime;
			var targetDirection = (target - weaponBaseYPivot.position).normalized;
			var targetRotation = Quaternion.LookRotation(targetDirection);
			var rotation = Quaternion.Slerp(weaponBaseYPivot.rotation, targetRotation, stepSpeed);

			var rotationInEulerAngles = rotation.eulerAngles;
			rotationInEulerAngles.z = weaponBaseYPivot.rotation.eulerAngles.z;
			rotationInEulerAngles.x = weaponBaseYPivot.rotation.eulerAngles.x;

			if (rotationInEulerAngles.y >= 180 && rotationInEulerAngles.y <= weaponBaseYPivotBounds.lowerBounds) {
				rotationInEulerAngles.y = weaponBaseYPivotBounds.lowerBounds;
			} else if (rotationInEulerAngles.y < 180 && rotationInEulerAngles.y >= weaponBaseYPivotBounds.upperBounds) {
				rotationInEulerAngles.y = weaponBaseYPivotBounds.upperBounds;
			}

			rotation.eulerAngles = rotationInEulerAngles;
			weaponBaseYPivot.rotation = rotation;
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

		float GetAimSensitivity(Vector3 target) {
			var rotationDifference = RotationDifference(target);

			if (rotationDifference <= 2f) {
				return aimSensitivity / 4;
			} else {
				return aimSensitivity;
			}
		}
	}
}
