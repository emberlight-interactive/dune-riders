using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.GunSystem {
	[RequireComponent(typeof(LineRenderer))]
	public class MachineGunProjectile : MonoBehaviour
	{
		LineRenderer bulletLine;
		[SerializeField] int directHitDamage = 2;

		void Awake() {
			bulletLine = GetComponent<LineRenderer>();
		}

		void OnEnable()
		{
			FireBullet(); // todo: "Particles" for the hit point could be a single 2D image since the profiles will be hyper ephemeral
		}

		void OnDisable() {
			ResetBullet();
		}

		void FireBullet() {
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity)) {
				#if UNITY_EDITOR
					Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
				#endif

				RegisterDamageOnObjectIfDamageable(hit);

				bulletLine.positionCount = 2;
				bulletLine.SetPosition(0, transform.position);
				bulletLine.SetPosition(1, hit.point);
				bulletLine.enabled = true;
			}
		}

		void RegisterDamageOnObjectIfDamageable(RaycastHit hit) {
			if (hit.collider.gameObject.GetComponent<Damageable>()) {
				hit.collider.gameObject.GetComponent<Damageable>().Damage(directHitDamage);
			}
		}

		void ResetBullet() {
			bulletLine.positionCount = 0;
			bulletLine.enabled = false;
		}
	}
}
