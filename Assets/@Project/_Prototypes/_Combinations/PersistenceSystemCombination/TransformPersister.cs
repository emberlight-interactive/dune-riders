using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.Shared.PersistenceSystem {
	public class TransformPersister : MonoBehaviour, IPersistent
	{
		[SerializeField] bool usePrefabInstanceKeyInstead;
		[SerializeField] string persistenceKey;

		[Serializable]
		class TransformSerializable {
			public Vector3 position;
			public Quaternion rotation;
			public Vector3 scale;
		}

		public bool DisablePersistence { get => false; }

		public void Save(IPersistenceUtil persistUtil) {
			persistUtil.Save(GetPersistenceKey(), new TransformSerializable {
				position = transform.position,
				rotation = transform.rotation,
				scale = transform.localScale,
			});
		}

        public void Load(IPersistenceUtil persistUtil) {
			var transformSerializable = persistUtil.Load<TransformSerializable>(GetPersistenceKey());
			transform.position = transformSerializable.position;
			transform.rotation = transformSerializable.rotation;
			transform.localScale = transformSerializable.scale;
		}

		string GetPersistenceKey() {
			if (usePrefabInstanceKeyInstead) {
				if (GetComponent<UniqueIdentifier>() != null) {
					return GetComponent<UniqueIdentifier>().uniqueIdentifier;
				} else {
					Debug.LogError("No prefab instance key exists");
					return this.persistenceKey;
				}
			} else {
				return this.persistenceKey;
			}
		}
	}
}
