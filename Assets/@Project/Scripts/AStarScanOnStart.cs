using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders {
	[RequireComponent(typeof(AstarPath))]
	public class AStarScanOnStart : MonoBehaviour
	{
		void Start()
		{
			AstarPath.active.Scan();
		}
}
}
