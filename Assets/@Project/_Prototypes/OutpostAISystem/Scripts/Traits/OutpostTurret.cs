using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;

namespace DuneRiders.OutpostAI.Traits {
	[DisallowMultipleComponent]
	public class OutpostTurret : MonoBehaviour
	{
		public Allegiance allegiance = Allegiance.Bandits;
		public Allegiance enemyAllegiance = Allegiance.Player;
	}
}
