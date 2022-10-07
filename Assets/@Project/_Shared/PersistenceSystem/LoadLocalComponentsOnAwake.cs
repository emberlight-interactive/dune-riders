using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders.Shared.PersistenceSystem {
	public class LoadLocalComponentsOnAwake : MonoBehaviour {
		void Awake() {
			var persistenceManager = FindObjectOfType<PersistenceManagerBase>();
			if (persistenceManager != null) persistenceManager.LoadThisObjectAndChildren(this);
		}
	}
}
