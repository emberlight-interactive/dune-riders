using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AquariusMax.Wasteland3
{
    public class LadderClimb : MonoBehaviour
    {
        [SerializeField]
        private bool _onLadder = false;
        [SerializeField]
        private float heightFactor = 3.2f;

        private bool grabbed = false;
        private Quaternion ladderRotation = Quaternion.identity;

        private void OnTriggerEnter(Collider _ladder)
        {
            if (_ladder.gameObject.tag == "Ladder")
            {
                _onLadder = true;
                ladderRotation = _ladder.transform.rotation;
            }
        }

        private void OnTriggerExit(Collider _ladder)
        {
            if (_ladder.gameObject.tag == "Ladder")
            {
                _onLadder = false;
                grabbed = false;

                DemoCharacter DC = GetComponent("DemoCharacter") as DemoCharacter;
                DC.enabled = true;
            }
        }

        private void Update()
        {
            if(_onLadder)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    grabbed = !grabbed;

                    if (grabbed)
                    {
                        DemoCharacter DC = GetComponent("DemoCharacter") as DemoCharacter;
                        DC.enabled = false;
                    }
                    else
                    {
                        DemoCharacter DC = GetComponent("DemoCharacter") as DemoCharacter;
                        DC.enabled = true;
                    }
                }

                if (grabbed)
                {
                    if (Input.GetKey(KeyCode.W))
                    {
                        DemoCharacter DC = GetComponent("DemoCharacter") as DemoCharacter;
                        DC.GetComponent<CharacterController>().Move(ladderRotation * (Vector3.up / heightFactor));
                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        DemoCharacter DC = GetComponent("DemoCharacter") as DemoCharacter;
                        DC.GetComponent<CharacterController>().Move(ladderRotation * (Vector3.down / heightFactor));
                    }
                }
            }
        }
    }
}