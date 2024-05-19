using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSwap : MonoBehaviour
{
    public GameObject[] weapons;
    public bool[] hasWeapons;

    bool swapKey1down;      // .. 숫자1키
    bool swapKey2down;      // .. 숫자2키
    bool swapKey3down;      // .. 숫자3키

    public bool isSwap;

    public Weapon equipedWeapon;       // .. 현재 장착중인 무기
    int equipedWeaponIndex = -1;    // .. default weapon index

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

    // .. Weapon Swap
    void Swap()
    {
        // .. 같은무기로 스왑하거나, 무기가없는데 스왑하는경우 방지
        if (swapKey1down && (!hasWeapons[0] || equipedWeaponIndex == 0))
            return;
        if (swapKey2down && (!hasWeapons[1] || equipedWeaponIndex == 1))
            return;
        if (swapKey3down && (!hasWeapons[2] || equipedWeaponIndex == 2))
            return;

        int weaponIndex = -1;                   // .. default value
        if (swapKey1down) weaponIndex = 0;
        if (swapKey2down) weaponIndex = 1;
        if (swapKey3down) weaponIndex = 2;

        // .. Swap 키 입력 들어왔을 때(점프/회피중이 아닐 때)
        if((swapKey1down || swapKey2down || swapKey3down) && !GameManager.Instance.player.GetComponent<PlayerMove>().isJump 
            && !GameManager.Instance.player.GetComponent<PlayerMove>().isDodge)
        {
            // .. 장착중이던 무기를 비활성화 한 뒤 장착
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

    void SwapOut()
    {
        isSwap = false;
    }

    
}
