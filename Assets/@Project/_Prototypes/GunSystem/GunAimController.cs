using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DuneRiders.GunSystem {
	[RequireComponent(typeof(GunState))]
	public class GunAimController : MonoBehaviour
	{
		[Serializable]
		struct RotationAngleBounds {
			public float lowerBounds;
			public float upperBounds;
		}

		[SerializeField] float aimSensetivity = 1f;
		[SerializeField] InputActionProperty aimInput;
		[SerializeField] Transform weaponBaseYPivot;
		[SerializeField] Transform weaponBarrelXPivot;
		[SerializeField] RotationAngleBounds weaponBaseYPivotBounds = new RotationAngleBounds() { lowerBounds = 90, upperBounds = 90};
		[SerializeField] RotationAngleBounds weaponBarrelXPivotBounds = new RotationAngleBounds() { lowerBounds = 15, upperBounds = 25};
		GunState gunState;

		void Start() {
			aimInput.action.Enable();
			gunState = GetComponent<GunState>();
		}

		void Update() {
			if (IsTheGunInAnAimableState()) {
				UpdateRotationOfGunBasedOnAimInput();
			}
		}

		bool IsTheGunInAnAimableState() {
			return gunState.availableActions.canAim;
		}

		void UpdateRotationOfGunBasedOnAimInput() {
			if (aimInput.action.ReadValue<Vector2>() != Vector2.zero)
			{
				var xRot = aimInput.action.ReadValue<Vector2>().x;
				var rotateAmount = xRot * (aimSensetivity * Time.deltaTime);

				Vector3 currentRotation = weaponBaseYPivot.transform.localEulerAngles;
				currentRotation.y = currentRotation.y % 360;

				if (currentRotation.y > 180)
					currentRotation.y -= 360f;

				currentRotation.y = Mathf.Clamp(currentRotation.y + rotateAmount, -weaponBaseYPivotBounds.lowerBounds, weaponBaseYPivotBounds.upperBounds);
				weaponBaseYPivot.transform.localRotation = Quaternion.Euler(currentRotation);

				var yRot = aimInput.action.ReadValue<Vector2>().y;
				var pivotRotAmount = yRot * (aimSensetivity * Time.deltaTime);

				currentRotation = weaponBarrelXPivot.transform.localEulerAngles;
				currentRotation.x = currentRotation.x % 360;

				if (currentRotation.x > 180)
					currentRotation.x -= 360f;

				currentRotation.x = Mathf.Clamp(currentRotation.x - pivotRotAmount, -weaponBarrelXPivotBounds.lowerBounds, weaponBarrelXPivotBounds.upperBounds);
				weaponBarrelXPivot.transform.localRotation = Quaternion.Euler(currentRotation);

			}
		}
	}
}
