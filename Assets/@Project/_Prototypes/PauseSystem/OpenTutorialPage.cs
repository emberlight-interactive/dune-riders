using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuneRiders.PauseSystem {
	[RequireComponent(typeof(Pause))]
	public class OpenTutorialPage : MonoBehaviour
	{
		Pause pause;
		[SerializeField] Canvas tutorialCanvas;
		[SerializeField] Button tutorialButton;
		[SerializeField] Button firingTutorialButton;
		[SerializeField] Button drivingTutorialButton;

		void Awake() {
			pause = GetComponent<Pause>();
		}

		public void OpenGunSwapTutorial() {
			OpenTutorial(firingTutorialButton);
		}

		public void OpenDrivingTutorial() {
			OpenTutorial(drivingTutorialButton);
		}

		void OpenTutorial(Button pageButton) {
			pause.PauseGame();

			if (!tutorialCanvas.enabled) tutorialButton.onClick.Invoke();
			pageButton.onClick.Invoke();
		}
	}
}
