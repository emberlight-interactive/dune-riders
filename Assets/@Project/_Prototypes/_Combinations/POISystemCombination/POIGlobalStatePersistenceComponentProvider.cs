using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.POISystemCombination {
	public class POIGlobalStatePersistenceComponentProvider : ComponentTypeProvider
	{
		public override Type Component {
			get => typeof(POIGlobalStatePersistence);
		}
	}
}
