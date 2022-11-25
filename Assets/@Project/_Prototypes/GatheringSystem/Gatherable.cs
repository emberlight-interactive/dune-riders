using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.Shared.Sound;

namespace DuneRiders.GatheringSystem {
	public class Gatherable : MonoBehaviour
	{
		Gatherer gatherer;
		protected virtual Vector3 restingDestination { get; set; }
		[SerializeField] Gatherer.SupportedResources resourceType;
		[SerializeField] SoundPlayer succSoundPlayer;
		public int amount = 5;
		public float yOffsetOfRestingPlace = 2f;
		public float moveSpeed = 1f;
		public float gatherSpeed = 5f;
		public float rarityValue = 1f;
		[Tooltip("Maximum number of seconds it is moving towards the gatherer")]
		public float maxSecondsToGatherer = 1.2f;
		bool gathered = false;
		bool despawnTimerStarted = false;

		void OnEnable() {
			SetYAxisOfDestinationToGroundLevel();
			OffSetDestination();
			gathered = false;
			despawnTimerStarted = false;
		}

		void Update() {
			if (gatherer != null) {
				MoveTowardsGatherer();
				if (!despawnTimerStarted) {
					StartCoroutine(DespawnTimer());
					despawnTimerStarted = true;
				}
			} else MoveTowardsDestination();
		}

		void OnDisable() {
			StopAllCoroutines();
		}

		void SetYAxisOfDestinationToGroundLevel() {
			RaycastHit hit;
			if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), -Vector3.up, out hit, Mathf.Infinity, 1, QueryTriggerInteraction.Ignore)) {
				restingDestination = new Vector3(transform.position.x, hit.point.y, transform.position.z);
			}
		}

		void OffSetDestination() {
			restingDestination = new Vector3(restingDestination.x, restingDestination.y + yOffsetOfRestingPlace, restingDestination.z);
		}

		void MoveTowardsDestination() {
			if (transform.position == restingDestination) return;

			float step = moveSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, restingDestination, step);
		}

		void MoveTowardsGatherer() {
			if (Vector3.Distance(transform.position, gatherer.transform.position) < 0.2f) SimplePool.Despawn(gameObject);

			float step = gatherSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, gatherer.transform.position, step);
			transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0.1f, 0.1f, 0.1f), step / 4);
		}

		IEnumerator DespawnTimer() {
			yield return new WaitForSeconds(maxSecondsToGatherer);
			SimplePool.Despawn(gameObject);
		}

		private void OnTriggerEnter(Collider other) {
			var gathererComponent = other.GetComponentInParent<Gatherer>();

			if (gathererComponent && !gathered) {
				if (gathererComponent.GetManager(resourceType).Give(amount)) {
					succSoundPlayer.Play();

					gatherer = gathererComponent;
					gatherer.rootAdditionRow.RenderAddition(resourceType, amount);
					gathered = true;
				}
			}
		}
	}
}
