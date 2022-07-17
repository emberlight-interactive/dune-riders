using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace DuneRiders.Prototype
{
	public class CommandInterfaceController : MonoBehaviour
	{
		[SerializeField] private Rigidbody leverRB;

		private ConfigurableJoint joint;

		public event Action<BaseCommand> OnCommandSelected = delegate { };

		//In this feature the "slider" refers to forward backward movement, the "lever" refers to the up down

		public void OnMin()
		{
			Debug.Log("Min reached, execute command.");

			//TODO Figure out how to trigger these when the handle is in the correct positions.

			//If in min position check what forward position the lever is in and execute that command.
		}

		public void OnMid()
		{
			Debug.Log("Mid reached, allow for level movement");

			//If in the mid position enabled the above slider to slide the handle forward and back.
		}
	}
}