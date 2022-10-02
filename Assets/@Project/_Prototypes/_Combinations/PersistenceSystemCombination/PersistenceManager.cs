using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders.PersistenceSystemCombination {
	public class PersistenceTool : IPersistenceUtil {
			public void Save<T>(string key, T data) {
				ES3.Save<T>(key, data);
			}

			public T Load<T>(string key) {
				return ES3.Load<T>(key);
			}

			public void Delete(string key) {
				ES3.DeleteKey(key);
			}
		}

	public class PersistenceManager : MonoBehaviour
	{
		public PersistenceTool persistenceTool { get; private set; }
		[SerializeField] List<InstancePersister> instancePersisters = new List<InstancePersister>();
		[SerializeField] bool loadOnAwake = false;

		void Awake() {
			persistenceTool = new PersistenceTool();
			if (loadOnAwake) LoadGame();
		}

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
