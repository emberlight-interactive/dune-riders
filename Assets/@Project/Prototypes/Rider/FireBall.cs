using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireBall : MonoBehaviour
{
	[SerializeField] Rigidbody bulletPrefab;
	[SerializeField] InputActionProperty leftControllerY;

    // Start is called before the first frame update
    void Start()
    {
		leftControllerY.action.Enable();

    }

	void OnEnable() {
		leftControllerY.action.Enable();
		leftControllerY.action.performed += Fire;
	}

	void OnDisable() {
		leftControllerY.action.performed -= Fire;
	}

    void Fire(InputAction.CallbackContext a) {
		var ball = Instantiate(bulletPrefab);
		ball.transform.position = transform.position;
		ball.transform.rotation = transform.rotation;
		ball.velocity += transform.forward * 30;
	}
}
