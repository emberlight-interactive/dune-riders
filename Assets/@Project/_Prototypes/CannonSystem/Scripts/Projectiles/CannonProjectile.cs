using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.Prototype
{
	public class CannonProjectile : BaseProjectile
	{
		[BoxGroup("Debug"), SerializeField] private bool isDebug = true;
		[BoxGroup("Debug"), ShowIf("isDebug", true), SerializeField] private Color explosiveRadiusColor = Color.red;

		[BoxGroup("Projectile Stats"), SerializeField] private int initialForce = 100;
		[BoxGroup("Projectile Stats"), SerializeField] private int explosiveRadius = 3;

		[BoxGroup("Projectile Effects"), SerializeField] private ParticleSystem explosionParticle;
		[BoxGroup("Projectile Effects"), SerializeField] private float particleScale = 2;

		private Rigidbody rb;

		private void Start()
		{
			rb = GetComponent<Rigidbody>();
			rb.velocity += transform.forward * initialForce;
		}

		private void Explode()
		{
			Collider[] hits = Physics.OverlapSphere(transform.position, explosiveRadius);
			for (int i = 0; i < hits.Length; i++)
				if (hits[i].GetComponent<TestTarget>() != null)
					hits[i].GetComponent<TestTarget>().OnHit();

			var p = Instantiate(explosionParticle, transform.position, Quaternion.identity);
			p.transform.localScale = Vector3.one * particleScale;

			Destroy(gameObject);
		}

		private void OnCollisionEnter(Collision c)
		{
			Explode();
		}

		private void OnDrawGizmos()
		{
			if (isDebug == false)
				return;

			Gizmos.color = explosiveRadiusColor;
			Gizmos.DrawSphere(transform.position, explosiveRadius);
		}
	}
}
