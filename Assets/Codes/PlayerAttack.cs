using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool fireKeydown;            // .. ���� Ű[left ctrl / mouse left
    bool reloadKeydown;                 // .. ������ Ű[R]
    public bool isReload;               // .. ��������

    public bool isFireReady;            // .. ���� ���� ����
    float fireDelay;                    // .. ���� ������

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
        // .. ������ ���Ⱑ ������� ����
        if(GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon == null)
        {
            return;
        }

        // .. fireDelay�� ���� ���ݼӵ� ���� Ŭ �� ���� ����
        fireDelay += Time.deltaTime;
        isFireReady = GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon.rate < fireDelay;

        // .. ���� ����
        if(fireKeydown && isFireReady && !GameManager.Instance.player.GetComponent<PlayerMove>().isDodge
            && !GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().isSwap
            && !GameManager.Instance.player.GetComponent<PlayerMove>().isJump)
        {
            // .. Weapon type�� ���� Ʈ���� ����
            GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon.Use();
            anim.SetTrigger(GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon.type == Weapon.attackType.Melee ? "doSwing" : "doShot");
            // .. �����̸� �ʱ�ȭ
            fireDelay = 0;
        }
    }

    // .. ������
    void Reload()
    {
        // .. ������ ���Ⱑ ������ ����
        if (GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon == null)
        {
            return;
        }

        // .. ������ ���Ⱑ ���������� ��
        if(GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon.type == Weapon.attackType.Melee)
        {
            return;
        }

        // .. �Ѿ��� ���� ��
        if(GameManager.Instance.player.GetComponent<ObjectInteraction>().ammo == 0)
        {
            return;
        }

        // .. ����, ȸ��, �������� �ƴ� ��
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

    // .. ������ Ż�� �� �Ѿ� ���� ���
    void ReloadOut()
    {
        // .. ���� źâ������ �ִ�źâ���� ������ �ִ�źâ����, ������ ����źâ����
        int newAmmo = GameManager.Instance.player.GetComponent<ObjectInteraction>().ammo <
            GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon.maxAmmo ? GameManager.Instance.player.GetComponent<ObjectInteraction>().ammo :
            GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon.maxAmmo;

        GameManager.Instance.player.GetComponent<ObjectInteraction>().ammo -= newAmmo;

        GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon.curAmmo = 
            GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().equipedWeapon.maxAmmo;
        isReload = false;
    }
}
