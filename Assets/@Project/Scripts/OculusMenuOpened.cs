using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DuneRiders {
	public class OculusMenuOpened : MonoBehaviour
	{
		[SerializeField] UnityEvent menuOpenedEvent = new UnityEvent();
		[SerializeField] UnityEvent menuClosedEvent = new UnityEvent();

		void OnEnable() {
			Unity.XR.Oculus.InputFocus.InputFocusLost += MenuOpened;
			Unity.XR.Oculus.InputFocus.InputFocusAcquired += MenuClosed;
		}

		void OnDisable() {
			Unity.XR.Oculus.InputFocus.InputFocusLost -= MenuOpened;
			Unity.XR.Oculus.InputFocus.InputFocusAcquired -= MenuClosed;
		}

		void MenuOpened() { menuOpenedEvent?.Invoke(); }
		void MenuClosed() { menuClosedEvent?.Invoke(); }
	}
}
