using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.RiderAI.State {
	public class HealthStateUpdater : MonoBehaviour
	{
		[SerializeField] HealthState healthState;

		void OnCollisionEnter(Collision collision)
		{
			healthState.health -= 50;
		}
	}
}
