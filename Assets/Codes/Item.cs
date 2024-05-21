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

    // .. ������ ȸ��
    void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }

    // .. ������ ���� �浹 ����
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            rigid.isKinematic = true;
            sphereCollider.enabled = false;
        }
    }
}
