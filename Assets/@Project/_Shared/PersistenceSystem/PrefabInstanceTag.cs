using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.Shared.PersistenceSystem {
	[RequireComponent(typeof(UniqueIdentifier))]
	[Obsolete]
	public class PrefabInstanceTag : MonoBehaviour
	{
		public string prefabInstanceKey = String.Empty;

		void Awake() {
			if (prefabInstanceKey == String.Empty) prefabInstanceKey = Guid.NewGuid().ToString();
		}
	}
}
