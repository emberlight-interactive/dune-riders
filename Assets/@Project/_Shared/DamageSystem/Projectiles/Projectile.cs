using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.Shared.Sound;

namespace DuneRiders.Shared.DamageSystem {
	public class Projectile : MonoBehaviour
	{
		public DamageableCheck damageableCheck;
		public float accuracyVariance = 0f;
		public bool playHitMarkerAudio = false;
		public SoundPlayer directHitMarkerSoundPlayer;
		public SoundPlayer radiusHitMarkerSoundPlayer;

		protected bool CanDamage(Damageable damageable) {
			if (damageableCheck == null) return true;
			return damageableCheck.CanDamage(damageable);
		}

		protected void ApplyRandomRotation() {
			if (accuracyVariance == 0) return;

			var variance = Random.Range(-accuracyVariance, accuracyVariance);
			var rotation = Vector3.one * variance;
			transform.Rotate(rotation);
		}
	}
}
