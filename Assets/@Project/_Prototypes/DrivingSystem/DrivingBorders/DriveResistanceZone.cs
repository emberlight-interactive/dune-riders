using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DuneRiders.DrivingSystem {
	public class DriveResistanceZone : MonoBehaviour
	{
		Rigidbody playerRiderRb;
		bool active = false;
		[SerializeField] float borderResistanceMuliplier = 10f;
		[SerializeField] UnityEvent onOutOfBounds;

		void Awake() {
			playerRiderRb = FindObjectOfType<RiderDrivingControl>().GetComponent<Rigidbody>();
		}

		void FixedUpdate() {
			if (active) ApplyResistanceForceToPlayerInBorderZDirection();
		}

		void OnTriggerEnter(Collider c)
		{
			if (!active) {
				if (c.GetComponentInParent<RiderDrivingControl>() != null) {
					active = true;
					onOutOfBounds?.Invoke();
				}
			}
		}

		void OnTriggerExit(Collider c)
		{
			if (active) {
				if (c.GetComponentInParent<RiderDrivingControl>() != null) {
					active = false;
				}
			}
		}

		void ApplyResistanceForceToPlayerInBorderZDirection() {
			float velocityInDirection = Vector3.Dot(playerRiderRb.velocity, transform.forward);

			if (velocityInDirection < 10) {
				playerRiderRb.AddForce(borderResistanceMuliplier * transform.forward.normalized, ForceMode.Acceleration);
			}
		}

		void OnDrawGizmos() {
			Gizmos.color = new Vector4(1, 0, 0, 0.3f);
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawCube(Vector3.zero, Vector3.one);
		}
	}
}
