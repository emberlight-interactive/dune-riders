using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

namespace DuneRiders.GunSystem {
	public class ReturnTurretPistolToHolster : MonoBehaviour
	{
		[SerializeField] Grabbable pistol;
		[SerializeField] PlacePoint holster;

		void OnCollisionEnter(Collision collision) {
			if (GameObject.ReferenceEquals(collision.gameObject, pistol.gameObject)) {
				holster.Place(pistol);
			}
		}
	}
}
