using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;

namespace DuneRiders.OutpostAI.Traits { // todo: Add colliders to turret platforms
	[DisallowMultipleComponent]
	public class OutpostTurret : MonoBehaviour
	{
		public Allegiance allegiance = Allegiance.Bandits;
		public Allegiance enemyAllegiance = Allegiance.Player;
	}
}
