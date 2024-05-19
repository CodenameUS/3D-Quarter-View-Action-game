using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    GameObject nearObject;
    public GameObject[] grenades;

    bool interactionKeyDown;

    public int coin;
    public int ammo;
    public int health;
    public int hasGrenades;

    public int maxCoin;
    public int maxAmmo;
    public int maxHealth;
    public int maxHasGrenades;

  
    void Update()
    {
        GetInput();
        Interaction();
    }

    void GetInput()
    {
        interactionKeyDown = Input.GetButtonDown("Interaction");
    }

    // .. 상호작용[E키]
    void Interaction()
    {
        // .. 점프/회피중이 아니고, 근처 오브젝트가 있을 때
        if(interactionKeyDown && nearObject != null && !GameManager.Instance.player.GetComponent<PlayerMove>().isJump 
            && !GameManager.Instance.player.GetComponent<PlayerMove>().isDodge)
        {
            // .. 무기 오브젝트
            if(nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();                    // .. 무기 정보 가져오기
                int itemIndex = item.value;
                GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().hasWeapons[itemIndex] = true; // .. 해당 무기를 소유했음
                Destroy(nearObject);
            }
        }
    }

   

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Weapon")
        {
            nearObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Weapon")
        {
            nearObject = null;
        }
    }

    // .. 총알, 코인, 체력, 수류탄을 먹었을 때
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();

            switch(item.type)
            {
                case Item.itemType.Ammo:
                    ammo += item.value;
                    if(ammo > maxAmmo)
                    {
                        ammo = maxAmmo;
                    }
                    break;
                case Item.itemType.Coin:
                    coin += item.value;
                    if (coin > maxCoin)
                    {
                        coin = maxCoin;
                    }
                    break;
                case Item.itemType.Heart:
                    health += item.value;
                    if (health > maxHealth)
                    {
                        health = maxHealth;
                    }
                    break;
                case Item.itemType.Grenade:
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    if (hasGrenades > maxHasGrenades)
                    {
                        hasGrenades = maxHasGrenades;
                    }
                    break;
            }
            Destroy(other.gameObject);
        }
    }
}
