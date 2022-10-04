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
			public int resourceAmount;
		}

		[SerializeField] Gatherer.SupportedResources resourceType;
		public Gatherer.SupportedResources ResourceType { get => resourceType; }
		public bool DisablePersistence { get => false; }
		[SerializeField] string persistenceKey;

		[SerializeField] int resourceAmount = 0;
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
		public int RemainingCapacity() { return resourceLimit - resourceAmount; }

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
