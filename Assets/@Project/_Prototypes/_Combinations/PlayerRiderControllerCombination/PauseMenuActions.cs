using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DuneRiders.PersistenceSystemCombination;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class PauseMenuActions : MonoBehaviour
	{
		[SerializeField] PersistenceManager persistenceManager;

		public bool savingEnabled = true;

		public void SaveAndQuit() {
			if (savingEnabled) persistenceManager.SaveGame();

			#if UNITY_EDITOR
				Debug.Log("Application Exited");
			#else
				Application.Quit();
			#endif
		}
	}
}
