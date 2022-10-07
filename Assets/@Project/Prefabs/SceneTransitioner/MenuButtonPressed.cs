using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace DuneRiders {
	public class MenuButtonPressed : MonoBehaviour
	{
		[SerializeField] InputActionProperty pauseInput;
		public UnityEvent menuPressed = new UnityEvent();

		void Start() {
			pauseInput.action.Enable();

			pauseInput.action.performed += context =>
			{
				menuPressed.Invoke();
			};
		}
	}
}
