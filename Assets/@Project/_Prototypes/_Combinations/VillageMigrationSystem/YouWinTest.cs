using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.VillageMigrationSystem {
	public class YouWinTest : MonoBehaviour
	{
		public void Win() {
			Debug.Log("YOU WIN");
			gameObject.SetActive(false);
		}
	}
}
