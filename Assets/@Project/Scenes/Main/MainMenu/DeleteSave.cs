using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders {
	public class DeleteSave : MonoBehaviour
	{
		public string SaveFileName { get; set; }

		public void DeleteSaveFile() {
			ES3.DeleteFile(SaveFileName);
		}
	}
}
