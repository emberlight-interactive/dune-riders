using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DuneRiders.GatheringSystem;

namespace DuneRiders.PlayerRiderControllerCombination {
	[RequireComponent(typeof(Button))]
	public class InsufficentScrapDisablesButton : MonoBehaviour
	{
		[SerializeField] ResourceManager scrapResourceManager;
		[SerializeField] float requiredScrapToEnable = 0f;
		Button button;

		void Awake() {
			button = GetComponent<Button>();
		}

		void OnEnable() {
			if (scrapResourceManager.Amount() < requiredScrapToEnable) {
				button.interactable = false;
			} else {
				button.interactable = true;
			}
		}
	}
}
