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

	public float acceleration = 2500f;
	public float breakingForce = 4000f;
	public float maxTurnAngle = 20f;

	public float currentAcceleration = 0f;
	public float currentTurnAngle = 15f;

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
}
