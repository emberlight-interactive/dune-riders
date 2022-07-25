using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RunEventOnTriggerEnter : MonoBehaviour
{
    [SerializeField] UnityEvent eventToRun;
    [SerializeField] GameObject otherGameObject;

	void OnTriggerEnter(Collider other)
    {
        if (GameObject.ReferenceEquals(other.gameObject, otherGameObject)) {
			eventToRun.Invoke();
		}
    }

}
