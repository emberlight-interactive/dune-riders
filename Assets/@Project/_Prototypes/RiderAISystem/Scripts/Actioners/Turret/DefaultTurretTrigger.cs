using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.RiderAI.Actioners {
	public class DefaultTurretTrigger : TurretTrigger
	{
		[SerializeField] Rigidbody bulletPrefab;
		[SerializeField] Transform bulletSpawnPosition;
		[SerializeField] float timeToReload = 2f;
		bool reloading = false;

		public override void PullTrigger() {
			if (!reloading) {
				FireVolley();
				reloading = true;
				StartCoroutine(Reload());
			}
		}

		public override void ReleaseTrigger() {}

		void FireVolley() {
			var ball = SimplePool.Spawn(bulletPrefab.gameObject, bulletSpawnPosition.transform.position, bulletSpawnPosition.transform.rotation);
			ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
			ball.GetComponent<Rigidbody>().velocity += bulletSpawnPosition.transform.forward * 100;
			SimplePool.Despawn(ball, 4f);
		}

		IEnumerator Reload() {
			yield return new WaitForSeconds(timeToReload);
			reloading = false;
		}
	}
}
