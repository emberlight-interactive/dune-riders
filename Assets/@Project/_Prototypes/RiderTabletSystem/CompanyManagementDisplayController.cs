using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DuneRiders.RiderTabletSystem {
	public class CompanyManagementDisplayController : MonoBehaviour
	{
		public delegate void DisbandCallback();

		public enum ArmourType {
			Heavy,
			Medium,
			Light,
		};

		public enum WeaponType {
			Cannon,
			TripleMissileLauncher,
			MachineGun,
		};


		[Serializable]
		public struct RiderToDisplay {
			public ArmourType armourType;
			public WeaponType weaponType;
			public int health;
			public DisbandCallback disbandCallback;
			public bool isBeingRepaired;
		}

		[SerializeField] TextMeshProUGUI title;
		[SerializeField] List<RiderDisplayTile> availableTiles = new List<RiderDisplayTile>();
		[SerializeField] Sprite heavyArmourIcon;
		[SerializeField] Sprite mediumArmourIcon;
		[SerializeField] Sprite lightArmourIcon;

		[SerializeField] Sprite cannonIcon;
		[SerializeField] Sprite missileIcon;
		[SerializeField] Sprite bulletIcon;

		public List<RiderToDisplay> ridersToDisplay = new List<RiderToDisplay>(10);

		void OnEnable() {
			StartCoroutine(UpdateDisplay());
		}

		void OnDisable() {
			StopAllCoroutines();
		}

		void DeactivateAllTiles() {
			foreach (var tile in availableTiles) {
				tile.gameObject.SetActive(false);
			}
		}

		IEnumerator UpdateDisplay() {
			while (true) {
				DeactivateAllTiles();
				title.text = $"Company ({ridersToDisplay.Count}/10)";

				var allHeavyUnits = ridersToDisplay.Where((v) => v.armourType == ArmourType.Heavy).ToList();
				var allMediumUnits = ridersToDisplay.Where((v) => v.armourType == ArmourType.Medium).ToList();
				var allLightUnits = ridersToDisplay.Where((v) => v.armourType == ArmourType.Light).ToList();

				allHeavyUnits.Sort((x, y) => x.weaponType.CompareTo(y.weaponType));
				allMediumUnits.Sort((x, y) => x.weaponType.CompareTo(y.weaponType));
				allLightUnits.Sort((x, y) => x.weaponType.CompareTo(y.weaponType));

				var allRiders = new List<RiderToDisplay>();
				allRiders.AddRange(allHeavyUnits);
				allRiders.AddRange(allMediumUnits);
				allRiders.AddRange(allLightUnits);

				int i = 0;
				foreach (var rider in allRiders) {
					availableTiles[i].gameObject.SetActive(true);
					availableTiles[i].disband = rider.disbandCallback;
					availableTiles[i].armourIconography.sprite = GetIconForArmour(rider.armourType);
					availableTiles[i].weaponIconography.sprite = GetIconForWeapon(rider.weaponType);
					availableTiles[i].healthOverlay.fillAmount = HealthToDamageOverlay(rider.health);
					availableTiles[i].repairImage.gameObject.SetActive(rider.isBeingRepaired);
					i++;
				}

				yield return new WaitForSeconds(0.25f);
			}
		}

		Sprite GetIconForArmour(ArmourType armourType) {
			switch (armourType) {
				case ArmourType.Heavy:
					return heavyArmourIcon;
				case ArmourType.Medium:
					return mediumArmourIcon;
				case ArmourType.Light:
					return lightArmourIcon;
				default:
					return lightArmourIcon;
			}
		}

		Sprite GetIconForWeapon(WeaponType weaponType) {
			switch (weaponType) {
				case WeaponType.Cannon:
					return cannonIcon;
				case WeaponType.TripleMissileLauncher:
					return missileIcon;
				case WeaponType.MachineGun:
					return bulletIcon;
				default:
					return bulletIcon;
			}
		}

		float HealthToDamageOverlay(int health) {
			var healtAvailablepercentage = (float) health / 100f;
			return 1f - healtAvailablepercentage;
		}
	}
}
