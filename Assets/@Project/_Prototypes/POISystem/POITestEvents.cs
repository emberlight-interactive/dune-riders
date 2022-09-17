using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.POISystem {
	public class POITestEvents : MonoBehaviour
	{
		public void ExhaustedPOI() {
			Debug.Log("All gone");
		}

		public void TouchedPOI() {
			Debug.Log("Touched lol");
		}
	}
}
