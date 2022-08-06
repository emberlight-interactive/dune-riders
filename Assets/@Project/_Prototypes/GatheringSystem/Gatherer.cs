using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace DuneRiders.GatheringSystem {
	[RequireComponent(typeof(Rigidbody))]
	public class Gatherer : MonoBehaviour
	{
		public enum SupportedResources {
			Fuel,
			ScrapMetal,
			PreciousMetal,
		}

		[SerializeField] ResourceManager[] resourceManagers;

		public ResourceManager GetManager(SupportedResources resourceType) {
			if (resourceManagers == null) return null;
			return resourceManagers.SingleOrDefault(item => item.ResourceType == resourceType);
		}
	}
}
