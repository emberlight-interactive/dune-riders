using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DuneRiders.RiderTabletSystem {
	[RequireComponent(typeof(Button))]
	public class RiderDisplayTile : MonoBehaviour
	{
		public CompanyManagementDisplayController.DisbandCallback disband;
		public Image healthOverlay;
		public Image armourIconography;
		public Image weaponIconography;
		public Image repairImage;
		public TextMeshProUGUI healthText;

		DisbandConfirmation disbandConfirmation;
		Button hostButton;

		void Awake() {
			hostButton = GetComponent<Button>();
			disbandConfirmation = FindObjectOfType<DisbandConfirmation>(true);

			hostButton.onClick.AddListener(() => {
				disbandConfirmation.gameObject.SetActive(true);
				disbandConfirmation.ConfirmDisbanding(disband, gameObject);
			});
		}


	}
}
