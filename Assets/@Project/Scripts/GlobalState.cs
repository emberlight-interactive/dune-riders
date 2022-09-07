using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders {
	/// <summary>
	/// Strategy to maintain state outside of the lifespan of a prefab
	/// </summary>
	public class GlobalState
	{
		public static void InitState<TGlobalState, TKey, TState>(TKey key, TState state, out TState localState) where TGlobalState : GlobalStateGameObject<TKey, TState> {
			TGlobalState globalState;
			TGlobalState existingGlobalState = MonoBehaviour.FindObjectOfType<TGlobalState>();
			if (existingGlobalState != null) {
				globalState = existingGlobalState;
			} else {
				globalState = new GameObject($"{typeof(TState).Name} [GlobalState]").AddComponent<TGlobalState>();
			}


			globalState.AddState(key, state);
			localState = globalState.GetState(key);
		}
	}

	public class GlobalStateGameObject<TKey, TState> : MonoBehaviour {
		private static GlobalStateGameObject<TKey, TState> _instance;
		public static GlobalStateGameObject<TKey, TState> Instance { get { return _instance; } }

		private void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Destroy(this.gameObject);
			} else {
				_instance = this;
			}
		}

		[Serializable] public class GlobalStateDictionary : SerializableDictionary<TKey, TState> {}
		[ReadOnly] public GlobalStateDictionary states = new GlobalStateDictionary();

		/// <summary>
		/// Idempotent state setter
		/// </summary>
		public void AddState(TKey key, TState state) {
			if (!states.ContainsKey(key)) states.Add(key, state);
		}

		public TState GetState(TKey key) {
			TState state;
			if (states.TryGetValue(key, out state)) return state;
			else return default(TState);
		}
	}
}
