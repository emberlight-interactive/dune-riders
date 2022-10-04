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

	public class PersistenceManager : PersistenceManagerBase
	{
		[SerializeField] bool loadOnAwake = false;

		void Awake() {
			persistenceTool = new PersistenceTool();
			if (loadOnAwake) LoadGame();
		}
	}
}
