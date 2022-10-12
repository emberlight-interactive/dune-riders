using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

namespace DuneRiders.Shared.PersistenceSystem {
	public class PersistenceManagerBase : MonoBehaviour
	{
		class AsyncPersistenceUtil : IPersistenceUtil {
			IPersistenceUtilInternal persistenceUtil;

			public AsyncPersistenceUtil(IPersistenceUtilInternal persistenceUtil) {
				this.persistenceUtil = persistenceUtil;
			}

			public void Save<T>(string key, T data) {
				this.persistenceUtil.PrimeObjectForAsyncSave(key, data);
			}

			public T Load<T>(string key) { return default(T); }
		}

		public delegate void TaskFinishedCallback();

		public IPersistenceUtilInternal persistenceTool { get; protected set; }
		IPersistenceUtil _asyncPersistenceTool;
		IPersistenceUtil asyncPersistenceTool { get => _asyncPersistenceTool ?? (_asyncPersistenceTool = new AsyncPersistenceUtil(persistenceTool)); }
		[SerializeField] List<InstancePersister> instancePersisters = new List<InstancePersister>();

		public void SaveGame() {
			SaveInstances();

			PerformSaveOperationOnAllPersistentClasses(persistenceTool);
		}

		public void SaveGameAsync(TaskFinishedCallback asyncSaveJobFinishedCallback) {
			PrimeInstancesForAsyncSave();
			StartCoroutine(
				PerformSaveOperationOnAllPersistentClassesOverFrames(asyncPersistenceTool, 4, () => {
					StartCoroutine(CheckIfAsyncSaveJobHasFinished(persistenceTool.SaveAsync(), asyncSaveJobFinishedCallback));
				})
			);
		}

		public void LoadGame() {
			if (!persistenceTool.SaveFileExists()) return;

			LoadInstances();
			LoadPersistentClasses(GetAllPersistentClasses());
		}

		public void LoadThisObjectAndChildren(MonoBehaviour monoBehaviour) {
			if (!persistenceTool.SaveFileExists()) return;
			LoadPersistentClasses(monoBehaviour.GetComponentsInChildren<MonoBehaviour>().OfType<IPersistent>().ToArray());
		}

		IEnumerator CheckIfAsyncSaveJobHasFinished(JobHandle asyncSaveJob, TaskFinishedCallback asyncSaveJobFinishedCallback) {
			while (true) {
				if (asyncSaveJob.IsCompleted) {
					asyncSaveJob.Complete();
					persistenceTool.ClearObjectsPrimedForAsyncSave();
					asyncSaveJobFinishedCallback();
					yield break;
				}

				yield return null;
			}
		}

		void PerformSaveOperationOnAllPersistentClasses(IPersistenceUtil persistenceUtil) {
			var persistentClasses = GetAllPersistentClasses();
			foreach (IPersistent persistentClass in persistentClasses) {
				persistentClass.Save(persistenceUtil);
			}
		}

		IEnumerator PerformSaveOperationOnAllPersistentClassesOverFrames(IPersistenceUtil persistenceUtil, int numberOfFrames = 1, TaskFinishedCallback saveOperationsFinished = null) {
			var persistentClasses = GetAllPersistentClasses();
			var batchSize = persistentClasses.Length / numberOfFrames;

			for (int i = 0; i < persistentClasses.Length; i++) {
				if (persistentClasses[i].DisablePersistence) continue;
				persistentClasses[i].Save(persistenceUtil);
				if (i % batchSize == 0) yield return null;
			}

			if (saveOperationsFinished != null) saveOperationsFinished();
		}

		void LoadPersistentClasses(IPersistent[] persistentClasses) {
			foreach (IPersistent persistentClass in persistentClasses) {
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

		void PrimeInstancesForAsyncSave() {
			foreach (var instancePersister in instancePersisters) {
				instancePersister.PrimeInstancesForAsyncSave(persistenceTool);
			}
		}

		IPersistent[] GetAllPersistentClasses() {
			return FindObjectsOfType<MonoBehaviour>().OfType<IPersistent>().Where((persistentClass) => persistentClass.DisablePersistence == false).ToArray();
		}

		[SerializeField] bool asyncSaveGame;
		[SerializeField] bool saveGame;
		[SerializeField] bool loadGame;

		void OnValidate() {
			if (saveGame) {
				SaveGame();
				saveGame = false;
			} else if (asyncSaveGame) {
				SaveGameAsync(() => Debug.Log("Done"));
				asyncSaveGame = false;
			} else if (loadGame) {
				LoadGame();
				loadGame = false;
			}
		}
	}
}
