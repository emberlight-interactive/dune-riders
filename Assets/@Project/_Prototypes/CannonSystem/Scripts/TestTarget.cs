using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.Prototype
{
	public class TestTarget : MonoBehaviour
	{
		private SpriteRenderer sr;
		private bool flashing;
		private int iterations = 3;
		private float duration = .075f;
		private Color startingColor;

		private void Start()
		{
			sr = GetComponent<SpriteRenderer>();
			startingColor = sr.color;
		}

		public void OnHit()
		{
			if (flashing == false)
				StartCoroutine(OnHitRoutine());
		}

		private IEnumerator OnHitRoutine()
		{
			flashing = true;
			for (int i = 0; i < iterations; i++)
			{
				sr.color = Color.red;
				yield return new WaitForSeconds(duration);
				sr.color = startingColor;
				yield return new WaitForSeconds(duration);
			}
			flashing = false;
		}
	}
}
