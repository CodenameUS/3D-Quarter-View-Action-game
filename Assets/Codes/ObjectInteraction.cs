using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    GameObject nearObject;
    public GameObject[] grenades;               // .. ����ź
        
    bool interactionKeyDown;                    // .. ��ȣ�ۿ� Ű[E]

    [Header("#Player Status")]
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

    // .. ��ȣ�ۿ�
    void Interaction()
    {
        // .. ��ȣ�ۿ� ����
        if(interactionKeyDown && nearObject != null && !GameManager.Instance.player.GetComponent<PlayerMove>().isJump 
            && !GameManager.Instance.player.GetComponent<PlayerMove>().isDodge)
        {
            // .. ���� ������Ʈ ��ȣ�ۿ�
            if(nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();                    // .. ���� ���� ��������
                int itemIndex = item.value;
                GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().hasWeapons[itemIndex] = true; // .. �ش� ���⸦ ��������
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

    // .. �Ѿ�, ����, ü��, ����ź�� �Ծ��� ��
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
