using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.RiderAI {
	[RequireComponent(typeof(AudioSource))]
	public class IBrrrWhenIMoveLOL : MonoBehaviour
	{
		[SerializeField] AudioClip acceleration;
		[SerializeField] AudioClip idle;

		AudioSource engineAudioSource;
		Vector3 currPos = Vector3.zero;

		enum MovementState {Moving, Idle};
		MovementState movementStateCache;

		void Awake() {
			engineAudioSource = GetComponent<AudioSource>();
			movementStateCache = MovementState.Idle;
			engineAudioSource.clip = idle;
			engineAudioSource.Play();
		}

		void Update() {
			MovementState movementState = MovementState.Idle;
			if (transform.position != currPos) {
				movementState = MovementState.Moving;
				currPos = transform.position;
			}

			if (movementStateCache == movementState) return;

			if (movementState == MovementState.Moving) {
				engineAudioSource.clip = acceleration;
				engineAudioSource.Play();
			} else {
				engineAudioSource.clip = idle;
				engineAudioSource.Play();
			}
		}
	}
}
