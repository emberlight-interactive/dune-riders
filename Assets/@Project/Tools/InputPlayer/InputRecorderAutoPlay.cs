using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.Oculus;

[RequireComponent(typeof(InputRecorder))]
public class InputRecorderAutoPlay : MonoBehaviour
{
    InputRecorder recorder;
    public string recorderTracePath;
    public bool playOnAwake = false;

	void Reset() {
		recorder = GetComponent<InputRecorder>();
		recorder.replayOnNewDevices = true;
		recorder.simulateOriginalTimingOnReplay = true;
		recorder.startRecordingWhenEnabled = false;
	}

    void Awake() {
        if (playOnAwake) {
            recorder = GetComponent<InputRecorder>();
            recorder.LoadCaptureFromFile(recorderTracePath);
            recorder.StartReplay();

            StartCoroutine("SetUpDevices");
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
