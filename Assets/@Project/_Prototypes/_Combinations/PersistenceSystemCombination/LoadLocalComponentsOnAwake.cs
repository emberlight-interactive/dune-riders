using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders.PersistenceSystemCombination {
	public class LoadLocalComponentsOnAwake : MonoBehaviour {
		void Awake() {
			var persistenceManager = FindObjectOfType<PersistenceManager>();
			if (persistenceManager != null) {
				var persistenceTool = persistenceManager.persistenceTool;
				IPersistent[] persistentClasses = GetComponentsInChildren<MonoBehaviour>().OfType<IPersistent>().ToArray();
				foreach (IPersistent persistentClass in persistentClasses) {
					if (persistentClass.DisablePersistence) continue;
					persistentClass.Load(persistenceTool);
				}
			}

		}
	}
}
