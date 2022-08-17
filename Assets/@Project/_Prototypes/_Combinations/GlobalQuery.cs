using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.Combinations {
	public class GlobalQuery : MonoBehaviour
	{
		public static Rider[] GetAllCompanyRiders() {
			var allRiders = FindObjectsOfType<Rider>();

			return allRiders.Where((rider) => {
				if (rider.allegiance == AI.Allegiance.Player && rider.GetComponent<Player>() == null) return true;
				return false;
			}).ToArray();
		}
	}
}
