using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAI.State {
	[DisallowMultipleComponent]
	public class RiderSpeedState : MonoBehaviour
	{
		public float traverseSpeed;

		public float heavyChasisChargeSpeed;
		public float heavyChasisChargeAcceleration;

		public float normalChasisChargeSpeed;
		public float normalChasisChargeAcceleration;

		public float lightChasisChargeSpeed;
		public float lightChasisChargeAcceleration;

		public (float speed, float acceleration) GetRiderSpeed(Rider.ChasisType chasisType) {
			switch (chasisType) {
				case Rider.ChasisType.Heavy:
					return (heavyChasisChargeSpeed, heavyChasisChargeAcceleration);
				case Rider.ChasisType.Normal:
					return (normalChasisChargeSpeed, normalChasisChargeAcceleration);
				case Rider.ChasisType.Light:
					return (lightChasisChargeSpeed, lightChasisChargeAcceleration);
				default:
					return (lightChasisChargeSpeed, lightChasisChargeAcceleration);
			}
		}
	}
}
