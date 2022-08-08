using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.GatheringSystem {
	public class ResourceManager : MonoBehaviour
	{
		[SerializeField] Gatherer.SupportedResources resourceType;
		public Gatherer.SupportedResources ResourceType { get => resourceType; }

		[SerializeField, ReadOnly] int resourceAmount = 0;
		[SerializeField] int resourceLimit = 10000;

		public bool Take(int amount) {
			if (resourceAmount - amount < 0) return false;
			else {
				resourceAmount -= amount;
				return true;
			}
		}

		public bool Give(int amount) {
			if (resourceAmount + amount > resourceLimit) return false;
			else {
				resourceAmount += amount;
				return true;
			}
		}

		public int Amount() { return resourceAmount; }
		public int ResourceLimit() { return resourceLimit; }
	}
}
