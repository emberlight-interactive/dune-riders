using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DuneRiders.GatheringSystem;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class ResourceManagerDepleter : MonoBehaviour
	{
		[SerializeField] BurnRateSystem burnRateSystem;
		[SerializeField] BurnRateSystem.ResourceType resourceTypeForDepletionRate;
		[SerializeField] ResourceManager resourceManager;
		[SerializeField] UnityEvent resourceFullyDepletedEvent = new UnityEvent();

		void OnEnable() {
			StartCoroutine(ResourceConsumption());
		}

		void OnDisable() {
			StopAllCoroutines();
		}

		IEnumerator ResourceConsumption() {
			while (true) {
				yield return new WaitForSeconds(10f); // todo: Could exit and reload rapidly to cheat?

				var resourcesPerMinute = burnRateSystem.GetResourceBurnRate(resourceTypeForDepletionRate) / 360;
				if (!resourceManager.Take(resourcesPerMinute)) {
					resourceFullyDepletedEvent.Invoke();
				}
			}
		}
	}
}
