using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.GatheringSystem {
	public class Gatherable : MonoBehaviour
	{
		Gatherer gatherer;
		Vector3 restingDestination;
		[SerializeField] Gatherer.SupportedResources resourceType;
		public int amount = 5;
		public float yOffsetOfRestingPlace = 2f;
		public float moveSpeed = 1f;
		public float gatherSpeed = 5f;
		bool gathered = false;

		void OnEnable() {
			SetYAxisOfDestinationToGroundLevel();
			OffSetDestination();
			gathered = false;
		}

		void Update() {
			if (gatherer != null) MoveTowardsGatherer();
			else MoveTowardsDestination();
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
			if (Vector3.Distance(transform.position, gatherer.transform.position) < 0.05f) gameObject.SetActive(false);

			float step = gatherSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, gatherer.transform.position, step);
			transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0.1f, 0.1f, 0.1f), step / 4);
		}

		private void OnTriggerEnter(Collider other) {
			var gathererComponent = other.GetComponentInParent<Gatherer>();

			if (gathererComponent && !gathered) {
				if (gathererComponent.GetManager(resourceType).Give(amount)) {
					gatherer = gathererComponent;
					gathered = true;
				}
			}
		}
	}
}
