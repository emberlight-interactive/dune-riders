using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.Shared.DamageSystem {
	[RequireComponent(typeof(Rigidbody))]
	public class CannonProjectile : MonoBehaviour
	{
		[BoxGroup("Debug"), SerializeField] private Color debugExplosiveRadiusColor = Color.red;

		[BoxGroup("Projectile Stats"), SerializeField] private int initialForce = 100;
		[BoxGroup("Projectile Stats"), SerializeField] private int explosiveRadius = 3;
		[BoxGroup("Projectile Stats"), SerializeField] private int directHitDamage = 20;

		[BoxGroup("Projectile Effects"), SerializeField] private ParticleSystem launchParticle;
		[BoxGroup("Projectile Effects"), SerializeField] private float launchParticleScale = 2;

		[BoxGroup("Projectile Effects"), SerializeField] private ParticleSystem explosionParticle;
		[BoxGroup("Projectile Effects"), SerializeField] private float explosionParticleScale = 2;


		Rigidbody rb;

		void Awake() {
			rb = GetComponent<Rigidbody>();
		}

		void OnEnable() {
			rb.velocity += transform.forward * initialForce;
			SpawnLaunchParticles();
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

			SpawnExplosionParticles();
			SimplePool.Despawn(gameObject);
		}

		void SpawnExplosionParticles() {
			var p = SimplePool.Spawn(explosionParticle.gameObject, transform.position, Quaternion.identity);
			p.transform.localScale = Vector3.one * explosionParticleScale;
			SimplePool.Despawn(p, p.GetComponent<ParticleSystem>().main.duration);
		}

		void SpawnLaunchParticles() {
			var p = SimplePool.Spawn(launchParticle.gameObject, transform.position, Quaternion.identity);
			p.transform.localScale = Vector3.one * launchParticleScale;
			SimplePool.Despawn(p, p.GetComponent<ParticleSystem>().main.duration);
		}
	}
}
