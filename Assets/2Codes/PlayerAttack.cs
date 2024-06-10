using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Camera followCamera;

    Vector3 mousePos;

    public bool fireKeydown;            // .. 공격 키[left ctrl / mouse left
    bool reloadKeydown;                 // .. 재장전 키[R]
    
    public bool isReload;               // .. 장전 여부
    public bool isFireReady;            // .. 공격 가능 여부

    float fireDelay;                    // .. 공격 딜레이

    Animator anim;

    PlayerWeaponSwap playerWeaponSwap;
    PlayerMove playerMove;
    ObjectInteraction objInt;

    void Awake()
    { 
        anim = GetComponentInChildren<Animator>();
        playerWeaponSwap = GetComponent<PlayerWeaponSwap>();
        playerMove = GetComponent<PlayerMove>();
        objInt = GetComponent<ObjectInteraction>();
    }

    void Update()
    {
        GetInput();
        Attack();
        Reload();
    }

    void GetInput()
    {
        fireKeydown = Input.GetButton("Fire1");
        reloadKeydown = Input.GetButtonDown("Reload");
        mousePos = Input.mousePosition;
    }

    // .. Player Attack
    void Attack()
    {
        // .. 장착한 무기가 없을경우 무시
        if(playerWeaponSwap.equipedWeapon == null)
        {
            return;
        }

        // .. fireDelay가 무기 공격속도 보다 클 때 공격 가능
        fireDelay += Time.deltaTime;
        isFireReady = playerWeaponSwap.equipedWeapon.rate < fireDelay;

        // .. 공격 제한
        if(fireKeydown && isFireReady && !playerMove.isDodge
            && !playerWeaponSwap.isSwap
            && !playerMove.isJump)
        {
            // 마우스 포인터 방향으로 방향전환 
            if (fireKeydown)
            {
                Ray ray = followCamera.ScreenPointToRay(mousePos);
                RaycastHit rayHit;
                if (Physics.Raycast(ray, out rayHit, 100))
                {
                    Vector3 nextVec = rayHit.point - transform.position;
                    nextVec.y = 0;
                    transform.LookAt(transform.position + nextVec);
                }
            }

            // .. Weapon type에 따른 트리거 설정
            playerWeaponSwap.equipedWeapon.Use();
            anim.SetTrigger(playerWeaponSwap.equipedWeapon.type == Weapon.attackType.Melee ? "doSwing" : "doShot");
            // .. 딜레이를 초기화
            fireDelay = 0;
        }
    }

    // .. 재장전
    void Reload()
    {
        // .. 장착한 무기가 없으면 리턴
        if (playerWeaponSwap.equipedWeapon == null)
        {
            return;
        }

        // .. 장착한 무기가 근접무기일 때
        if(playerWeaponSwap.equipedWeapon.type == Weapon.attackType.Melee)
        {
            return;
        }

        // .. 총알이 없을 때
        if(objInt.ammo == 0)
        {
            return;
        }

        // .. 점프, 회피, 스왑중이 아닐 때
        if(reloadKeydown && !playerMove.isJump &&
            !playerMove.isDodge &&
            !playerWeaponSwap.isSwap &&
            isFireReady)
        {
            anim.SetTrigger("doReload");
            isReload = true;

            Invoke("ReloadOut", 3f);
        }
    }

    // .. 재장전 탈출 및 총알 개수 계산
    void ReloadOut()
    {
        // .. 남은 탄창개수가 최대탄창보다 많으면 최대탄창으로, 적으면 남은탄창으로
        int newAmmo = objInt.ammo < playerWeaponSwap.equipedWeapon.maxAmmo ? objInt.ammo :
            playerWeaponSwap.equipedWeapon.maxAmmo;

        objInt.ammo -= newAmmo;

        playerWeaponSwap.equipedWeapon.curAmmo = playerWeaponSwap.equipedWeapon.maxAmmo;
        isReload = false;
    }
}
