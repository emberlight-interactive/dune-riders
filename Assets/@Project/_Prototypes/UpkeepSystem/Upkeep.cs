using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.UpkeeperSystem {
	public class Upkeep : MonoBehaviour
	{
		[SerializeField] UpkeepTracker.UpkeepType upkeepType;
		public UpkeepTracker.UpkeepType UpkeepType { get => upkeepType; }

		[SerializeField] float rate;
		public float Rate { get => rate; }

		UpkeepTracker upkeepTracker;

		void OnEnable() {
			if (upkeepTracker == null) {
				upkeepTracker = FindObjectOfType<UpkeepTracker>();
				if (upkeepTracker != null) upkeepTracker.Add(this);
			} else upkeepTracker.Add(this);
		}

		void OnDisable() {
			if (upkeepTracker == null) {
				upkeepTracker = FindObjectOfType<UpkeepTracker>();
				if (upkeepTracker != null) upkeepTracker.Remove(this);
			} else upkeepTracker.Remove(this);

		}
	}
}
