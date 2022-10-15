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

	bool rightHandHoldingWheel = false;
	bool leftHandHoldingWheel = false;

	public float acceleration;
	public float breakingForce;
	public float maxTurnAngle;

	float currentAcceleration = 0f;
	float currentTurnAngle = 15f;

	void Start() {
		rightControllerTrigger.action.Enable();
		leftControllerTrigger.action.Enable();
		leftControllerY.action.Enable();
		leftControllerX.action.Enable();
	}

	private void FixedUpdate() {
		currentAcceleration = acceleration * GetAccelerationValue();

		frontLeft.motorTorque = currentAcceleration;
		frontRight.motorTorque = currentAcceleration;

		var velocity = riderRB.velocity;
		var localVel = transform.InverseTransformDirection(velocity);

		if (localVel.z > 10f && currentAcceleration < 0)
		{
			Brake();
		} else if (localVel.z < -10f && currentAcceleration > 0) {
			Brake();
		} else {
			ReleaseBrake();
		}

		currentTurnAngle = maxTurnAngle * steeringWheel.GetValue();
		frontLeft.steerAngle = currentTurnAngle;
		frontRight.steerAngle = currentTurnAngle;
	}

	void Brake() {
		frontLeft.brakeTorque = breakingForce;
		frontRight.brakeTorque = breakingForce;
		backLeft.brakeTorque = breakingForce;
		backRight.brakeTorque = breakingForce;
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
}
