using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;
using UnityEngine.InputSystem;

public class RiderDrivingControl : MonoBehaviour
{
	[SerializeField] PhysicsGadgetHingeAngleReader steeringWheel;
	[SerializeField] InputActionProperty rightControllerB;
	[SerializeField] InputActionProperty rightControllerA;

	[SerializeField] WheelCollider frontLeft;
	[SerializeField] WheelCollider frontRight;
	[SerializeField] WheelCollider backLeft;
	[SerializeField] WheelCollider backRight;

	public float acceleration = 500f;
	public float breakingForce = 300f;
	public float maxTurnAngle = 15f;

	public float currentAcceleration = 0f;
	public float currentBreakForce = 0f;
	public float currentTurnAngle = 15f;

	void Start() {
		rightControllerB.action.Enable();
	}

	private void FixedUpdate() {
		currentAcceleration = acceleration * (rightControllerB.action.ReadValue<float>() - rightControllerA.action.ReadValue<float>());

		currentBreakForce = 0; // Add conditional

		frontLeft.motorTorque = currentAcceleration;
		frontRight.motorTorque = currentAcceleration;

		frontLeft.brakeTorque = currentBreakForce;
		frontRight.brakeTorque = currentBreakForce;
		backLeft.brakeTorque = currentBreakForce;
		backRight.brakeTorque = currentBreakForce;

		currentTurnAngle = maxTurnAngle * steeringWheel.GetValue();
		frontLeft.steerAngle = currentTurnAngle;
		frontRight.steerAngle = currentTurnAngle;
	}
}
