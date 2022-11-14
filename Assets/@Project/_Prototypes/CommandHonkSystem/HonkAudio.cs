using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.CommandHonkSystem {
	[RequireComponent(typeof(AudioSource))]
	public class HonkAudio : MonoBehaviour
	{
		[SerializeField] AudioClip chargeHonk;
		[SerializeField] AudioClip followHonk;
		[SerializeField] AudioClip haltHonk;
		[SerializeField] AudioClip retreatHonk;

		AudioSource audioSource;

		void Awake() {
			audioSource = GetComponent<AudioSource>();
		}

		public void PlayChargeHonk() { PlayHonk(chargeHonk); }
		public void PlayFollowHonk() { PlayHonk(followHonk); }
		public void PlayHaltHonk() { PlayHonk(haltHonk); }
		public void PlayRetreatHonk() { PlayHonk(retreatHonk); }

		void PlayHonk(AudioClip audioClip) {
			audioSource.Stop();
			audioSource.clip = audioClip;
			audioSource.Play();
		}
	}
}
