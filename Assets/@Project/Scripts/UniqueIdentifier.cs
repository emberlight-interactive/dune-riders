using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

namespace DuneRiders {
	class GuidAssigner {
		public static void DetectAndApplyGuid(UniqueIdentifier uniqueIdentifier) {
			if (uniqueIdentifier.uniqueIdentifier == String.Empty) ApplyNewGuid(uniqueIdentifier);
			else {
				var otherIdentifiers = UnityEngine.Object.FindObjectsOfType<UniqueIdentifier>();
				var identiferCount = otherIdentifiers.Where((identifiers) => identifiers.uniqueIdentifier == uniqueIdentifier.uniqueIdentifier).Count();
				if (identiferCount > 1) {
					ApplyNewGuid(uniqueIdentifier);
				}
			}
		}

		public static void ApplyNewGuid(UniqueIdentifier uniqueIdentifier) {
			uniqueIdentifier.uniqueIdentifier = Guid.NewGuid().ToString();
			PrefabUtility.RecordPrefabInstancePropertyModifications(uniqueIdentifier);
			EditorUtility.SetDirty(uniqueIdentifier);
		}
	}

	[DisallowMultipleComponent]
	public class UniqueIdentifier : MonoBehaviour
	{
		[ReadOnly] public string uniqueIdentifier = String.Empty;

		void Reset() {
			GuidAssigner.DetectAndApplyGuid(this);
		}

		void Awake() {
			GuidAssigner.DetectAndApplyGuid(this);
		}

		void OnValidate() {
			GuidAssigner.DetectAndApplyGuid(this);
		}
	}

	[CustomEditor(typeof(UniqueIdentifier))]
	public class UniqueIdentifierEditor : Editor {
		void OnEnable()
		{
			var uniqueIdentifier = (UniqueIdentifier) target;
			GuidAssigner.DetectAndApplyGuid(uniqueIdentifier);
		}
	}
}
