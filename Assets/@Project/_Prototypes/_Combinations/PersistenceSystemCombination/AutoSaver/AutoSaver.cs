using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.PersistenceSystemCombination {
	[RequireComponent(typeof(PersistenceManager))]
	public class AutoSaver : MonoBehaviour
	{
		[SerializeField] float autoSaveCycleTime = 15 * 60;
		PersistenceManager persistenceManager;

		void Awake() {
			persistenceManager = GetComponent<PersistenceManager>();
		}

		void OnEnable() {
			StartCoroutine(AutoSaveLoop());
		}

		void OnDisable() {
			StopAllCoroutines();
		}

		public void TriggerSaveGame() {
			persistenceManager.SaveGame();
			RestartAutoSaverLoop();
		}

		IEnumerator AutoSaveLoop() {
			while (true) {
				yield return new WaitForSeconds(autoSaveCycleTime);
				TriggerSaveGame();
			}
		}

		void RestartAutoSaverLoop() {
			StopAllCoroutines();
			StartCoroutine(AutoSaveLoop());
		}
	}
}
