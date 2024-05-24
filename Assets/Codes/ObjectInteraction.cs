using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    GameObject nearObject;
    public GameObject[] grenades;               // .. ����ź
    public GameObject grenadeObj;
    public Camera followCamera;

    Vector3 mousePos;

    bool interactionKeyDown;                    // .. ��ȣ�ۿ� Ű[E]
    bool grenadeThrowKeyDown;                   // .. ����ź ��ôŰ[��Ŭ��]

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
        Grenade();
    }

    void GetInput()
    {
        interactionKeyDown = Input.GetButtonDown("Interaction");
        grenadeThrowKeyDown = Input.GetButtonDown("Fire2");
        mousePos = Input.mousePosition;
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

    // .. ����ź ��ô
    void Grenade()
    {
        if (hasGrenades == 0)
            return;

        if(grenadeThrowKeyDown && !GameManager.Instance.player.GetComponent<PlayerAttack>().isReload
            && !GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().isSwap)
        {
            Ray ray = followCamera.ScreenPointToRay(mousePos);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 2;

                GameObject instantGrenade = Instantiate(grenadeObj, transform.position, transform.rotation);
                Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();

                // .. ���콺 �������� ����ź ��ô
                rigidGrenade.AddForce(nextVec * 3, ForceMode.Impulse);
                rigidGrenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);

                hasGrenades--;
                grenades[hasGrenades].SetActive(false);
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
                    if (hasGrenades >= maxHasGrenades)
                    {
                        hasGrenades = maxHasGrenades;
                        break;
                    }
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    break;
            }
            Destroy(other.gameObject);
        }
    }

    
}
