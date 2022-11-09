using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.Shared.Sound {
	public class SoundPlayer : MonoBehaviour
	{
		[SerializeField] AudioSource soundAudioSource;
		[SerializeField] AudioClip sound;

		public void Play() {
			var audioSourceInstance = SimplePool.Spawn(soundAudioSource.gameObject, Vector3.zero, Quaternion.identity).GetComponent<AudioSource>();
			audioSourceInstance.clip = sound;
			audioSourceInstance.Play();
			SimplePool.Despawn(audioSourceInstance.gameObject, sound.length);
		}
	}
}
