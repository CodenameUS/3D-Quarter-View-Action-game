using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    // .. ī�޶� ����
    void Update()
    {
        transform.position = target.position + offset;
    }
}
