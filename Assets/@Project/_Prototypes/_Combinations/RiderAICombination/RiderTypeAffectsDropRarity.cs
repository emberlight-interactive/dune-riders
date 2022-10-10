using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.RiderAI.Traits;
using DuneRiders.GatheringSystem;

namespace DuneRiders.RiderAICombination {
	[RequireComponent(typeof(Rider))]
	[RequireComponent(typeof(MakeItRain))]
	public class RiderTypeAffectsDropRarity : MonoBehaviour
	{
		static (Rider.ChasisType, Rider.GunType, float)[] riderRareProbabilityMultiplerMap = new (Rider.ChasisType, Rider.GunType, float)[] {
			(Rider.ChasisType.Light, Rider.GunType.MachineGun, 50),
			(Rider.ChasisType.Light, Rider.GunType.TripleMissileLauncher, 200),
			(Rider.ChasisType.Normal, Rider.GunType.MachineGun, 300),
			(Rider.ChasisType.Normal, Rider.GunType.TripleMissileLauncher, 500),
			(Rider.ChasisType.Normal, Rider.GunType.Cannon, 700),
			(Rider.ChasisType.Heavy, Rider.GunType.TripleMissileLauncher, 900),
			(Rider.ChasisType.Heavy, Rider.GunType.Cannon, 1100),
		};

		void Awake() {
			GetComponent<MakeItRain>().increaseRareProbabilityMultipler = GetRareProbabilityMultipler();
		}

		float GetRareProbabilityMultipler() {
			var rider = GetComponent<Rider>();

			return riderRareProbabilityMultiplerMap.Where((elem) => (elem.Item1 == rider.chasisType && elem.Item2 == rider.gunType)).Select((elem) => elem.Item3).First();
		}
	}
}
