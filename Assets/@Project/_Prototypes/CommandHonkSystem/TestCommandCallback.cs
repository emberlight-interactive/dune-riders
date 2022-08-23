using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCommandCallback : MonoBehaviour
{
	[SerializeField] string testMessage;

    public void TestCallback() {
		Debug.Log(testMessage);
	}
}
