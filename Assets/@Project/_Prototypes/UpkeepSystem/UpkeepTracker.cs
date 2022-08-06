using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.UpkeeperSystem {
	public class UpkeepTracker : MonoBehaviour // todo: When we build the burn rate cycle add increases by the upkeep to the next cycle
	{
		public enum UpkeepType {
			Fuel,
			ScrapMetal,
			PreciousMetal,
		}

		[Serializable] class UpkeepDictionary : SerializableDictionary<UpkeepType, float> {}
		[SerializeField, ReadOnly] List<Upkeep> upkeepComponents = new List<Upkeep>();
		[SerializeField, ReadOnly] UpkeepDictionary upkeepRates = new UpkeepDictionary() {
			{ UpkeepType.Fuel, 0f },
			{ UpkeepType.ScrapMetal, 0f },
			{ UpkeepType.PreciousMetal, 0f },
		};

		void OnEnable() {
			upkeepComponents.Clear();
			upkeepComponents.AddRange(FindObjectsOfType<Upkeep>());
			UpdateUpkeepRates();
		}

		public void Add(Upkeep upkeep) {
			if (upkeepComponents.Find(item => item == upkeep) == null) {
				upkeepComponents.Add(upkeep);
				UpdateUpkeepRates();
			}
		}

		public void Remove(Upkeep upkeep) {
			if (upkeepComponents.Remove(upkeep)) {
				UpdateUpkeepRates();
			}
		}

		public float GetUpkeepRate(UpkeepType upkeepType) {
			return upkeepRates[upkeepType];
		}

		void UpdateUpkeepRates() {
			var tempUpkeepRates = new Dictionary<UpkeepType, float>() {
				{ UpkeepType.Fuel, 0f },
				{ UpkeepType.ScrapMetal, 0f },
				{ UpkeepType.PreciousMetal, 0f },
			};

			foreach (var upkeep in upkeepComponents) {
				tempUpkeepRates[upkeep.UpkeepType] += upkeep.Rate;
			}

			foreach (var kvp in tempUpkeepRates) {
				upkeepRates[kvp.Key] = kvp.Value;
			}
		}
	}

	[Serializable]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver // I'm so smart ..... at ctrl+c ctrl+v
	{
		[SerializeField]
		private List<TKey> keys = new List<TKey>();

		[SerializeField]
		private List<TValue> values = new List<TValue>();

		// save the dictionary to lists
		public void OnBeforeSerialize()
		{
			keys.Clear();
			values.Clear();
			foreach(KeyValuePair<TKey, TValue> pair in this)
			{
				keys.Add(pair.Key);
				values.Add(pair.Value);
			}
		}

		// load dictionary from lists
		public void OnAfterDeserialize()
		{
			this.Clear();

			if(keys.Count != values.Count)
				throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

			for(int i = 0; i < keys.Count; i++)
				this.Add(keys[i], values[i]);
		}
	}
}
