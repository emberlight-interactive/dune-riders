using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders {
	abstract public class ComponentTypeProvider : MonoBehaviour
	{
		public abstract Type Component { get; }
	}
}
