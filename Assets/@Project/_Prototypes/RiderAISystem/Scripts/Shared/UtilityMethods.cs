using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuneRiders.RiderAI.Shared {
	public class UtilityMethods
	{
		public static float GetAngleOfTargetFromCurrentDirection(Transform current, Vector3 targetPosition) {
			var angleOfTargetFromDirectionOfTravel = Vector3.Angle(targetPosition - current.position, current.forward);
			float angle2 = Vector3.Angle((targetPosition - current.position), current.right);

			if (angle2 > 90)
			{
				angleOfTargetFromDirectionOfTravel = 360 - angleOfTargetFromDirectionOfTravel;
			}

			return angleOfTargetFromDirectionOfTravel;
		}
	}
}
