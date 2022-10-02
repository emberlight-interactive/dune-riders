using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders.PersistenceSystemCombination {
	public abstract class InstancePersister : MonoBehaviour {
		[SerializeField] GameObject prefab;
		protected abstract string PrefabNickName { get; }

		[Serializable]
		class InstanceSerializable {
			public int instanceId;
			public string prefabInstanceKey;
		}

		public abstract GameObject[] GetAllPrefabInstances();

		public void SaveInstances(PersistenceTool persistenceTool) {
			var instances = GetAllPrefabInstances();
			List<InstanceSerializable> instancesToSave = new List<InstanceSerializable>();

			foreach (var instance in instances) {
				instancesToSave.Add(new InstanceSerializable {
					instanceId = instance.GetInstanceID(),
					prefabInstanceKey = instance.GetComponent<PrefabInstanceTag>()?.prefabInstanceKey,
				});
			}

			persistenceTool.Delete(PrefabNickName);
			persistenceTool.Save(PrefabNickName, instancesToSave.ToArray());
		}

		public void LoadInstances(PersistenceTool persistenceTool) {
			var currentInstances = GetAllPrefabInstances();
			var loadedInstances = persistenceTool.Load<InstanceSerializable[]>(PrefabNickName);

			foreach(var loadedInstance in loadedInstances) {
				var currentInstance = currentInstances.Where((instance) => instance.GetInstanceID() == loadedInstance.instanceId).FirstOrDefault();
				if (currentInstance != default(GameObject)) {
					if (currentInstance.GetComponent<PrefabInstanceTag>() != null) {
						currentInstance.GetComponent<PrefabInstanceTag>().prefabInstanceKey = loadedInstance.prefabInstanceKey;
					}

					continue;
				}

				var gm = Instantiate(prefab);
				var prefabInstanceTag = gm.GetComponent<PrefabInstanceTag>();

				if (prefabInstanceTag != null) prefabInstanceTag.prefabInstanceKey = loadedInstance.prefabInstanceKey;
			}
		}
	}
}
