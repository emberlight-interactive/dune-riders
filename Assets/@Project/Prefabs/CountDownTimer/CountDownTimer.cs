using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace DuneRiders {
	public class CountDownTimer : MonoBehaviour
	{
		[SerializeField] float currentTime = 10f;
		[SerializeField] TextMeshProUGUI timerDisplay;
		[SerializeField] UnityEvent onTimerFinished = new UnityEvent();
		bool timerFinishedEventCalled = false;

		void Update() {
			var currentTimeRoundedUp = Mathf.Ceil(currentTime);

			if (currentTimeRoundedUp > 0) {
				timerDisplay.text = currentTimeRoundedUp.ToString();
			} else {
				if (!timerFinishedEventCalled) {
					onTimerFinished.Invoke();
					timerFinishedEventCalled = true;
				}

				timerDisplay.text = "0";
			}

			currentTime -= Time.deltaTime;
		}
	}
}
