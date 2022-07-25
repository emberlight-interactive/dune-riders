using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEvent : MonoBehaviour
{
	[SerializeField] string logMessage;

    public void TestMethod() {
		Debug.Log(logMessage);
	}
}
