using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.MercenaryHiringSystem {
	[Serializable]
	class MercenaryHiringSpotState {
		public string transformHash;
		public bool mercenaryHired = false;
	}

	public class MercenaryHiringSpotSpawner : MonoBehaviour
	{
		[SerializeField] GameObject mercenaryHiringSpot;
		MercenaryInteractionTarget mercenaryInteractionTarget;
		ProceduralTools proceduralTools;
		string transformHash;

		MercenaryHiringSpotGlobalState globalState;
		[SerializeField, ReadOnly] MercenaryHiringSpotState state;

		void Awake() {
			proceduralTools = new ProceduralTools(transform);
			transformHash = proceduralTools.BuildTransformHash();
			InitializeGlobalState();
			InitializeLocalState();
		}

		void Start() {
			if (!state.mercenaryHired) {
				SpawnMercenaryHiringSpot();
			}
		}

		void SpawnMercenaryHiringSpot() {
			Instantiate(mercenaryHiringSpot, transform);
			mercenaryInteractionTarget = GetComponentInChildren<MercenaryInteractionTarget>();
			mercenaryInteractionTarget?.mercenaryHiredEvent.AddListener(() => MarkMercenaryHired());
		}

		public void MarkMercenaryHired() {
			state.mercenaryHired = true;
		}

		void InitializeGlobalState() {
			MercenaryHiringSpotGlobalState existingGlobalState = FindObjectOfType<MercenaryHiringSpotGlobalState>();
			if (existingGlobalState != null) {
				globalState = existingGlobalState;
				return;
			}

			globalState = new GameObject("MercenaryHiringSpotGlobalState").AddComponent<MercenaryHiringSpotGlobalState>();
		}

		void InitializeLocalState() {
			var mercenaryHiringSpotState = new MercenaryHiringSpotState() {
				transformHash = transformHash,
				mercenaryHired = false,
			};

			globalState.AddState(mercenaryHiringSpotState);
			state = globalState.GetState(transformHash);
		}

		#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			Color sphereColor = Color.magenta;

			GUIStyle style = new GUIStyle();
			style.normal.textColor = sphereColor;

			Gizmos.DrawSphere(transform.position, 1);
		}
		#endif

		class MercenaryHiringSpotGlobalState : MonoBehaviour
		{
			private static MercenaryHiringSpotGlobalState _instance;
			public static MercenaryHiringSpotGlobalState Instance { get { return _instance; } }

			private void Awake()
			{
				if (_instance != null && _instance != this)
				{
					Destroy(this.gameObject);
				} else {
					_instance = this;
				}
			}

			[Serializable] public class MercenaryHiringSpotStateDictionary : SerializableDictionary<string, MercenaryHiringSpotState> {}
			[ReadOnly] public MercenaryHiringSpotStateDictionary mercHiringStates = new MercenaryHiringSpotStateDictionary();

			/// <summary>
			/// Idempotent state setter
			/// </summary>
			public void AddState(MercenaryHiringSpotState mercHiringState) {
				if (!mercHiringStates.ContainsKey(mercHiringState.transformHash)) mercHiringStates.Add(mercHiringState.transformHash, mercHiringState);
			}

			public MercenaryHiringSpotState GetState(string transformHash) {
				MercenaryHiringSpotState mercHiringState;
				if (mercHiringStates.TryGetValue(transformHash, out mercHiringState)) return mercHiringState;
				else return null;
			}
		}
	}
}
