using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 offset;

    void Start()
    {
        offset = new Vector3(0, transform.position.y, transform.position.z);    
    }

    // .. ī�޶� ����
    void Update()
    {
        transform.position = target.position + offset;
    }
}
