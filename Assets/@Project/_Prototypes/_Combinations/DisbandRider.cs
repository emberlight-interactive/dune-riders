using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;
using DuneRiders.RiderAI.State;

namespace DuneRiders.Combinations {
	[RequireComponent(typeof(Rider))]
	public class DisbandRider : MonoBehaviour
	{
		Rider rider;
		[SerializeField] Rider mercenaryToReplaceRider;

		void Awake() {
			rider = GetComponent<Rider>();
		}

		public void Disband() {
			mercenaryToReplaceRider.chasisType = rider.chasisType;
			mercenaryToReplaceRider.gunType = rider.gunType;
			mercenaryToReplaceRider.GetComponent<IsParkedState>().isParked = false;
			gameObject.SetActive(false);
			// todo: Investigate not pooling riders, so that we do not have to change static propertise like traits
			SimplePool.Spawn(mercenaryToReplaceRider.gameObject, transform.position, transform.rotation);
		}
	}
}
