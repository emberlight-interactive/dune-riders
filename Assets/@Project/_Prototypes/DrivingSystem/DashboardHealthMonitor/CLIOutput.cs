using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DuneRiders.DrivingSystem {
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class CLIOutput : MonoBehaviour
	{
		static int cliLineCapacity = 6;
		TextMeshProUGUI cliOutput;
		Queue<string> cliOutputLines = new Queue<string>(cliLineCapacity);
		Coroutine cliOutputter;

		void Awake() {
			cliOutput = GetComponent<TextMeshProUGUI>();
		}

		public void OuputToCliInIntervals(string[] cliLinesToOutput, (float lowerInterval, float upperInterval) interval) {
			if (cliOutputter != null) StopCoroutine(cliOutputter);
			cliOutputter = StartCoroutine(CliOutputter(cliLinesToOutput, interval));
		}

		IEnumerator CliOutputter(string[] cliLinesToOutput, (float lowerInterval, float upperInterval) interval) {
			for (int i = 0; i < cliLinesToOutput.Length; i++) {
				OuputToCli(cliLinesToOutput[i]);
				yield return new WaitForSeconds(Random.Range(interval.lowerInterval, interval.upperInterval));
			}
		}

		public void OuputToCli(string line) {
			string compiledOutput = "";

			if (cliOutputLines.Count == cliLineCapacity) {
				cliOutputLines.Dequeue();
			}

			cliOutputLines.Enqueue(line);
			foreach (string cliLine in cliOutputLines.ToArray()) {
				compiledOutput += cliLine + "\n";
			}

			cliOutput.text = compiledOutput;
		}

		public void ClearText() {
			cliOutputLines.Clear();
			cliOutput.text = "";
		}
	}
}
