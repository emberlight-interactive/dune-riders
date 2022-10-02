using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuneRiders.AI;
using DuneRiders.Shared.PersistenceSystem;

namespace DuneRiders.RiderAI.Traits {
	[RequireComponent(typeof(PrefabInstanceTag))]
	[DisallowMultipleComponent]
	public class Rider : MonoBehaviour, IPersistent
	{
		[Serializable]
		class RiderSerializable {
			public Allegiance allegiance;
			public Allegiance enemyAllegiance;
			public ChasisType chasisType;
			public GunType gunType;
		}

		public enum ChasisType {
			Light,
			Normal,
			Heavy
		}

		public enum GunType {
			MachineGun,
			TripleMissileLauncher,
			Cannon
		}

		[SerializeField] bool disablePersistence = false;
		public bool DisablePersistence { get => disablePersistence; }

		public Allegiance allegiance = Allegiance.Mercenary;
		public Allegiance enemyAllegiance = Allegiance.Bandits;
		public ChasisType chasisType = ChasisType.Heavy;
		public GunType gunType = GunType.Cannon;

		public void CopyFields(Rider otherRider) {
			allegiance = otherRider.allegiance;
			enemyAllegiance = otherRider.enemyAllegiance;
			chasisType = otherRider.chasisType;
			gunType = otherRider.gunType;
		}

		public void Save(IPersistenceUtil persistUtil) {
			persistUtil.Save(PersistenceKey(), new RiderSerializable {
				allegiance = this.allegiance,
				enemyAllegiance = this.enemyAllegiance,
				chasisType = this.chasisType,
				gunType = this.gunType,
			});
		}

        public void Load(IPersistenceUtil persistUtil) {
			var loadedRider = persistUtil.Load<RiderSerializable>(PersistenceKey());
			allegiance = loadedRider.allegiance;
			enemyAllegiance = loadedRider.enemyAllegiance;
			chasisType = loadedRider.chasisType;
			gunType = loadedRider.gunType;
		}

		string PersistenceKey() {
			return $"Rider-{GetComponent<PrefabInstanceTag>().prefabInstanceKey}";
		}
	}
}
