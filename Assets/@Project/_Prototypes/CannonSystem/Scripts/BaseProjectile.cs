using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DuneRiders.Prototype
{
	public class BaseProjectile : MonoBehaviour
	{
		[BoxGroup("Variables"), SerializeField] private float damage;
	}
}
