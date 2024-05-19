using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeOrbit : MonoBehaviour
{
    public Transform target;
    public float orbitSpeed;
    Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;
    }
    void Update()
    {
        // .. 회전하는 수류탄들이 타겟(플레이어)을 따라가도록
        transform.position = target.position + offset;
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);
        offset = transform.position - target.position;
    }
}
