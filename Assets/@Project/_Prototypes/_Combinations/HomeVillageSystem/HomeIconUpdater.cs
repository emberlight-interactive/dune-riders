using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuneRiders.HomeVillageSystem {
	public class HomeIconUpdater : MonoBehaviour
	{
		[SerializeField] Image homeImage;
		[SerializeField] Image homeFillerImage;
		[SerializeField] Animator iconAnimator;
		HomeVillageFuelManager homeVillageFuelManager;

		Color darkRed = new Color(128, 0, 0);

		void OnEnable() {
			homeVillageFuelManager = FindObjectOfType<HomeVillageFuelManager>();
			if (homeVillageFuelManager == null) enabled = false;

			StartCoroutine(UpdateIcon());
		}

		void OnDisable() {
			StopAllCoroutines();
		}

		IEnumerator UpdateIcon() {
			while (true) {
				if (homeVillageFuelManager.GetPercentageOfVillageFuelLeft() < 0.1f) SetIconIntoAlert();
				else SetIconIntoNormal();

				homeFillerImage.fillAmount = homeVillageFuelManager.GetPercentageOfVillageFuelLeft();

				yield return new WaitForSeconds(1f);
			}
		}

		void SetIconIntoAlert() {
			iconAnimator.SetInteger("Type", 2);
			homeImage.color = darkRed;
			homeFillerImage.color = Color.red;
		}

		void SetIconIntoNormal() {
			iconAnimator.SetInteger("Type", 0);
			homeImage.color = Color.grey;
			homeFillerImage.color = Color.white;
		}
	}
}
