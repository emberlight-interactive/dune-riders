using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enable90Hertz : MonoBehaviour
{
    void Start()
    {
        OVRPlugin.systemDisplayFrequency = 90.0f;
    }
}
