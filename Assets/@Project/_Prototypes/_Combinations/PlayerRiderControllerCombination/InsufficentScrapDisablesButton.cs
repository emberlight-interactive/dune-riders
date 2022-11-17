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
		[SerializeField] GameObjectDisabledEvent purchaseConfirmationDisabled;
		Button button;

		void Awake() {
			button = GetComponent<Button>();

			purchaseConfirmationDisabled.onDisableEvent.AddListener(() => Invoke(nameof(HandleInsufficentResources), 0.05f));
		}

		void OnEnable() {
			HandleInsufficentResources();
		}

		public void HandleInsufficentResources() {
			if (scrapResourceManager.Amount() < requiredScrapToEnable) {
				button.interactable = false;
			} else {
				button.interactable = true;
			}
		}
	}
}
