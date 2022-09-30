using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DuneRiders.PlayerRiderControllerCombination {
	public class HandleDeath : MonoBehaviour
	{
		[SerializeField] float fadeOutTime = 4f;
		[SerializeField] ParticleSystem smoke;
		[SerializeField] Material burntMetalMateral;
		[SerializeField] List<MeshRenderer> riderMeshes = new List<MeshRenderer>();
		[SerializeField] List<OVRScreenFade> faders = new List<OVRScreenFade>();
		[SerializeField] List<WheelCollider> wheelColliders = new List<WheelCollider>();


		public void BurnMetal() {
			foreach (var riderMesh in riderMeshes) {
				riderMesh.material = burntMetalMateral;
			}
		}

		public void SlowDownRider() {
			foreach (var wheel in wheelColliders) {
				wheel.brakeTorque = 2000f;
			}
		}

		public void StartSmoke() {
			smoke.gameObject.SetActive(true);
		}

		public void FadeOutCamera() {
			foreach (var fader in faders) {
				fader.fadeTime = fadeOutTime;
				fader.FadeOut();
			}
		}

		public void ReloadScene() {
			StartCoroutine(ReloadSceneRoutine());
		}

		IEnumerator ReloadSceneRoutine() {
			yield return new WaitForSeconds(5f);
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}
}
