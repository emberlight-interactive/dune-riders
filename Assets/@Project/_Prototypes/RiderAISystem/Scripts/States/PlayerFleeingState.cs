using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAI.State {
	public class PlayerFleeingState : MonoBehaviour
	{
		PlayerFleeingStateGlobalState globalState;
		public bool PlayerFleeing { get => globalState.playerFleeing; }

		void Awake() {
			InitializeGlobalState();
		}

		void InitializeGlobalState() {
			PlayerFleeingStateGlobalState existingGlobalState = FindObjectOfType<PlayerFleeingStateGlobalState>();
			if (existingGlobalState != null) {
				globalState = existingGlobalState;
				return;
			}

			globalState = new GameObject("PlayerFleeingStateGlobalState").AddComponent<PlayerFleeingStateGlobalState>();
			BubbleGameObjectToActiveScene.BubbleUp(globalState.gameObject);
		}

		[RequireComponent(typeof(PlayerCommandState))]
		[RequireComponent(typeof(AveragePositionOfEntitiesState))]
		class PlayerFleeingStateGlobalState : MonoBehaviour
		{
			private static PlayerFleeingStateGlobalState _instance;
			public static PlayerFleeingStateGlobalState Instance { get { return _instance; } }

			PlayerCommandState playerCommandState;
			AveragePositionOfEntitiesState averagePositionOfEntitiesState;
			Player player;

			private void Awake()
			{
				if (_instance != null && _instance != this)
				{
					Destroy(this.gameObject);
				} else {
					_instance = this;
				}

				playerCommandState = GetComponent<PlayerCommandState>();
				averagePositionOfEntitiesState = GetComponent<AveragePositionOfEntitiesState>();
				player = FindObjectOfType<Player>();

				StartCoroutine(UpdateState());
			}

			public bool playerFleeing = false;

			IEnumerator UpdateState() {
				int consecutiveCheck = 0;
				float lastCheckedPlayerDistanceFromEnemyAverage = 0f;

				while (true) {
					yield return new WaitForSeconds(2f);

					if (playerCommandState.command == PlayerCommandState.Command.Follow || playerCommandState.command == PlayerCommandState.Command.Retreat) {
						var currentPlayerDistanceFromEnemyAverage = Vector3.Distance(averagePositionOfEntitiesState.GetAverageWorldPositionOfEntities(Allegiance.Bandits), player.transform.position);
						if (currentPlayerDistanceFromEnemyAverage > 25 && currentPlayerDistanceFromEnemyAverage > lastCheckedPlayerDistanceFromEnemyAverage) {
							consecutiveCheck++;
							lastCheckedPlayerDistanceFromEnemyAverage = currentPlayerDistanceFromEnemyAverage;
							if (consecutiveCheck >= 4) playerFleeing = true;
							continue;
						}
					}

					lastCheckedPlayerDistanceFromEnemyAverage = 0f;
					consecutiveCheck = 0;
					playerFleeing = false;
				}
			}
		}
	}
}
