using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DuneRiders.GatheringSystem;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class ResourceSliderUpdater : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI amountAndCapacityText;
		[SerializeField] TextMeshProUGUI burnRateText;
		[SerializeField] string burnRateUnits = "L/h";
		[SerializeField] BurnRateSystem burnRateSystem;
		[SerializeField] bool oneTimeBurnRateResource;
		[SerializeField] Gatherer.SupportedResources resourceTracked;
		[SerializeField] Gatherer gatherer;
		[SerializeField] Slider resourceSlider;

		void OnEnable() {
			StartCoroutine(UpdateSlider());
		}

		void OnDisable() {
			StopAllCoroutines();
		}

		IEnumerator UpdateSlider() {
			while (true) {
				resourceSlider.value = GetTotalResourcesPercentage();
				amountAndCapacityText.text = GetAmountAndCapacityFraction();

				if (oneTimeBurnRateResource) {
					var oneTimeBurnRateAmount = (int) burnRateSystem.GetOneTimeResourceCost(GetBurnRateResourceTypeEquivalentEnum(resourceTracked));

					if (oneTimeBurnRateAmount > 0) burnRateText.text = $"-{oneTimeBurnRateAmount} {burnRateUnits}";
					else burnRateText.text = "";
				} else {
					var burnRate = burnRateSystem.GetResourceBurnRate(GetBurnRateResourceTypeEquivalentEnum(resourceTracked));
					if (burnRate > 0) burnRateText.text = GetBurnRateString(burnRate);
					else burnRateText.text = "";
				}

				yield return new WaitForSeconds(0.25f);
			}
		}

		float GetTotalResourcesPercentage() {
			return (float) gatherer.GetManager(resourceTracked).Amount() / (float) gatherer.GetManager(resourceTracked).ResourceLimit();
		}

		string GetAmountAndCapacityFraction() {
			return $"<b>{gatherer.GetManager(resourceTracked).Amount()}</b>/{gatherer.GetManager(resourceTracked).ResourceLimit()}";
		}

		string GetBurnRateString(float burnRate) {
			return $"-{burnRate} {burnRateUnits}";
		}

		BurnRateSystem.ResourceType GetBurnRateResourceTypeEquivalentEnum(Gatherer.SupportedResources gathererResource) {
			switch (gathererResource) {
				case Gatherer.SupportedResources.Fuel:
					return BurnRateSystem.ResourceType.Fuel;
				case Gatherer.SupportedResources.ScrapMetal:
					return BurnRateSystem.ResourceType.ScrapMetal;
				case Gatherer.SupportedResources.PreciousMetal:
					return BurnRateSystem.ResourceType.PreciousMetal;
				default:
					throw new NotImplementedException();
			}
		}
	}
}
