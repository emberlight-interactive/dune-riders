using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders.PersistenceSystemCombination {
	public class PersistenceTool : IPersistenceUtilInternal {
		bool saveFileExists = false;
		static List<(string key, object data)> primedObjectsForPersistence = new List<(string, object)>();

		struct SaveAsyncJob : IJob
		{

			public void Execute()
			{
				foreach (var primedObject in PersistenceTool.primedObjectsForPersistence) {
					ES3.Save(primedObject.key, primedObject.data);
				}
			}
		}

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

		public void PrimeObjectForAsyncSave<T>(string key, T data) {
			primedObjectsForPersistence.Add((key, data));
		}

		public JobHandle SaveAsync() {
			var saveAsyncJob = new SaveAsyncJob();
			return saveAsyncJob.Schedule();
		}

		public void ClearObjectsPrimedForAsyncSave() {
			primedObjectsForPersistence.Clear();
		}
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
