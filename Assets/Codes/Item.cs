using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum itemType { Ammo, Coin, Grenade, Heart, Weapon};
    public itemType type;
    public int value;

    // .. 아이템 회전
    void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }
}
