using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.Shared.DamageSystem {
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(AudioSource))]
	public class CannonProjectile : Projectile
	{
		[BoxGroup("Debug"), SerializeField] private Color debugExplosiveRadiusColor = Color.red;

		[BoxGroup("Projectile Stats"), SerializeField] private float initialForce = 100;
		[BoxGroup("Projectile Stats"), SerializeField] private float explosiveRadius = 3;
		[BoxGroup("Projectile Stats"), SerializeField] private float directHitDamage = 20;

		[BoxGroup("Projectile Effects"), SerializeField] private ParticleSystem launchParticle;
		[BoxGroup("Projectile Effects"), SerializeField] private float launchParticleScale = 2;

		[BoxGroup("Projectile Effects"), SerializeField] private ParticleSystem explosionParticle;
		[BoxGroup("Projectile Effects"), SerializeField] private float explosionParticleScale = 2;

		[BoxGroup("Audio"), SerializeField] private AudioClip explosionNoise;
		[BoxGroup("Audio"), SerializeField] private AudioClip launchNoise;

		[SerializeField] float maxProjectileDistance = 0f;

		Rigidbody rb;
		AudioSource audioSource;

		void Awake() {
			rb = GetComponent<Rigidbody>();
			audioSource = GetComponent<AudioSource>();
		}

		void OnEnable() {
			ApplyRandomRotation();
			rb.velocity += transform.forward * initialForce;
			SpawnLaunchParticles();
			StartMaxDistanceCheck();
			PlayLaunchAudio();
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
			if (collisionObjectDamagable != null && CanDamage(collisionObjectDamagable)) {
				collisionObjectDamagable.Damage(directHitDamage);
				if (playHitMarkerAudio) directHitMarkerSoundPlayer.Play();
			}

			Collider[] hits = Physics.OverlapSphere(transform.position, explosiveRadius);

			for (int i = 0; i < hits.Length; i++) {
				if (GameObject.ReferenceEquals(collision.gameObject, hits[i].gameObject)) continue;

				var damageableComponent = hits[i].GetComponent<Damageable>();
				if (damageableComponent != null) {
					var hitDistance = Vector3.Distance(transform.position, hits[i].transform.position);
					if (CanDamage(damageableComponent)) {
						damageableComponent.Damage(Mathf.CeilToInt(directHitDamage / (hitDistance < 1 ? 1 : hitDistance)));
						if (playHitMarkerAudio) radiusHitMarkerSoundPlayer.Play();
					}
				}
			}

			SpawnExplosionParticles();
			PlayExplosionAudio();
			SimplePool.Despawn(gameObject);
		}

		void SpawnExplosionParticles() {
			var p = SimplePool.Spawn(explosionParticle.gameObject, transform.position, Quaternion.identity);
			p.transform.localScale = Vector3.one * explosionParticleScale;
			SimplePool.Despawn(p, p.GetComponent<ParticleSystem>().main.duration);
		}

		void SpawnLaunchParticles() {
			var p = SimplePool.Spawn(launchParticle.gameObject, transform.position, transform.rotation);
			p.transform.localScale = Vector3.one * launchParticleScale;
			SimplePool.Despawn(p, p.GetComponent<ParticleSystem>().main.duration);
		}

		void PlayLaunchAudio() {
			AudioSource.PlayClipAtPoint(launchNoise, transform.position);
		}

		void PlayExplosionAudio() {
			AudioSource.PlayClipAtPoint(explosionNoise, transform.position);
		}

		void StartMaxDistanceCheck() {
			if (maxProjectileDistance == 0) return;
			Invoke(nameof(DisableProjectileAfterMaxDistance), maxProjectileDistance / initialForce);
		}

		void DisableProjectileAfterMaxDistance() {
			SimplePool.Despawn(gameObject);
		}
	}
}
