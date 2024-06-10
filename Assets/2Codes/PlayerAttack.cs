using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Camera followCamera;

    Vector3 mousePos;

    public bool fireKeydown;            // .. ���� Ű[left ctrl / mouse left
    bool reloadKeydown;                 // .. ������ Ű[R]
    
    public bool isReload;               // .. ���� ����
    public bool isFireReady;            // .. ���� ���� ����

    float fireDelay;                    // .. ���� ������

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
        // .. ������ ���Ⱑ ������� ����
        if(playerWeaponSwap.equipedWeapon == null)
        {
            return;
        }

        // .. fireDelay�� ���� ���ݼӵ� ���� Ŭ �� ���� ����
        fireDelay += Time.deltaTime;
        isFireReady = playerWeaponSwap.equipedWeapon.rate < fireDelay;

        // .. ���� ����
        if(fireKeydown && isFireReady && !playerMove.isDodge
            && !playerWeaponSwap.isSwap
            && !playerMove.isJump)
        {
            // ���콺 ������ �������� ������ȯ 
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

            // .. Weapon type�� ���� Ʈ���� ����
            playerWeaponSwap.equipedWeapon.Use();
            anim.SetTrigger(playerWeaponSwap.equipedWeapon.type == Weapon.attackType.Melee ? "doSwing" : "doShot");
            // .. �����̸� �ʱ�ȭ
            fireDelay = 0;
        }
    }

    // .. ������
    void Reload()
    {
        // .. ������ ���Ⱑ ������ ����
        if (playerWeaponSwap.equipedWeapon == null)
        {
            return;
        }

        // .. ������ ���Ⱑ ���������� ��
        if(playerWeaponSwap.equipedWeapon.type == Weapon.attackType.Melee)
        {
            return;
        }

        // .. �Ѿ��� ���� ��
        if(objInt.ammo == 0)
        {
            return;
        }

        // .. ����, ȸ��, �������� �ƴ� ��
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

    // .. ������ Ż�� �� �Ѿ� ���� ���
    void ReloadOut()
    {
        // .. ���� źâ������ �ִ�źâ���� ������ �ִ�źâ����, ������ ����źâ����
        int newAmmo = objInt.ammo < playerWeaponSwap.equipedWeapon.maxAmmo ? objInt.ammo :
            playerWeaponSwap.equipedWeapon.maxAmmo;

        objInt.ammo -= newAmmo;

        playerWeaponSwap.equipedWeapon.curAmmo = playerWeaponSwap.equipedWeapon.maxAmmo;
        isReload = false;
    }
}
