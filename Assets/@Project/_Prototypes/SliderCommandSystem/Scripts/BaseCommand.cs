using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace DuneRiders.Prototype
{
	public class BaseCommand : MonoBehaviour
	{
		[BoxGroup("Components"), SerializeField] private CommandInterfaceController controller;

		public virtual void ExecuteCommand()
		{
			Debug.Log("Execute command.");
		}
	}
}
