using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    // .. 카메라 조정
    void Update()
    {
        transform.position = target.position + offset;
    }
}
