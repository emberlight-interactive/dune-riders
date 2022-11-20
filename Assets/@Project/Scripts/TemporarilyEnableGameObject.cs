using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders {
	public class TemporarilyEnableGameObject : MonoBehaviour
	{
		public float timeEnabled = 4f;

		public void TemporarilyEnable() {
			gameObject.SetActive(true);
			Invoke(nameof(DisableMyself), timeEnabled);
		}

		void DisableMyself() {
			gameObject.SetActive(false);
		}
	}
}
