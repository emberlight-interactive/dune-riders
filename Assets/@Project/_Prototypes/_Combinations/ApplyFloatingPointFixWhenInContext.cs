using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gaia;

namespace DuneRiders.Combinations {
	public class ApplyFloatingPointFixWhenInContext : MonoBehaviour
	{
		void Awake() {
			var sessionManager = GaiaSessionManager.GetSessionManager(false, false);
			if (sessionManager != null) {
				gameObject.AddComponent<FloatingPointFixMember>();
			}
		}
	}
}
