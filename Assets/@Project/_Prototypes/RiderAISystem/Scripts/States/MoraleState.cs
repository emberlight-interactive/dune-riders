using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAI.State {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(AllActiveRidersState))]
	[RequireComponent(typeof(Rider))]
	public class MoraleState : MonoBehaviour
	{
		public enum MoraleOptions {Steady, Broken};
		[ReadOnly] public MoraleOptions morale = MoraleOptions.Steady;

		AllActiveRidersState allActiveRidersState;
		Rider rider;

		float maxTeammatesToLive = 0;
		float teammatesAlive = 0;

		float numberOfEnemiesAlive = 0;

		void Awake()
		{
			allActiveRidersState = GetComponent<AllActiveRidersState>();
			rider = GetComponent<Rider>();
		}

		void Start() {
			StartCoroutine(UpdateMoraleState());
		}

		IEnumerator UpdateMoraleState() {
			while (true) {
				UpdateNumberOfAliveTeammates();
				UpdateNumberOfAliveEnemies();
				if (HaveAliveTeammatesFallenBelow(0.26f) && DoesTheEnemyOverwhelmUsBy(2.5f)) {
					morale = MoraleOptions.Broken;
					yield break;
				}
				yield return new WaitForSeconds(2f);
			}
		}

		void UpdateNumberOfAliveTeammates() {
			var allFriendlyRiders = allActiveRidersState.GetAllRidersOfAllegiance(rider.allegiance);
			if (maxTeammatesToLive < allFriendlyRiders.Count) maxTeammatesToLive = allFriendlyRiders.Count;
			teammatesAlive = allFriendlyRiders.Count;
		}

		void UpdateNumberOfAliveEnemies() {
			numberOfEnemiesAlive = allActiveRidersState.GetAllRidersOfAllegiance(rider.enemyAllegiance).Count;
		}

		bool HaveAliveTeammatesFallenBelow(float percentage) {
			return ((teammatesAlive / maxTeammatesToLive) < percentage);
		}

		bool DoesTheEnemyOverwhelmUsBy(float thisManyTimesBigger) {
			return ((numberOfEnemiesAlive / teammatesAlive) > thisManyTimesBigger);
		}
	}
}
