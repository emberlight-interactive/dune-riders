using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.DrivingSystem {
	[RequireComponent(typeof(RiderDrivingControl))]
	public class EngineAudio : MonoBehaviour
	{
		[SerializeField] AudioClip acceleration;
		[SerializeField] AudioClip reversing;

		[SerializeField] AudioSource engineAudio;

		RiderDrivingControl riderDrivingControl;

		enum DrivingState { Accelerating, Idle, Reversing};
		DrivingState drivingStateCache;

		void Awake() {
			riderDrivingControl = GetComponent<RiderDrivingControl>();
		}

		void Update() {
			if (GetDrivingState() == drivingStateCache) return;
			SetAudioClip();
			StartEngineAudioSource();
			drivingStateCache = GetDrivingState();
		}

		DrivingState GetDrivingState() {
			if (riderDrivingControl.Accelerating) {
				return DrivingState.Accelerating;
			} else if (riderDrivingControl.Reversing) {
				return DrivingState.Reversing;
			} else {
				return DrivingState.Idle;
			}
		}

		void SetAudioClip() {
			switch (GetDrivingState()) {
				case DrivingState.Accelerating:
					engineAudio.clip = acceleration;
					break;
				case DrivingState.Reversing:
					engineAudio.clip = reversing;
					break;
				default:
					engineAudio.clip = null;
					break;
			}
		}

		void StartEngineAudioSource() {
			if (GetDrivingState() == DrivingState.Idle) engineAudio.Stop();
			else engineAudio.Play();
		}
	}
}
