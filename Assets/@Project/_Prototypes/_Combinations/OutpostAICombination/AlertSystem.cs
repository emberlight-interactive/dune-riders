using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DuneRiders.AI;

namespace DuneRiders.OutpostAICombination {
	[RequireComponent(typeof(RidersInRange))]
	public class AlertSystem : MonoBehaviour
	{
		class CountdownManager {
			public float timerMaxSeconds;
			public float remainingSeconds;
			public bool paused;

			public void ResetCountdown() {
				remainingSeconds = timerMaxSeconds;
			}

			public void PauseCountdown() {
				paused = true;
			}

			public void ResumeCountdown() {
				paused = false;
			}
		}

		// EnemyRiderAISpawner enemyRiderAISpawner;

		CountdownManager countdownManager;
		RidersInRange ridersInRange;

		[SerializeField] TextMeshProUGUI countdownTimer;
		[SerializeField] float secondsUntilDefendersSpawn = 240;
		[SerializeField] float secondsToTimerResetAfterAbsentRiders = 120;
		[SerializeField] Allegiance enemyAllegiance = Allegiance.Player;

		void Awake() {
			ridersInRange = GetComponent<RidersInRange>();
			countdownManager = new CountdownManager() {
				timerMaxSeconds = secondsUntilDefendersSpawn,
				remainingSeconds = secondsUntilDefendersSpawn,
				paused = true,
			};
		}

		void OnEnable() {
			UpdateTimerDisplay();
			StartCoroutine(LocalEnemiesAffectTimerPauseAndReset());
			StartCoroutine(DefenseCountdown());
		}

		void OnDisable() {
			StopAllCoroutines();
		}

		IEnumerator DefenseCountdown() {
			while (true) {
				if (!countdownManager.paused && countdownManager.remainingSeconds > 0) {
					if (countdownManager.remainingSeconds <= 0) {
						countdownManager.remainingSeconds = 0;
						UpdateTimerDisplay();

						// Call defense
					}

					UpdateTimerDisplay();
					countdownManager.remainingSeconds -= Time.deltaTime;
				}

				yield return null;
			}
		}

		IEnumerator LocalEnemiesAffectTimerPauseAndReset() {
			float secondsUntilDefenseTimerReset = secondsToTimerResetAfterAbsentRiders;
			float remainingSeconds = secondsUntilDefenseTimerReset;
			float detectionIntervals = 1f;

			while (true) {
				yield return new WaitForSeconds(detectionIntervals);

				if (ridersInRange.AreAnyRidersInRange(enemyAllegiance)) {
					countdownManager.ResumeCountdown();
					remainingSeconds = secondsUntilDefenseTimerReset;
				} else {
					countdownManager.PauseCountdown();

					if (remainingSeconds <= 0) {
						countdownManager.ResetCountdown();
						UpdateTimerDisplay();
					} else {
						remainingSeconds -= detectionIntervals;
					}
				}
			}
		}

		void UpdateTimerDisplay() {
			countdownTimer.text = SecondsToTimerString(countdownManager.remainingSeconds);
		}

		string SecondsToTimerString(float totalSeconds) {
			var displayedMinutes = SecondsToTimerMinutesString(totalSeconds);
			var displayedSeconds = SecondsToTimerSecondsString(totalSeconds);

			return $"{displayedMinutes}:{displayedSeconds}";
		}

		string SecondsToTimerMinutesString(float totalSeconds) {
			var minutes = Mathf.FloorToInt(totalSeconds / 60);
			if (minutes > 0) { return minutes.ToString(); }
			return "0";
		}

		string SecondsToTimerSecondsString(float totalSeconds) {
			var seconds = Mathf.FloorToInt(totalSeconds % 60);
			if (seconds >= 10) { return seconds.ToString(); }
			if (seconds < 10 && seconds > 0) { return $"0{seconds}"; }
			return "00";
		}
	}
}
