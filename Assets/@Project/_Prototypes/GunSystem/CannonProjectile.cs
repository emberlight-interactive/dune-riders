using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.GunSystem {
	[RequireComponent(typeof(Rigidbody))]
	public class CannonProjectile : MonoBehaviour
	{
		[BoxGroup("Debug"), SerializeField] private Color debugExplosiveRadiusColor = Color.red;

		[BoxGroup("Projectile Stats"), SerializeField] private int initialForce = 100;
		[BoxGroup("Projectile Stats"), SerializeField] private int explosiveRadius = 3;
		[BoxGroup("Projectile Stats"), SerializeField] private int directHitDamage = 20;

		[BoxGroup("Projectile Effects"), SerializeField] private ParticleSystem explosionParticle;
		[BoxGroup("Projectile Effects"), SerializeField] private float particleScale = 2;

		Rigidbody rb;

		void Awake() {
			rb = GetComponent<Rigidbody>();
		}

		void OnEnable() {
			rb.velocity += transform.forward * initialForce;
		}

		void OnDisable() {
			rb.velocity = new Vector3(0, 0, 0);
		}

		private void OnCollisionEnter(Collision c)
		{
			Explode(c);
		}

		#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Gizmos.color = debugExplosiveRadiusColor;
			Gizmos.DrawSphere(transform.position, explosiveRadius);
		}
		#endif

		private void Explode(Collision collision)
		{
			var collisionObjectDamagable = collision.gameObject.GetComponent<Damageable>();
			if (collisionObjectDamagable != null) {
				collisionObjectDamagable.Damage(directHitDamage);
			}

			Collider[] hits = Physics.OverlapSphere(transform.position, explosiveRadius);

			for (int i = 0; i < hits.Length; i++) {
				if (GameObject.ReferenceEquals(collision.gameObject, hits[i].gameObject)) continue;

				if (hits[i].GetComponent<Damageable>() != null) {
					var hitDistance = Vector3.Distance(transform.position, hits[i].transform.position);
					hits[i].GetComponent<Damageable>().Damage(Mathf.CeilToInt(directHitDamage / (hitDistance < 1 ? 1 : hitDistance)));
				}
			}

			var p = SimplePool.Spawn(explosionParticle.gameObject, transform.position, Quaternion.identity); // todo: Add that despawner to the simple pool
			p.transform.localScale = Vector3.one * particleScale;

			SimplePool.Despawn(gameObject);
		}
	}
}
