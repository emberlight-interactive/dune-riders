using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class PlayerLoaded : MonoBehaviour
	{
		public bool loaded = false;

		void OnEnable() {
			loaded = true;
		}
	}
}
