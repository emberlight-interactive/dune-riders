using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Pathfinding;
using Gaia;

namespace DuneRiders {
	[RequireComponent(typeof(AstarPath))]
	[RequireComponent(typeof(FloatingPointFixMember))]
	public class AStarGraphFloatingPointFix : MonoBehaviour
	{
		Vector3 currentPositionOffset;
		ShiftGraphsJob shiftGraphsJob;

		struct ShiftGraphsJob : IJob
		{
			public Vector3 currentPositionOffset;

			public void Execute()
			{
				RecastGraph graph = AstarPath.active.data.recastGraph;
				graph.forcedBoundsCenter = currentPositionOffset;
				var transform = graph.CalculateTransform();
				graph.RelocateNodes(transform);
			}
		}

		void Awake() {
			if (FindObjectOfType<FloatingPointFix>() == null) enabled = false;

			transform.position = new Vector3(0, 0, 0);
			currentPositionOffset = transform.position;
		}

		void Update()
		{
			if (transform.position != currentPositionOffset) {
				currentPositionOffset = transform.position;
				ShiftGraphs();
			}
		}

		void ShiftGraphs() {
			shiftGraphsJob = new ShiftGraphsJob() {
				currentPositionOffset = currentPositionOffset,
			};

			shiftGraphsJob.Schedule();
		}
	}
}
