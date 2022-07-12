using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAI.Actioners {
	public class Turret : MonoBehaviour
	{
		[SerializeField] Rigidbody bulletPrefab;
		[SerializeField] Transform bulletSpawnPosition;
		Rider riderCurrentlyTargetting;
		public float turretTurnSpeed = 1;
		Quaternion originalRotation;

		void Update()
		{
			if (riderCurrentlyTargetting && riderCurrentlyTargetting.gameObject.activeSelf) {
				IncrementTurretBarrelTowardsTarget(riderCurrentlyTargetting.gameObject.transform);
			} else {
				ReturnTurretToDefaultPosition();
			}
		}

		public void FireOnTarget(Rider rider, float yOffset = 0f) {
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
				yield return new WaitForSeconds(2f);
				if (riderCurrentlyTargetting) {
					if (IsTurretAimedAtTarget()) {
						FireVolley();
					}
				}
			}
		}

		void IncrementTurretBarrelTowardsTarget(Transform target) {
			Vector3 targetDirection = target.position - transform.position;
			float singleStep = turretTurnSpeed * Time.deltaTime;
			Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
			Debug.DrawRay(transform.position, newDirection, Color.red);

			var rotation = Quaternion.LookRotation(newDirection);
			var rotationInEulerAngles = rotation.eulerAngles;
			rotationInEulerAngles.x = 0;
			rotationInEulerAngles.z = 0;

			rotation.eulerAngles = rotationInEulerAngles;

			transform.rotation = rotation;

			var localRotation = transform.localRotation.eulerAngles;
			if (localRotation.y < 270 && localRotation.y > 180) {
				localRotation.y = 270;
			} else if (localRotation.y > 90 && localRotation.y < 180) {
				localRotation.y = 90;
			}

			var newLocalRotation = new Quaternion();
			newLocalRotation.eulerAngles = localRotation;
			transform.localRotation = newLocalRotation;
		}

		void FireVolley() {
			var ball = SimplePool.Spawn(bulletPrefab.gameObject, bulletSpawnPosition.transform.position, bulletSpawnPosition.transform.rotation);
			ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
			ball.GetComponent<Rigidbody>().velocity += bulletSpawnPosition.transform.forward * 50;
			StartCoroutine(DespawnABullet(ball, 4f));
		}

		IEnumerator DespawnABullet(GameObject bullet, float delayInSeconds = 0f) { // todo: Does not complete when rider dies
			yield return new WaitForSeconds(delayInSeconds);
			SimplePool.Despawn(bullet);
		}

		bool IsTurretAimedAtTarget() {
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)) {
				var rider = hit.collider.gameObject.GetComponentInParent<Rider>();
				if (rider) {
					if (rider.gameObject.name == riderCurrentlyTargetting.gameObject.name) return true;
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
