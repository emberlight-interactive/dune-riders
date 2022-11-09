using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.RiderAI.Actioners {
	public abstract class TurretTrigger : MonoBehaviour
	{
		public abstract void PullTrigger();
		public abstract void ReleaseTrigger();
	}
}
