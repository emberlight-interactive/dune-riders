using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DuneRiders.RiderTabletSystem {
	public class DisbandConfirmation : MonoBehaviour
	{
		[SerializeField] Button yesButton;
		[SerializeField] Button noButton;

		void Awake() {
			gameObject.SetActive(false);
			noButton.onClick.AddListener(() => gameObject.SetActive(false));
		}

		public void ConfirmDisbanding(CompanyManagementDisplayController.DisbandCallback disbandCallback, GameObject initiatingTile) {
			yesButton.onClick.AddListener(() => {
				disbandCallback();
				initiatingTile.SetActive(false);
				gameObject.SetActive(false);
			});
		}
	}
}
