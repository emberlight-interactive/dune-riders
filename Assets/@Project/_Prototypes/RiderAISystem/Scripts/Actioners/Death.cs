using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DuneRiders.AI;

namespace DuneRiders.RiderAI.Actioners {
	public class Death : Actioner
	{
		public UnityEvent deathEvent = new UnityEvent();

		public override bool currentlyActive {get => false;}

		public override void StartAction()
		{
			if (SimplePool.IsGameObjectFromPool(gameObject)) {
				SimplePool.Despawn(gameObject);
			} else {
				gameObject.SetActive(false);
			}

			deathEvent?.Invoke();
		}

		public override void EndAction() {}

	}
}
