using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

namespace DuneRiders.CommandHonkSystem {
	[RequireComponent(typeof(SphereCollider))]
	public class HonkCommand : MonoBehaviour
	{
		SphereCollider honkArea;

		[SerializeField] CommandSelector commandSelector;

		private void Start()
		{
			honkArea = GetComponent<SphereCollider>();
		}

		private void OnTriggerEnter(Collider c)
		{
			if (c.GetComponent<Hand>() != null)
			{
				commandSelector.CurrentCommand.commandFunction.Invoke();
			}
		}
	}
}
