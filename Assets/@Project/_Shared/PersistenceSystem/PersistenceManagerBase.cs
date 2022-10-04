using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.Shared.PersistenceSystem {
	public class PersistenceManagerBase : MonoBehaviour
	{
		public IPersistenceUtil persistenceTool { get; protected set; }
		[SerializeField] List<InstancePersister> instancePersisters = new List<InstancePersister>();

		public void SaveGame() {
			SaveInstances();

			IPersistent[] persistentClasses = FindObjectsOfType<MonoBehaviour>().OfType<IPersistent>().ToArray();
			foreach (IPersistent persistentClass in persistentClasses) {
				if (persistentClass.DisablePersistence) continue;
				persistentClass.Save(persistenceTool);
			}
		}

		public void LoadGame() {
			LoadInstances();

			IPersistent[] persistentClasses = FindObjectsOfType<MonoBehaviour>().OfType<IPersistent>().ToArray();
			foreach (IPersistent persistentClass in persistentClasses) {
				if (persistentClass.DisablePersistence) continue;
				persistentClass.Load(persistenceTool);
			}
		}

		void LoadInstances() {
			foreach (var instancePersister in instancePersisters) {
				instancePersister.LoadInstances(persistenceTool);
			}
		}

		void SaveInstances() {
			foreach (var instancePersister in instancePersisters) {
				instancePersister.SaveInstances(persistenceTool);
			}
		}

		public bool saveGame;
		public bool loadGame;

		void OnValidate() {
			if (saveGame) {
				SaveGame();
				saveGame = false;
			} else if (loadGame) {
				LoadGame();
				loadGame = false;
			}
		}
	}
}
