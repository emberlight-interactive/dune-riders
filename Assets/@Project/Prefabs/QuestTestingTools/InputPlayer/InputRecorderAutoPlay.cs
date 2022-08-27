using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace DuneRiders.Shared {
	[RequireComponent(typeof(InputRecorder))]
	public class InputRecorderAutoPlay : MonoBehaviour
	{
		InputRecorder recorder;
		public string recorderTracePath;
		[FormerlySerializedAs("playOnAwake")]
		public bool playOnStart = false;
		bool started = false;

		void Reset() {
			recorder = GetComponent<InputRecorder>();
			recorder.replayOnNewDevices = true;
			recorder.simulateOriginalTimingOnReplay = true;
			recorder.startRecordingWhenEnabled = false;
		}

		void FixedUpdate() {
			if (playOnStart && !started) {
				recorder = GetComponent<InputRecorder>();
				recorder.LoadCaptureFromFile(recorderTracePath);
				recorder.StartReplay();

				StartCoroutine("SetUpDevices");

				started = true;
			}
		}

		IEnumerator SetUpDevices() {
			yield return new WaitForSeconds(0.05f);

			var rightInputDevice = InputSystem.GetDevice("OculusTouchControllerRight");
			InputSystem.SetDeviceUsage(rightInputDevice, "RightHand");

			var leftInputDevice = InputSystem.GetDevice("OculusTouchControllerLeft");
			InputSystem.SetDeviceUsage(leftInputDevice, "LeftHand");
		}
	}
}
