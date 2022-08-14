using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;
using DuneRiders.AI;

namespace DuneRiders.Combinations {
	[RequireComponent(typeof(Rider))]
	public class DisbandRider : MonoBehaviour
	{
		public void Disband() {
			GetComponent<Rider>().allegiance = Allegiance.Mercenary;
		}
	}
}
