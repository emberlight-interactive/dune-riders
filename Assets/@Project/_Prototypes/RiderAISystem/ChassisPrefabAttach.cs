using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.RiderAI.Traits;

namespace DuneRiders.RiderAI {
	public class ChassisPrefabAttach : MonoBehaviour // todo: Add a global prefab attacher that recursively loads mesh renderers and child prefab attachers for context
	{
		[SerializeField] Chassis heavyChassis;
		[SerializeField] Chassis normalChassis;
		[SerializeField] Chassis lightChassis;
		Rider parentContextSource;

		void Awake() {
			InitParentContextSource();
			InstantiateChasisHere();
		}

		void InitParentContextSource() {
			parentContextSource = GetComponentInParent<Rider>();
		}

		void InstantiateChasisHere() {
			switch (parentContextSource.chasisType) {
				case Rider.ChasisType.Heavy:
					Instantiate(heavyChassis, transform);
					break;
				case Rider.ChasisType.Normal:
					Instantiate(normalChassis, transform);
					break;
				case Rider.ChasisType.Light:
					Instantiate(lightChassis, transform);
					break;
			}
		}
	}
}
