using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace DuneRiders.DrivingSystem {
	public class DashboardHealthMonitor : MonoBehaviour
	{
		[SerializeField] Image cliImage;
		[SerializeField] Image pulseImage;

		public void UpdateMonitors(float healthPercentage) {
			PulseMonitor();

			var healthPercentageRelativeToOneHundred = 100 * healthPercentage;
			if (healthPercentageRelativeToOneHundred == 100) {
				cliImage.fillAmount = 0f;
				return;
			} else if (healthPercentageRelativeToOneHundred > 80) {
				cliImage.fillAmount = 0.2f;
				return;
			} else if (healthPercentageRelativeToOneHundred > 60) {
				cliImage.fillAmount = 0.4f;
				return;
			} else if (healthPercentageRelativeToOneHundred > 40) {
				cliImage.fillAmount = 0.6f;
				return;
			} else if (healthPercentageRelativeToOneHundred > 20) {
				cliImage.fillAmount = 0.8f;
				return;
			} else if (healthPercentageRelativeToOneHundred <= 0) {
				cliImage.fillAmount = 1f;
				return;
			}
		}

		void PulseMonitor() {
			StopAllCoroutines();
			StartCoroutine(DamagePulse());
		}

		IEnumerator DamagePulse() {
			pulseImage.enabled = true;
			yield return new WaitForSeconds(0.1f);
			pulseImage.enabled = false;
		}
	}
}
