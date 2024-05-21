using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum itemType { Ammo, Coin, Grenade, Heart, Weapon};
    public itemType type;
    public int value;

    Rigidbody rigid;
    SphereCollider sphereCollider;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    // .. 아이템 회전
    void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }

    // .. 아이템 물리 충돌 제거
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            rigid.isKinematic = true;
            sphereCollider.enabled = false;
        }
    }
}
