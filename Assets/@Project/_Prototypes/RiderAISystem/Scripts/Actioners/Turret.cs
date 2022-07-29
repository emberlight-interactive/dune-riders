using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;

namespace DuneRiders.RiderAI.Actioners {
	public class Turret : MonoBehaviour
	{
		[SerializeField] TurretTrigger trigger;
		Transform riderCurrentlyTargetting;
		Transform aimGuider;
		public float turretTurnSpeed = 1;
		Quaternion originalRotation;

		// Hacky af //
		[SerializeField] float upperXBounds = 310;
		[SerializeField] float lowerXBounds = 0;

		void Update()
		{
			if (riderCurrentlyTargetting && riderCurrentlyTargetting.gameObject.activeSelf) {
				if (aimGuider) IncrementTurretBarrelTowardsTarget(aimGuider);
				else IncrementTurretBarrelTowardsTarget(riderCurrentlyTargetting);
			} else {
				ReturnTurretToDefaultPosition();
			}
		}

		public void FireOnTarget(Transform rider, float yOffset = 0f) {
			var targetTransform = rider.GetComponent<TargetTransform>();
			if (targetTransform) aimGuider = targetTransform.target;

			riderCurrentlyTargetting = rider;

			if (!riderCurrentlyTargetting.gameObject.activeSelf) {
				riderCurrentlyTargetting = null;
				return;
			}
		}

		public void StopFiring() {
			riderCurrentlyTargetting = null;
		}

		void Start() {
			originalRotation = transform.localRotation;
			StartCoroutine(Gunner());
		}

		IEnumerator Gunner() {
			while (true) {

				if (riderCurrentlyTargetting && IsTurretAimedAtTarget()) {
					trigger.PullTrigger();
				} else {
					trigger.ReleaseTrigger();
				}

				yield return null;
			}
		}

		void IncrementTurretBarrelTowardsTarget(Transform target) {
			Vector3 targetDirection = target.position - transform.position;
			float singleStep = turretTurnSpeed * Time.deltaTime;
			Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
			Debug.DrawRay(transform.position, newDirection, Color.red);

			var rotation = Quaternion.LookRotation(newDirection);
			var rotationInEulerAngles = rotation.eulerAngles;
			rotationInEulerAngles.z = 0;

			rotation.eulerAngles = rotationInEulerAngles;

			transform.rotation = rotation;

			var localRotation = transform.localRotation.eulerAngles;
			if (localRotation.y < 270 && localRotation.y > 180) {
				localRotation.y = 270;
			} else if (localRotation.y > 90 && localRotation.y < 180) {
				localRotation.y = 90;
			}

			if (localRotation.x < upperXBounds && localRotation.x > 180) {
				localRotation.x = upperXBounds;
			} else if (localRotation.x > lowerXBounds && localRotation.x < 180) {
				localRotation.x = lowerXBounds;
			}

			var newLocalRotation = new Quaternion();
			newLocalRotation.eulerAngles = localRotation;
			transform.localRotation = newLocalRotation;
		}

		bool IsTurretAimedAtTarget() {
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)) {
				GameObject currentGameObject = hit.collider.gameObject;
				if(GameObject.ReferenceEquals(hit.collider.gameObject, riderCurrentlyTargetting.gameObject)) return true;

				while (currentGameObject.transform.parent != null) {
					currentGameObject = currentGameObject.transform.parent.gameObject;
					if(GameObject.ReferenceEquals(currentGameObject, riderCurrentlyTargetting.gameObject)) return true;
				}
			}

			return false;
		}

		void ReturnTurretToDefaultPosition() {
			if (transform.localRotation == originalRotation) return;
			transform.localRotation = Quaternion.RotateTowards(transform.localRotation, originalRotation, 20.0f * Time.deltaTime);
		}
	}
}
