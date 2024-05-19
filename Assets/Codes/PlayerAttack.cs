using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool fireKeydown;            // .. 공격 키[left ctrl / mouse left
    bool reloadKeydown;                 // .. 재장전 키[R]
    public bool isReload;               // .. 장전상태

    public bool isFireReady;            // .. 공격 가능 여부
    float fireDelay;                    // .. 공격 딜레이

    Animator anim;

    void Awake()
    { 
        anim = GetComponentInChildren<Animator>();
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
    }

    // .. Player Attack
    void Attack()
    {
        // .. 장착한 무기가 없을경우 무시
        if(GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon == null)
        {
            return;
        }

        // .. fireDelay가 무기 공격속도 보다 클 때 공격 가능
        fireDelay += Time.deltaTime;
        isFireReady = GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon.rate < fireDelay;

        // .. 공격 제한
        if(fireKeydown && isFireReady && !GameManager.Instance.player.GetComponent<PlayerMove>().isDodge
            && !GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().isSwap
            && !GameManager.Instance.player.GetComponent<PlayerMove>().isJump)
        {
            // .. Weapon type에 따른 트리거 설정
            GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon.Use();
            anim.SetTrigger(GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon.type == Weapon.attackType.Melee ? "doSwing" : "doShot");
            // .. 딜레이를 초기화
            fireDelay = 0;
        }
    }

    // .. 재장전
    void Reload()
    {
        // .. 장착한 무기가 없으면 리턴
        if (GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon == null)
        {
            return;
        }

        // .. 장착한 무기가 근접무기일 때
        if(GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon.type == Weapon.attackType.Melee)
        {
            return;
        }

        // .. 총알이 없을 때
        if(GameManager.Instance.player.GetComponent<ObjectInteraction>().ammo == 0)
        {
            return;
        }

        // .. 점프, 회피, 스왑중이 아닐 때
        if(reloadKeydown && !GameManager.Instance.player.GetComponent<PlayerMove>().isJump &&
            !GameManager.Instance.player.GetComponent<PlayerMove>().isDodge &&
            !GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().isSwap &&
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
        int newAmmo = GameManager.Instance.player.GetComponent<ObjectInteraction>().ammo <
            GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon.maxAmmo ? GameManager.Instance.player.GetComponent<ObjectInteraction>().ammo :
            GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon.maxAmmo;

        GameManager.Instance.player.GetComponent<ObjectInteraction>().ammo -= newAmmo;

        GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon.curAmmo = 
            GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon.maxAmmo;
        isReload = false;
    }
}
