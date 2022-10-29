using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.Shared.DamageSystem {
	[RequireComponent(typeof(AudioSource))]
	public class MachineGunProjectile : Projectile
	{
		[SerializeField] int directHitDamage = 2;

		[BoxGroup("Projectile Effects"), SerializeField] private ParticleSystem launchParticle;
		[BoxGroup("Projectile Effects"), SerializeField] private float launchParticleScale = 2;
		[BoxGroup("Projectile Effects"), SerializeField] private ParticleSystem landParticle;
		[BoxGroup("Projectile Effects"), SerializeField] private float landParticleScale = 2;

		[BoxGroup("Audio"), SerializeField] private AudioClip explosionNoise;
		[BoxGroup("Audio"), SerializeField] private AudioClip launchNoise;

		AudioSource audioSource;

		void Awake() {
			audioSource = GetComponent<AudioSource>();
		}

		void OnEnable()
		{
			ApplyRandomRotation();
			FireBullet();
			SpawnLaunchParticles();
			PlayLaunchAudio();
		}

		void FireBullet() {
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity)) {
				#if UNITY_EDITOR
					Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
				#endif

				RegisterDamageOnObjectIfDamageable(hit);
				SpawnLandParticle(hit.point);
				PlayExplosionAudio(hit.point);
			}
		}

		void RegisterDamageOnObjectIfDamageable(RaycastHit hit) {
			var damageableComponent = hit.collider.gameObject.GetComponent<Damageable>();
			if (damageableComponent != null) {
				if (CanDamage(damageableComponent)) damageableComponent.Damage(directHitDamage);
			}
		}

		void SpawnLaunchParticles() {
			var p = SimplePool.Spawn(launchParticle.gameObject, transform.position, transform.rotation);
			p.transform.localScale = Vector3.one * launchParticleScale;
			SimplePool.Despawn(p, p.GetComponent<ParticleSystem>().main.duration);
		}

		void SpawnLandParticle(Vector3 position) {
			var p = SimplePool.Spawn(landParticle.gameObject, position, Quaternion.identity);
			p.transform.localScale = Vector3.one * landParticleScale;
			SimplePool.Despawn(p, p.GetComponent<ParticleSystem>().main.duration);
		}

		void PlayLaunchAudio() {
			AudioSource.PlayClipAtPoint(launchNoise, transform.position);
		}

		void PlayExplosionAudio(Vector3 position) {
			AudioSource.PlayClipAtPoint(explosionNoise, position);
		}
	}
}
