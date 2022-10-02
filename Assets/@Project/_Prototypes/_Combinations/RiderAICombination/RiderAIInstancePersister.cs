using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.Traits;
using DuneRiders.PersistenceSystemCombination;

namespace DuneRiders.RiderAICombination {
	public class RiderAIInstancePersister : InstancePersister
	{
		protected override string PrefabNickName { get => "RiderAI"; }

		public override GameObject[] GetAllPrefabInstances() {
			return FindObjectsOfType<Rider>().Where(
				(rider) => rider.allegiance != Allegiance.Mercenary && rider.GetComponent<Player>() == null
			).Select((rider) => rider.gameObject).ToArray();
		}
	}
}
