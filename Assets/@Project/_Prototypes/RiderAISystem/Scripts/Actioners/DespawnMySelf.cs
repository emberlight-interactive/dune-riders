using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;

namespace DuneRiders.RiderAI.Actioners {
	public class DespawnMySelf : Actioner
	{
		public override bool currentlyActive {get => false;}

		public override void StartAction()
		{
			if (SimplePool.IsGameObjectFromPool(gameObject)) {
				SimplePool.Despawn(gameObject);
			} else {
				gameObject.SetActive(false);
			}
		}

		public override void EndAction() {}

	}
}
