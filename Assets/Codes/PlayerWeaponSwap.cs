using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSwap : MonoBehaviour
{
    public GameObject[] weapons;
    public bool[] hasWeapons;
    
    bool swapKey1down;      // .. ����1 Ű
    bool swapKey2down;      // .. ����2 Ű
    bool swapKey3down;      // .. ����3 Ű

    public bool isSwap;
    
    public Weapon equipedWeapon;         // .. ���� �������� ����

    int equipedWeaponIndex = -1;         // .. default weapon index

    Animator anim;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        GetInput();
        Swap();
    }

    void GetInput()
    {
        swapKey1down = Input.GetButtonDown("Swap1");
        swapKey2down = Input.GetButtonDown("Swap2");
        swapKey3down = Input.GetButtonDown("Swap3");
    }

    // .. ���� ����
    void Swap()
    {
        // .. ��������� �����ϰų�, ���Ⱑ���µ� �����ϴ°�� ����
        if (swapKey1down && (!hasWeapons[0] || equipedWeaponIndex == 0))
            return;
        if (swapKey2down && (!hasWeapons[1] || equipedWeaponIndex == 1))
            return;
        if (swapKey3down && (!hasWeapons[2] || equipedWeaponIndex == 2))
            return;

        int weaponIndex = -1;                   // .. ���� �ε��� �ο� default value = -1
        if (swapKey1down) weaponIndex = 0;
        if (swapKey2down) weaponIndex = 1;
        if (swapKey3down) weaponIndex = 2;

        // .. Swap ����
        if((swapKey1down || swapKey2down || swapKey3down) && !GameManager.Instance.player.GetComponent<PlayerMove>().isJump 
            && !GameManager.Instance.player.GetComponent<PlayerMove>().isDodge)
        {
            // .. �������̴� ���⸦ ��Ȱ��ȭ �� �� ����
            if (equipedWeapon != null)
                equipedWeapon.gameObject.SetActive(false);

            equipedWeaponIndex = weaponIndex;
            equipedWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipedWeapon.gameObject.SetActive(true);

            // .. Swap animation
            anim.SetTrigger("doSwap");

            isSwap = true;

            Invoke("SwapOut", 0.4f);
        }
    }

    // .. ���һ��� Ż��
    void SwapOut()
    {
        isSwap = false;
    }

    
}
