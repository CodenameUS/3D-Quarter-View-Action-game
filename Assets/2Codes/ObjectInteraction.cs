using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    GameObject nearObject;
    public GameObject[] grenades;               // .. 수류탄
    public GameObject grenadeObj;
    public Camera followCamera;

    Vector3 mousePos;

    bool interactionKeyDown;                    // .. 상호작용 키[E]
    bool grenadeThrowKeyDown;                   // .. 수류탄 투척키[우클릭]

    public int coin;
    public int ammo;
    public int health;
    public int hasGrenades;

    public int maxCoin;
    public int maxAmmo;
    public int maxHealth;
    public int maxHasGrenades;

    bool isDamage;

    MeshRenderer[] meshs;
    PlayerMove playerMove;
    PlayerWeaponSwap playerWeaponSwap;
    PlayerAttack playerAttack;

    void Awake()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();
        playerMove = GetComponent<PlayerMove>();
        playerWeaponSwap = GetComponent<PlayerWeaponSwap>();
        playerAttack = GetComponent<PlayerAttack>();
    }

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

    // .. 상호작용
    void Interaction()
    {
        // .. 상호작용 제한
        if(interactionKeyDown && nearObject != null && !playerMove.isJump && !playerMove.isDodge)
        {
            // .. 무기 오브젝트 상호작용
            if(nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();                    // .. 무기 정보 가져오기
                int itemIndex = item.value;
                playerWeaponSwap.hasWeapons[itemIndex] = true; // .. 해당 무기를 소유했음
                Destroy(nearObject);
            }
        }
    }

    // .. 수류탄 투척
    void Grenade()
    {
        if (hasGrenades == 0)
            return;

        if(grenadeThrowKeyDown && !playerAttack.isReload && !playerWeaponSwap.isSwap)
        {
            Ray ray = followCamera.ScreenPointToRay(mousePos);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 2;

                GameObject instantGrenade = Instantiate(grenadeObj, transform.position, transform.rotation);
                Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();

                // .. 마우스 방향으로 수류탄 투척
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
        else if(other.tag == "EnemyBullet")
        {
            if(!isDamage)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;

                bool isBossAttack = other.name == "Boss Melee Area";

                StartCoroutine(OnDamage(isBossAttack));
            }
            // .. 원거리 미사일 파괴
            if (other.GetComponent<Rigidbody>() != null)
                Destroy(other.gameObject);
        }
    }

    IEnumerator OnDamage(bool isBossAttack)
    {
        isDamage = true;

        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.yellow;
        }

        if (isBossAttack)
            playerMove.rigid.AddForce(transform.forward * -25, ForceMode.Impulse);

        yield return new WaitForSeconds(1f);

        isDamage = false;

        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }

        if (isBossAttack)
            playerMove.rigid.velocity = Vector3.zero;
    }
}
