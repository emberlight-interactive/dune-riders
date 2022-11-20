using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DuneRiders.PauseSystem {
	public class Pause : MonoBehaviour
	{
		[SerializeField] InputActionProperty pauseInput;
		public UnityEvent pauseEvents = new UnityEvent();
		public UnityEvent unpauseEvents = new UnityEvent();

		bool paused = false;

		void Start() {
			pauseInput.action.Enable();

			pauseInput.action.performed += context =>
			{
				if (paused) {
					UnPauseGame();
				} else {
					PauseGame();
				}
			};
		}

		public void PauseGame() {
			pauseEvents.Invoke();
			paused = true;
		}

		public void UnPauseGame() {
			unpauseEvents.Invoke();
			paused = false;
		}
	}
}
