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

		private Rigidbody rb;

		private void Start()
		{
			rb = GetComponent<Rigidbody>();
			rb.AddForce(transform.position - transform.right * initialForce, ForceMode.VelocityChange);
		}

		private void Explode()
		{
			Collider[] hits = Physics.OverlapSphere(transform.position, explosiveRadius);
			for (int i = 0; i < hits.Length; i++)
				if (hits[i].GetComponent<TestTarget>() != null)
					hits[i].GetComponent<TestTarget>().OnHit();

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
