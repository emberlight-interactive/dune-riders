using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DuneRiders.GatheringSystem;

namespace DuneRiders.PlayerRiderControllerCombination {
	[RequireComponent(typeof(Button))]
	public class UpgradePurchaser : MonoBehaviour
	{
		[SerializeField] Button confirmButton;
		[SerializeField] ResourceManager scrapResourceManager;
		[SerializeField] PlayerTurretUpgrader playerTurretUpgrader;
		[SerializeField] PlayerTurretUpgrader.UpgradeType upgradeType;
		[SerializeField] float scrapForPurchase;
		Button purchaseButton;

		void Awake() {
			purchaseButton = GetComponent<Button>();
		}

		public void RegisterPurchaserOnConfirmButton() {
			confirmButton.onClick.RemoveAllListeners();
			confirmButton.onClick.AddListener(ProcessPurchase);
		}

		public void ProcessPurchase() {
			if (scrapResourceManager.Take(scrapForPurchase)) {
				playerTurretUpgrader.ApplyUpgrade(upgradeType);
				DisablePurchaseButton();
			}
		}

		public void DisablePurchaseButton() {
			purchaseButton.gameObject.SetActive(false);
		}
	}
}
