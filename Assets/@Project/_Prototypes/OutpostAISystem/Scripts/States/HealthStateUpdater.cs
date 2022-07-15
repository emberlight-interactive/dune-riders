using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.OutpostAI.State {
	public class HealthStateUpdater : MonoBehaviour
	{
		[SerializeField] HealthState healthState;

		void OnCollisionEnter(Collision collision)
		{
			healthState.health -= 50;
		}
	}
}

