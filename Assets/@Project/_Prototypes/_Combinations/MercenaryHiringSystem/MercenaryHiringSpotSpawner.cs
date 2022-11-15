using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders.MercenaryHiringSystem {
	[Serializable]
	class MercenaryHiringSpotState {
		public string transformHash;
		public bool mercenaryHired = false;
	}

	public class MercenaryHiringSpotSpawner : MonoBehaviour
	{
		[SerializeField] bool enableRegeneration = false;
		[SerializeField] float percentageChanceOfRegeneration;

		[SerializeField] GameObject mercenaryHiringSpot;
		GameObject spawnedSpot;
		MercenaryInteractionTarget mercenaryInteractionTarget;
		ProceduralTools proceduralTools;
		string transformHash;

		MercenaryHiringSpotGlobalState globalState;
		[SerializeField, ReadOnly] MercenaryHiringSpotState state;

		void Awake() {
			proceduralTools = new ProceduralTools(transform, true);
			transformHash = proceduralTools.BuildTransformHash();
			InitializeState();
		}

		void Start() {
			if (!state.mercenaryHired) {
				SpawnMercenaryHiringSpot();
			}
		}

		void OnDestroy() {
			if (enableRegeneration) HandleRegeneration();
		}

		void SpawnMercenaryHiringSpot() {
			spawnedSpot = Instantiate(mercenaryHiringSpot, transform);
			mercenaryInteractionTarget = GetComponentInChildren<MercenaryInteractionTarget>();
			mercenaryInteractionTarget?.mercenaryHiredEvent.AddListener(() => MarkMercenaryHired());
		}

		public void MarkMercenaryHired() {
			state.mercenaryHired = true;
		}


		void InitializeState() {
			var mercenaryHiringSpotState = new MercenaryHiringSpotState() {
				transformHash = transformHash,
				mercenaryHired = false,
			};

			GlobalState.InitState<MercenaryHiringSpotGlobalState, string, MercenaryHiringSpotState>(
				transformHash,
				mercenaryHiringSpotState,
				out state,
				new Type[] { typeof(LoadLocalComponentsOnAwake) }
			);
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

		void HandleRegeneration() {
			var randomNumber = UnityEngine.Random.Range(1, 100);
			if (randomNumber <= percentageChanceOfRegeneration * 100) {
				state.mercenaryHired = false;
			}
		}

		class MercenaryHiringSpotGlobalState : GlobalStateGameObject<string, MercenaryHiringSpotState> {}
	}
}
