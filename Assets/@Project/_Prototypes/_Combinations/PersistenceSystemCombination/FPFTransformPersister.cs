using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gaia;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders.PersistenceSystemCombination {
	public class FPFTransformPersister : MonoBehaviour, IPersistent
	{
		[SerializeField] bool usePrefabInstanceKeyInstead;
		[SerializeField] string persistenceKey;
		FloatingPointFix floatingPointFix;

		[Serializable]
		class FPFTransformSerializable {
			public Vector3 position;
			public Quaternion rotation;
			public Vector3 scale;
		}

		public bool DisablePersistence { get => false; }

		void Awake() {
			floatingPointFix = FindObjectOfType<FloatingPointFix>();
		}

		public void Save(IPersistenceUtil persistUtil) {
			persistUtil.Save(GetPersistenceKey(), new FPFTransformSerializable {
				position = ConvertPosition(transform.position),
				rotation = transform.rotation,
				scale = transform.localScale,
			});
		}

        public void Load(IPersistenceUtil persistUtil) {
			var transformSerializable = persistUtil.Load<FPFTransformSerializable>(GetPersistenceKey());
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

		Vector3 ConvertPosition(Vector3 pos) {
			if (floatingPointFix != null && (GetComponent<FloatingPointFixMember>() != null || GetComponent<FloatingPointFix>() != null)) {
				return floatingPointFix.ConvertToOriginalSpace(pos);
			} else {
				return pos;
			}
		}
	}
}
