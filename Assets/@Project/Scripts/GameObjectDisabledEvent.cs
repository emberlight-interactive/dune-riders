using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DuneRiders {
	public class GameObjectDisabledEvent : MonoBehaviour
	{
		public UnityEvent onDisableEvent = new UnityEvent();

		void OnDisable() { onDisableEvent?.Invoke(); }
	}
}
