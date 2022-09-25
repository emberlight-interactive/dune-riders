using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.MercenaryHiringSystem {
	public class MercenarySpotGlobalStatePersistenceComponentProvider : ComponentTypeProvider
	{
		public override Type Component {
			get => typeof(MercenarySpotGlobalStatePersistence);
		}
	}
}
