using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders.GatheringSystem {
	public class ResourceManager : MonoBehaviour, IPersistent
	{
		[Serializable]
		class ResourceManagerSerializable {
			public float resourceAmount;
		}

		[SerializeField] Gatherer.SupportedResources resourceType;
		public Gatherer.SupportedResources ResourceType { get => resourceType; }
		public bool DisablePersistence { get => false; }
		[SerializeField] string persistenceKey;

		[SerializeField] float resourceAmount = 0;
		[SerializeField] float resourceLimit = 10000;

		public bool Take(float amount) {
			if (resourceAmount - amount < 0) return false;
			else {
				resourceAmount -= amount;
				return true;
			}
		}

		public bool Give(float amount) {
			if (resourceAmount + amount > resourceLimit) return false;
			else {
				resourceAmount += amount;
				return true;
			}
		}

		public float Amount() { return resourceAmount; }
		public float ResourceLimit() { return resourceLimit; }
		public float RemainingCapacity() { return resourceLimit - resourceAmount; }
		public float RemainingCapacityPercentage() { return resourceAmount / resourceLimit; }

        public void Save(IPersistenceUtil persistUtil) {
			persistUtil.Save<ResourceManagerSerializable>(persistenceKey, new ResourceManagerSerializable {
				resourceAmount = this.resourceAmount,
			});
		}

        public void Load(IPersistenceUtil persistUtil) {
			var loadedResourceManager = persistUtil.Load<ResourceManagerSerializable>(persistenceKey);
			resourceAmount = loadedResourceManager.resourceAmount;
		}
	}
}
