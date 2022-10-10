using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DuneRiders.PersistenceSystemCombination;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class PauseMenuActions : MonoBehaviour
	{
		[SerializeField] AutoSaver autoSaver;

		public bool savingEnabled = true;

		public void SaveAndQuit() {
			if (savingEnabled) autoSaver.TriggerSaveGame();

			#if UNITY_EDITOR
				Debug.Log("Application Exited");
			#else
				Application.Quit();
			#endif
		}
	}
}
