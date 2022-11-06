using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DuneRiders.PersistenceSystemCombination;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class PauseMenuActions : MonoBehaviour
	{
		[SerializeField] PersistenceManager persistenceManager;
		[SerializeField] SceneTransitioner mainMenuTransitioner;

		public bool savingEnabled = true;

		public void SaveAndQuit() {
			if (savingEnabled) persistenceManager.SaveGame();

			#if UNITY_EDITOR
				Debug.Log("Application Exited");
			#else
				Application.Quit();
			#endif
		}

		public void SaveAndMainMenu() {
			if (savingEnabled) persistenceManager.SaveGame();
			Time.timeScale = 1.0f;
			mainMenuTransitioner.LoadNextScene();
		}
	}
}
