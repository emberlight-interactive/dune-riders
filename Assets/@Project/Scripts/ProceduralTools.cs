using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using UnityEngine;
using Gaia;

namespace DuneRiders {
	public class ProceduralTools
	{
		Transform transform;
		Vector3 originalPosition;

		public ProceduralTools(Transform transform) {
			this.transform = transform;

			var fpf = GameObject.FindObjectOfType<FloatingPointFix>();
			if (fpf != null) originalPosition = fpf.ConvertToOriginalSpace(transform.position);
			else originalPosition = transform.position;
		}

		public string BuildTransformHash() {
			var transformSum = originalPosition.x + originalPosition.y + originalPosition.z;
			byte[] transformSumBytes = BitConverter.GetBytes(transformSum);

			HashAlgorithm md5 = MD5.Create();
			byte[] result = md5.ComputeHash(transformSumBytes);

			return Encoding.UTF8.GetString(result);
		}

		public int HashToRandInt(string hash, int maxRangeInclusive) {
			if (maxRangeInclusive == 0) return 0;

			var bytes = Encoding.UTF8.GetBytes(hash).Take(30);

			int number = Mathf.Abs(BitConverter.ToInt32(bytes.ToArray(), 0));

			int rand = (number % maxRangeInclusive) + 1;
			return rand;
		}

		public string HashString(string strToHash) {
			byte[] sumBytes = Encoding.Unicode.GetBytes(strToHash);

			HashAlgorithm md5 = MD5.Create();
			byte[] result = md5.ComputeHash(sumBytes);

			return Encoding.UTF8.GetString(result);
		}
	}
}
