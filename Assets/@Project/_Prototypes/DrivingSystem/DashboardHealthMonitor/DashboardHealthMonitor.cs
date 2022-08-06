using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

		[SerializeField] CLIOutput cliOutput1;
		[SerializeField] CLIOutput cliOutput2;
		[SerializeField] CLIOutput emergencyCliOutput1;
		[SerializeField] CLIOutput emergencyCliOutput2;

		string[] positiveHealthIndications = {
			"[AUT@SYS] INFO: Armo L-H completed successful recursive pen test",
			"[SW@SYS] INFO: NET SYS 1.1 running with acceptable packets recieved",
			"[AUT@SYS] INFO: Hull Sec 3:3 returned health check with positive integer",
			"[TST@SYS] INFO: LIN KERN basic commands returning expected output",
		};

		string[] negativeHealthIndications = {
			"[AUT@SYS] WARNING: Armo R-G was unable to pass minimum integrity threshold",
			"[SW@SYS] WARNING: NET REC 1.21 packet drop rate above alert level",
			"[AUT@SYS] ERROR: Hull Sec 1:2 Major malfunction requiring attention",
			"[TST@SYS] ERROR: KRN HD hashcode mismatch, storage potentially corrupted",
		};

		void Start() {
			UpdateMonitors(_health);
		}

		void UpdateMonitors(int newHealth) {
			InitializeAndProcessCliOutput(cliOutput1, 100);
			InitializeAndProcessCliOutput(cliOutput2, 75);
			InitializeAndProcessCliOutput(emergencyCliOutput1, 50);
			InitializeAndProcessCliOutput(emergencyCliOutput2, 50);
		}

		void InitializeAndProcessCliOutput(CLIOutput cliOutput, int displayThreshold) {
			if (_health < displayThreshold) {
				cliOutput.gameObject.SetActive(true);

				cliOutput.OuputToCliInIntervals(
					CompileCliOutput(_health),
					(0.3f, 1.5f)
				);
			} else {
				cliOutput.ClearText();
				cliOutput.transform.gameObject.SetActive(false);
			}
		}

		string[] CompileCliOutput(int health) {
			var indicatorRatio = GetIndicatorRatio(health);
			var positiveIndicators = positiveHealthIndications.OrderBy(x => Random.Range(0, positiveHealthIndications.Length)).Take(indicatorRatio.positiveIndicators);
			var negativeIndicators = negativeHealthIndications.OrderBy(x => Random.Range(0, negativeHealthIndications.Length)).Take(indicatorRatio.negativeIndicators);
			negativeIndicators = ConvertTextmeshTextRed(negativeIndicators.ToArray());
			return negativeIndicators.Union(positiveIndicators).OrderBy(x => Random.Range(0, indicatorRatio.positiveIndicators + indicatorRatio.negativeIndicators)).ToArray();
		}

		(int positiveIndicators, int negativeIndicators) GetIndicatorRatio(int health, int totalIndicators = 4) {
			float segments = 100 / totalIndicators;
			int confirmedPositiveIndicators = (int) Mathf.Ceil((float) health / (float) segments);
			int confirmedNegativeIndicators = totalIndicators - confirmedPositiveIndicators;

			float supriseNegativeIndicatorChance = (segments - (health % segments)) / (float) segments; // value from 0 to "0.99f"
			supriseNegativeIndicatorChance = supriseNegativeIndicatorChance == 1 ? 0 : supriseNegativeIndicatorChance;
			var includeSupriseNegativeIndicator = (Random.Range(0f, 1f) < supriseNegativeIndicatorChance); // I'm just an idiot
			confirmedPositiveIndicators -= (includeSupriseNegativeIndicator ? 1 : 0);
			confirmedNegativeIndicators += (includeSupriseNegativeIndicator ? 1 : 0);

			return (positiveIndicators: confirmedPositiveIndicators, negativeIndicators: confirmedNegativeIndicators);
		}

		string ConvertTextmeshTextRed(string text) {
			return $"<color=#F86464>{text}</color>";
		}

		string[] ConvertTextmeshTextRed(string[] text) {
			string[] convertedText = new string[text.Length];

			for(int i = 0; i < text.Length; i++) {
				convertedText[i] = ConvertTextmeshTextRed(text[i]);
			}

			return convertedText;
		}
	}
}
