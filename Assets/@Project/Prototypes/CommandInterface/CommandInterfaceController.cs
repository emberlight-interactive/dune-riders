using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DuneRiders.Prototype
{
	public class CommandInterfaceController : MonoBehaviour
	{
		public event Action<BaseCommand> OnCommandSelected = delegate { };
	}
}
