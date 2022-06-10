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

		public event Action<BaseCommand> OnCommandSelected = delegate { };

		//In this feature the "slider" refers to forward backward movement, the "lever" refers to the up down

		public void OnMin()
		{
			Debug.Log("Min reached, execute command.");

			//If in min position check what forward position the lever is in and execute that command.
		}

		public void OnMid()
		{
			Debug.Log("Mid reached, allow for level movement");

			//If in the mid position enabled the above slider to slide the handle forward and back.
		}
	}

	//Create a collider setup where a slippery physics object and move back and forth but freezes it when its not at mid position 
}
