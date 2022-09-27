using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.MercenaryHiringSystem {
	[Serializable]
	public class Mercenary {
		public Rider.ChasisType chassis;
		public Rider.GunType gunType;
		public int preciousMetalCost = 1;
		public int availabilityValue = 1;
	}

	public class AvailableMercenaryProvider : MonoBehaviour
	{
		[SerializeField] Rider mercenaryPrefab;
		public Rider MercenaryPrefab { get => mercenaryPrefab; }

		[SerializeField] List<Mercenary> availableMercenaries = new List<Mercenary>();
		ProceduralTools proceduralTools;

		void Awake() {
			proceduralTools = new ProceduralTools(transform, true);
		}

		public Mercenary GetMercenaryInformation() {
			var combinedAvailabilityValues = availableMercenaries.Sum((merc) => merc.availabilityValue);
			var randomNumber = proceduralTools.HashToRandInt(proceduralTools.BuildTransformHash(), combinedAvailabilityValues);

			int currentIteratedRange = 0;
			foreach (var mercenary in availableMercenaries) {
				currentIteratedRange += mercenary.availabilityValue;

				if (randomNumber <= currentIteratedRange) return mercenary;
			}

			return availableMercenaries[0];
		}
	}
}
