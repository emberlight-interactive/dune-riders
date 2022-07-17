using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.AI {
	public abstract class Actioner : MonoBehaviour
	{
		public abstract bool currentlyActive {get;}
		public abstract void StartAction();
		public abstract void EndAction();
	}
}
