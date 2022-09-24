using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Pathfinding.Util;
using Gaia;

namespace DuneRiders.RiderAICombination {
	public class FPFRichAI : RichAI
	{
		public LocalSpaceGraph graph;

		protected override void Awake() {
			graph = FindObjectOfType<LocalSpaceGraph>();
			base.Awake();
		}

		public new float remainingDistance {
			get {
				return distanceToSteeringTarget + Vector3.Distance(steeringTarget, graph.transformation.Transform(richPath.Endpoint));
			}
		}

		public new bool reachedDestination {
			get {
				if (!reachedEndOfPath) return false;
				// Note: distanceToSteeringTarget is the distance to the end of the path when approachingPathEndpoint is true
				if (approachingPathEndpoint && distanceToSteeringTarget + movementPlane.ToPlane(destination - graph.transformation.Transform(richPath.Endpoint)).magnitude > endReachedDistance) return false;

				// Don't do height checks in 2D mode
				if (orientation != OrientationMode.YAxisForward) {
					// Check if the destination is above the head of the character or far below the feet of it
					float yDifference;
					movementPlane.ToPlane(destination - position, out yDifference);
					var h = tr.localScale.y * height;
					if (yDifference > h || yDifference < -h*0.5) return false;
				}

				return true;
			}
		}

		void RefreshTransform () {
			graph.Refresh();
			richPath.transform = graph.transformation;
			movementPlane = graph.transformation;
		}

		protected override void Start () {
			RefreshTransform();
			base.Start();
		}

		protected override void CalculatePathRequestEndpoints (out Vector3 start, out Vector3 end) {
			RefreshTransform();
			base.CalculatePathRequestEndpoints(out start, out end);
			start = graph.transformation.InverseTransform(start);
			end = graph.transformation.InverseTransform(end);
		}

		protected override void Update () {
			if (!reachedDestination) {
				base.Update();
				RefreshTransform();
			}
		}
	}
}
