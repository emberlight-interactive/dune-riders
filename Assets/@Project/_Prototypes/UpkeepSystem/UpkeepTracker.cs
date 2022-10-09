using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.UpkeeperSystem {
	public class UpkeepTracker : MonoBehaviour
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
}
