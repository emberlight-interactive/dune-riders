using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using UnityEditor;
using DuneRiders.Shared;

namespace DuneRiders.RiderAI.Actioners {
	[RequireComponent(typeof(BaseBarrelRotation))]
	public class Turret : MonoBehaviour
	{
		[SerializeField] TurretTrigger trigger;
		[SerializeField] float weaponRange = 350;
		BaseBarrelRotation baseBarrelRotation;
		Transform riderCurrentlyTargetting;
		Transform aimGuider;

		void Awake() {
			baseBarrelRotation = GetComponent<BaseBarrelRotation>();
		}

		void Update()
		{
			if (riderCurrentlyTargetting && riderCurrentlyTargetting.gameObject.activeSelf) {
				Transform target;

				if (aimGuider) target = aimGuider;
				else target = riderCurrentlyTargetting;

				baseBarrelRotation.target = target;
			} else {
				baseBarrelRotation.target = null;
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

		void OnEnable() {
			StartCoroutine(Gunner());
		}

		void OnDisable() {
			StopAllCoroutines();
		}

		IEnumerator Gunner() {
			while (true) {
				if (riderCurrentlyTargetting && IsTurretAimedAtTarget() && IsTargetWithinWeaponRange()) {
					trigger.PullTrigger();
				} else {
					trigger.ReleaseTrigger();
				}

				yield return null;
			}
		}

		#if UNITY_EDITOR
		void OnDrawGizmos() {
			GUIStyle style = new GUIStyle();

			var labelPosition = transform.position;

			Handles.color = Color.black;
        	style.normal.textColor = Color.black;
			labelPosition.y += 1f;
			Handles.Label(labelPosition, "Weapon Range", style);
        	Handles.DrawWireDisc(transform.position, new Vector3(0, 1, 0), weaponRange);
		}
		#endif

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

		bool IsTargetWithinWeaponRange() {
			return (Vector3.Distance(transform.position, riderCurrentlyTargetting.position) <= weaponRange);
		}
	}
}
