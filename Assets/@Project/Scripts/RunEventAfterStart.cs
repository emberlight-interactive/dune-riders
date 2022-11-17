using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DuneRiders {
	public class RunEventAfterStart : MonoBehaviour
	{
		[SerializeField] float timeToWait = 4f;
		[SerializeField] UnityEvent delayedEvent;

		void Start() {
			Invoke(nameof(RunEvent), timeToWait);
		}

		void RunEvent() { delayedEvent?.Invoke(); }
	}
}
