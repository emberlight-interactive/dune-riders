using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gaia;

namespace DuneRiders.Combinations {
	public class TransformPersistenceHelper : MonoBehaviour
	{
		FloatingPointFix floatingPointFix;

		public Vector3 position {
			get {
				var pos = transform.position;
				return floatingPointFix.ConvertToOriginalSpace(pos);
			}

			set { transform.position = value; }
		}

		public Quaternion rotation {
			get { return transform.rotation; }
			set { transform.rotation = value; }
		}

		void Awake() {
			floatingPointFix = FindObjectOfType<FloatingPointFix>();
		}
	}
}
