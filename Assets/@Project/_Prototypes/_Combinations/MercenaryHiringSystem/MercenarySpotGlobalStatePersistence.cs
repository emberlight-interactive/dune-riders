using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.MercenaryHiringSystem {
	public class MercenarySpotGlobalStatePersistence : MonoBehaviour
	{
		GlobalStateGameObject<string, MercenaryHiringSpotState> mercenaryHiringSpotGlobalState;
		string persistenceKey = "MercenaryHiringSpotGlobalState";

		void Awake() {
			mercenaryHiringSpotGlobalState = GetComponent<GlobalStateGameObject<string, MercenaryHiringSpotState>>();

			if (ES3.KeyExists(persistenceKey, ES3Settings.defaultSettings)) {
				var mercenaryHiringSpotStateDictionary = new GlobalStateGameObject<string, MercenaryHiringSpotState>.GlobalStateDictionary();
				var loadedDictionary = ES3.Load<Dictionary<string, MercenaryHiringSpotState>>(persistenceKey);
				foreach(var elem in loadedDictionary)
    				mercenaryHiringSpotStateDictionary.Add(elem.Key, elem.Value);

				mercenaryHiringSpotGlobalState.states = mercenaryHiringSpotStateDictionary;
			}
		}

		void OnApplicationQuit() {
			ES3.Save<Dictionary<string, MercenaryHiringSpotState>>(persistenceKey, (Dictionary<string, MercenaryHiringSpotState>) mercenaryHiringSpotGlobalState.states, ES3Settings.defaultSettings);
		}
	}
}
