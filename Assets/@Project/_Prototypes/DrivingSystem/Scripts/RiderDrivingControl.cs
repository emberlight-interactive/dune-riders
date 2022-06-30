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
	[SerializeField] InputActionProperty rightControllerB;
	[SerializeField] InputActionProperty rightControllerA;

	[SerializeField] WheelCollider frontLeft;
	[SerializeField] WheelCollider frontRight;
	[SerializeField] WheelCollider backLeft;
	[SerializeField] WheelCollider backRight;

	public float acceleration;
	public float breakingForce;
	public float maxTurnAngle;

	float currentAcceleration = 0f;
	float currentTurnAngle = 15f;

	void Start() {
		rightControllerB.action.Enable();
		rightControllerA.action.Enable();
	}

	private void FixedUpdate() {
		currentAcceleration = acceleration * (rightControllerB.action.ReadValue<float>() - rightControllerA.action.ReadValue<float>());

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
}
