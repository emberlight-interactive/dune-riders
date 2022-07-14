using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAI.Actioners {
	public class Turret : MonoBehaviour
	{
		[SerializeField] Rigidbody bulletPrefab;
		[SerializeField] Transform bulletSpawnPosition;
		Transform riderCurrentlyTargetting;
		public float turretTurnSpeed = 1;
		Quaternion originalRotation;

		void Update()
		{
			if (riderCurrentlyTargetting && riderCurrentlyTargetting.gameObject.activeSelf) {
				IncrementTurretBarrelTowardsTarget(riderCurrentlyTargetting);
			} else {
				ReturnTurretToDefaultPosition();
			}
		}

		public void FireOnTarget(Transform rider, float yOffset = 0f) {
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
			rotationInEulerAngles.z = 0;

			rotation.eulerAngles = rotationInEulerAngles;

			transform.rotation = rotation;

			var localRotation = transform.localRotation.eulerAngles;
			if (localRotation.y < 270 && localRotation.y > 180) {
				localRotation.y = 270;
			} else if (localRotation.y > 90 && localRotation.y < 180) {
				localRotation.y = 90;
			}

			if (localRotation.x < 310 && localRotation.x > 180) {
				localRotation.x = 310;
			} else if (localRotation.x > 0 && localRotation.x < 180) {
				localRotation.x = 0;
			}

			var newLocalRotation = new Quaternion();
			newLocalRotation.eulerAngles = localRotation;
			transform.localRotation = newLocalRotation;
		}

		void FireVolley() {
			var ball = SimplePool.Spawn(bulletPrefab.gameObject, bulletSpawnPosition.transform.position, bulletSpawnPosition.transform.rotation);
			ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
			ball.GetComponent<Rigidbody>().velocity += bulletSpawnPosition.transform.forward * 50;
			(Camera.main.gameObject.GetComponent<CoroutineParasite>() ?? Camera.main.gameObject.AddComponent<CoroutineParasite>()).StartCoroutine(DespawnABullet(ball, 4f));
		}

		IEnumerator DespawnABullet(GameObject bullet, float delayInSeconds = 0f) {
			yield return new WaitForSeconds(delayInSeconds);
			SimplePool.Despawn(bullet);
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
