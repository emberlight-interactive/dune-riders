using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.GunSystem {
	[RequireComponent(typeof(Rigidbody))]
	public class CannonProjectile : MonoBehaviour
	{
		Rigidbody rb;

		void Awake() {
			rb = GetComponent<Rigidbody>();
		}

		void OnEnable() {
			rb.velocity += transform.forward * 100;
		}

		void OnDisable() {
			rb.velocity = new Vector3(0, 0, 0);
		}
	}
}
