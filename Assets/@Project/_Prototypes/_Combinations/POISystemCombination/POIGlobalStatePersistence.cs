using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.POISystem;

namespace DuneRiders.POISystemCombination {
	public class POIGlobalStatePersistence : MonoBehaviour
	{
		GlobalStateGameObject<string, POI.POIState> poiGlobalState;
		string persistenceKey = "POIGlobalState";

		void Awake() {
			poiGlobalState = GetComponent<GlobalStateGameObject<string, POI.POIState>>();

			if (ES3.KeyExists(persistenceKey, ES3Settings.defaultSettings)) {
				var poiStateDictionary = new GlobalStateGameObject<string, POI.POIState>.GlobalStateDictionary();
				var loadedDictionary = ES3.Load<Dictionary<string, POI.POIState>>(persistenceKey);
				foreach(var elem in loadedDictionary)
    				poiStateDictionary.Add(elem.Key, elem.Value);

				poiGlobalState.states = poiStateDictionary;
			}
		}

		void OnApplicationQuit() {
			ES3.Save<Dictionary<string, POI.POIState>>(persistenceKey, (Dictionary<string, POI.POIState>) poiGlobalState.states, ES3Settings.defaultSettings);
		}
	}
}
