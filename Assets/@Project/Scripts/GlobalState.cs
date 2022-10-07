using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders {
	/// <summary>
	/// Strategy to maintain state outside of the lifespan of a prefab
	/// </summary>
	public class GlobalState
	{
		public static void InitState<TGlobalState, TKey, TState>(TKey key, TState state, out TState localState, Type[] includeComponents = null) where TGlobalState : GlobalStateGameObject<TKey, TState> {
			TGlobalState globalState;
			TGlobalState existingGlobalState = MonoBehaviour.FindObjectOfType<TGlobalState>();
			if (existingGlobalState != null) {
				globalState = existingGlobalState;
			} else {
				var globalStateObject = new GameObject($"{typeof(TState).Name} [GlobalState]");
				globalState = globalStateObject.AddComponent<TGlobalState>();

				if (includeComponents != null) {
					foreach (var component in includeComponents) {
						globalStateObject.AddComponent(component);
					}
				}
			}

			globalState.AddState(key, state);
			localState = globalState.GetState(key);
		}
	}

	public class GlobalStateGameObject<TKey, TState> : MonoBehaviour, IPersistent {
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
		string persistenceKey = $"{typeof(TState).Name}GlobalState";

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

		public bool DisablePersistence { get => false; }

		// todo: takes away flexibility for modification
        public void Save(IPersistenceUtil persistUtil) {
			persistUtil.Save<Dictionary<TKey, TState>>(persistenceKey, (Dictionary<TKey, TState>) states);
		}

        public void Load(IPersistenceUtil persistUtil) {
			var loadedDictionary = persistUtil.Load<Dictionary<TKey, TState>>(persistenceKey);

			if (loadedDictionary != default(Dictionary<TKey, TState>)) {
				var stateDictionary = new GlobalStateDictionary();
				foreach(var elem in loadedDictionary) {
					stateDictionary.Add(elem.Key, elem.Value);
				}

				states = stateDictionary;
			}
		}
	}
}
