using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DuneRiders.CommandHonkSystem {
	public class Command : MonoBehaviour
	{
		[SerializeField] public UnityEvent commandFunction;
		[SerializeField] public string commandName;
	}
}
