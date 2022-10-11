using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.Shared.PersistenceSystem {
	public abstract class InstancePersister : MonoBehaviour {
		[SerializeField] GameObject prefab;
		protected abstract string PrefabNickName { get; }

		[Serializable]
		class InstanceSerializable {
			public string uniqueIdentifier;
		}

		public abstract GameObject[] GetAllPrefabInstances();

		List<InstanceSerializable> CompileSerializableInstances() {
			var instances = GetAllPrefabInstances();
			List<InstanceSerializable> instancesToSave = new List<InstanceSerializable>();

			foreach (var instance in instances) {
				instancesToSave.Add(new InstanceSerializable {
					uniqueIdentifier = instance.GetComponent<UniqueIdentifier>().uniqueIdentifier,
				});
			}

			return instancesToSave;
		}

		public void SaveInstances(IPersistenceUtilInternal persistenceTool) {
			var instancesToSave = CompileSerializableInstances();

			persistenceTool.Save(PrefabNickName, instancesToSave.ToArray());
		}

		public void PrimeInstancesForAsyncSave(IPersistenceUtilInternal persistenceTool) {
			var instancesToSave = CompileSerializableInstances();

			persistenceTool.PrimeObjectForAsyncSave(PrefabNickName, instancesToSave.ToArray());
		}

		public void LoadInstances(IPersistenceUtilInternal persistenceTool) {
			var currentInstances = GetAllPrefabInstances();
			var loadedInstances = persistenceTool.Load<InstanceSerializable[]>(PrefabNickName);

			foreach(var loadedInstance in loadedInstances) {
				var currentInstance = currentInstances.Where((instance) => instance.GetComponent<UniqueIdentifier>().uniqueIdentifier == loadedInstance.uniqueIdentifier).FirstOrDefault();
				if (currentInstance != default(GameObject)) {
					if (currentInstance.GetComponent<UniqueIdentifier>() != null) {
						currentInstance.GetComponent<UniqueIdentifier>().uniqueIdentifier = loadedInstance.uniqueIdentifier;
					}

					continue;
				}

				var gm = Instantiate(prefab);
				var uniqueIdentifier = gm.GetComponent<UniqueIdentifier>();

				if (uniqueIdentifier != null) uniqueIdentifier.uniqueIdentifier = loadedInstance.uniqueIdentifier;
			}
		}
	}
}
