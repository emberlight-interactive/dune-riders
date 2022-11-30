using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;
using UnityEngine.InputSystem;

public class RiderDrivingControl : MonoBehaviour
{
	[SerializeField] PhysicsGadgetHingeAngleReader steeringWheel;
	[SerializeField] Rigidbody riderRB;
	[SerializeField] InputActionProperty rightControllerTrigger;
	[SerializeField] InputActionProperty leftControllerTrigger;
	[SerializeField] InputActionProperty leftControllerY;
	[SerializeField] InputActionProperty leftControllerX;

	[SerializeField] WheelCollider frontLeft;
	[SerializeField] WheelCollider frontRight;
	[SerializeField] WheelCollider backLeft;
	[SerializeField] WheelCollider backRight;

	bool persistentBrakeSinceForwardAcceleration = false;

	bool rightHandHoldingWheel = false;
	bool leftHandHoldingWheel = false;

	public float acceleration;
	public float breakingForce;
	public float maxTurnAngle;
	public float speedEffectOnTurnAngle = 0f;

	float currentAcceleration = 0f;
	float currentTurnAngle = 15f;

	bool accelerating = false;
	public bool Accelerating { get => accelerating; }

	bool reversing = false;
	public bool Reversing { get => reversing; }

	void Start() {
		rightControllerTrigger.action.Enable();
		leftControllerTrigger.action.Enable();
		leftControllerY.action.Enable();
		leftControllerX.action.Enable();
	}

	void FixedUpdate() {
		currentAcceleration = acceleration * GetAccelerationValue();

		DetectPersistentBrakeSinceForwardAcceleration();
		UpdateDrivingState(currentAcceleration);

		SetWheelTorque(currentAcceleration);

		var localVel = LocalVelocity();

		if (persistentBrakeSinceForwardAcceleration && currentAcceleration < 0)
		{
			if (GetRiderKMH() > 35f) {
				Brake(GetRiderKMH() / 100f);
			} else if (GetRiderKMH() <= 35f && GetRiderKMH() >= 5f) {
				ReleaseBrake();
			} else if (GetRiderKMH() < 5f) {
				Brake();
				SetWheelTorque(0);
			}
		} else if (localVel.z < -4f && currentAcceleration > 0) {
			Brake();
		} else {
			ReleaseBrake();
		}

		currentTurnAngle = GetTurnAngle() * steeringWheel.GetValue();
		frontLeft.steerAngle = currentTurnAngle;
		frontRight.steerAngle = currentTurnAngle;
	}

	void Brake(float multiplier = 1f) {
		frontLeft.brakeTorque = breakingForce * multiplier;
		frontRight.brakeTorque = breakingForce * multiplier;
		backLeft.brakeTorque = breakingForce * multiplier;
		backRight.brakeTorque = breakingForce * multiplier;
	}

	void ReleaseBrake() {
		frontLeft.brakeTorque = 0;
		frontRight.brakeTorque = 0;
		backLeft.brakeTorque = 0;
		backRight.brakeTorque = 0;
	}

	float GetForwardAccelerationValue() {
		var leftTriggerValue = leftHandHoldingWheel ? leftControllerTrigger.action.ReadValue<float>() : 0;
		var rightTriggerValue = rightHandHoldingWheel ? rightControllerTrigger.action.ReadValue<float>() : 0;
		var accelerationValue = rightTriggerValue + leftTriggerValue + leftControllerY.action.ReadValue<float>();
		if (accelerationValue > 1) return 1f;
		return accelerationValue;
	}

	float GetAccelerationValue() {
		var brakeValue = leftControllerX.action.ReadValue<float>();
		if (brakeValue > 0.15f) return -brakeValue;
		return GetForwardAccelerationValue();
	}

	public void WheelGrabbed(Hand hand, Grabbable grabbable) {
		if (hand.left) leftHandHoldingWheel = true;
		else rightHandHoldingWheel = true;
	}

	public void WheelReleased(Hand hand, Grabbable grabbable) {
		if (hand.left) leftHandHoldingWheel = false;
		else rightHandHoldingWheel = false;
	}

	Vector3 LocalVelocity() {
		return transform.InverseTransformDirection(riderRB.velocity);
	}

	float GetTurnAngle() {
		return Mathf.Max(maxTurnAngle - Mathf.Abs(speedEffectOnTurnAngle * LocalVelocity().z), 3);
	}

	void UpdateDrivingState(float currentAcceleration) {
		if (currentAcceleration > 0.15f) {
			accelerating = true;
			reversing = false;
		} else if (currentAcceleration < 0.15f && currentAcceleration > -0.15f) {
			accelerating = false;
			reversing = false;
		} else {
			accelerating = false;
			reversing = true;
		}
	}

	void SetWheelTorque(float acceleration) {
		frontLeft.motorTorque = acceleration;
		frontRight.motorTorque = acceleration;
	}

	void DetectPersistentBrakeSinceForwardAcceleration() {
		if (GetAccelerationValue() < 0 && LocalVelocity().z >= 1f) persistentBrakeSinceForwardAcceleration = true;
		else if (GetAccelerationValue() >= 0 && LocalVelocity().z < 1f && persistentBrakeSinceForwardAcceleration) persistentBrakeSinceForwardAcceleration = false;
	}

	float GetRiderKMH() {
		return 3.6f * riderRB.velocity.magnitude;
	}
}
