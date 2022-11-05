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
		[SerializeField] float minRegenerationTime;
		[SerializeField] float maxRegenerationTime;

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

		void OnEnable() {
			if (enableRegeneration) StartCoroutine(RegenerateSpot());
		}

		void Start() {
			if (!state.mercenaryHired) {
				SpawnMercenaryHiringSpot();
			}
		}

		void OnDisable() {
			StopAllCoroutines();
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

		IEnumerator RegenerateSpot() {
			while (true) {
				if (state.mercenaryHired) {
					yield return new WaitForSeconds(UnityEngine.Random.Range(minRegenerationTime, maxRegenerationTime));
					Destroy(spawnedSpot);
					state.mercenaryHired = false;
					SpawnMercenaryHiringSpot();
				}

				yield return new WaitForSeconds(10f);
			}
		}

		class MercenaryHiringSpotGlobalState : GlobalStateGameObject<string, MercenaryHiringSpotState> {}
	}
}
