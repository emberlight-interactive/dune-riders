using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.PlayerRiderControllerCombination;

namespace DuneRiders.Combinations {
	public class GameOver : MonoBehaviour
	{
		[SerializeField] SceneTransitioner failSceneTransistioner;
		[SerializeField] PauseMenuActions pauseMenuActions;
		[SerializeField] float gameOverDelay = 4f;

		public void DelayedGameOver() {
			pauseMenuActions.savingEnabled = false;
			Invoke(nameof(TriggerGameOver), gameOverDelay);
		}

		public void TriggerGameOver() {
			pauseMenuActions.savingEnabled = false;
			failSceneTransistioner.LoadNextScene();
		}
	}
}
