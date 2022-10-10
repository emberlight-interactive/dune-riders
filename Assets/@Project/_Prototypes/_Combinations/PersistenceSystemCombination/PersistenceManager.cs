using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders.PersistenceSystemCombination {
	public class PersistenceTool : IPersistenceUtil {
		bool saveFileExists = false;

		public PersistenceTool() {
			var persistenceFileName = UnityEngine.Object.FindObjectOfType<GameManager>().persistenceFileName;
			if (ES3.FileExists(persistenceFileName)) {
				saveFileExists = true;
			}

			ES3Settings.defaultSettings.path = persistenceFileName;
		}

		public void Save<T>(string key, T data) {
			ES3.Save<T>(key, data);
		}

		public T Load<T>(string key) {
			return ES3.Load<T>(key);
		}

		public void Delete(string key) {
			ES3.DeleteKey(key);
		}

		public bool SaveFileExists() { return saveFileExists; }
	}

	[DefaultExecutionOrder(-100)]
	public class PersistenceManager : PersistenceManagerBase
	{
		[SerializeField] bool loadOnAwake = false;

		void Awake() {
			persistenceTool = new PersistenceTool();
			if (loadOnAwake) LoadGame();
		}
	}
}
