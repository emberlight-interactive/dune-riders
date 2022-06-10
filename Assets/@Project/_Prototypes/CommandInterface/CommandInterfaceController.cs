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
		public void EnableSliderMovement()
		{

		}

		public void DisableSliderMovement()
		{

		}

		public void EnableLeverMovement()
		{

		}

		public void DisableLeverMovement()
		{

		}

		public void OnMin()
		{
			Debug.Log("Min reached");

			//If in min position check what forward position the lever is in and execute that command.
		}

		public void OnMid()
		{
			Debug.Log("Mid reached");

			//If in the mid position enabled the above slider to slide the handle forward and back.
		}

		public void OnMax()
		{
			Debug.Log("Max reached");
		}
	}
}
