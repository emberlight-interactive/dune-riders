using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace DuneRiders.DrivingSystem {
	public class DashboardHealthMonitor : MonoBehaviour
	{
		int _health = 100;
		public int Health {
			get => _health;
			set {
				var health = value >= 100 ? 100 : value;
				health = health <= 0 ? 0 : health;
				_health = health;

				UpdateMonitors(_health);
			}
		}

		[SerializeField] Image cliImage;

		void Start() {
			UpdateMonitors(_health);
		}

		void UpdateMonitors(int newHealth) {
			if (newHealth > 80) {
				cliImage.fillAmount = 0f;
				return;
			} else if (newHealth > 60) {
				cliImage.fillAmount = 0.2f;
				return;
			} else if (newHealth > 40) {
				cliImage.fillAmount = 0.4f;
				return;
			} else if (newHealth > 20) {
				cliImage.fillAmount = 0.6f;
				return;
			} else if (newHealth > 0) {
				cliImage.fillAmount = 0.8f;
				return;
			} else if (newHealth <= 0) {
				cliImage.fillAmount = 1f;
				return;
			}
		}
	}
}
