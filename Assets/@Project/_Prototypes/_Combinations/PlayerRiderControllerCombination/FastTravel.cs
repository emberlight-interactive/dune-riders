using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Gaia;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class FastTravel : MonoBehaviour
	{
		public Transform entityToFastTravel { set; get; }
		public Transform positionToTravelTo { set; get; }
		public UnityEvent fastTravelFinished = new UnityEvent();
		TerrainScene terrainSceneToWaitFor;
		bool currentlyTeleporting = false;

		public void Teleport() {
			currentlyTeleporting = true;
			terrainSceneToWaitFor = GetCurrentTerrainScene();
			MatchTravelPositions();
		}

		TerrainScene GetCurrentTerrainScene() {
			var floatingPointFix = FindObjectOfType<FloatingPointFix>();
			foreach (TerrainScene terrainScene in TerrainLoaderManager.TerrainScenes)
			{
				if (terrainScene.m_bounds.Contains(positionToTravelTo.position - floatingPointFix.totalOffset))
				{
					return terrainScene;
				}
			}

			return null;
		}

		void MatchTravelPositions() {
			entityToFastTravel.position = positionToTravelTo.position;
			entityToFastTravel.rotation = Quaternion.Euler(new Vector3(0, positionToTravelTo.rotation.eulerAngles.y, 0));
		}

		void Update() {
			if (currentlyTeleporting && terrainSceneToWaitFor.m_regularLoadState == LoadState.Loaded) {
				currentlyTeleporting = false;
				fastTravelFinished?.Invoke();
			}
		}

		void LateUpdate() {
			if (currentlyTeleporting) MatchTravelPositions();
		}
	}
}
