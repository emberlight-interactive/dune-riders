using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders {
	public class GlobalStatePersistence<State> : MonoBehaviour
	{
		GlobalStateGameObject<string, State> globalState;
		string persistenceKey = $"{typeof(State).Name}GlobalState";

		void Awake() {
			globalState = GetComponent<GlobalStateGameObject<string, State>>();

			if (ES3.KeyExists(persistenceKey, ES3Settings.defaultSettings)) {
				var stateDictionary = new GlobalStateGameObject<string, State>.GlobalStateDictionary();
				var loadedDictionary = ES3.Load<Dictionary<string, State>>(persistenceKey);
				foreach(var elem in loadedDictionary)
    				stateDictionary.Add(elem.Key, elem.Value);

				globalState.states = stateDictionary;
			}
		}

		void OnApplicationQuit() {
			ES3.Save<Dictionary<string, State>>(persistenceKey, (Dictionary<string, State>) globalState.states, ES3Settings.defaultSettings);
		}
	}
}
