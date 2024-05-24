using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum attackType { Melee, Range };
    public attackType type;
    public int damage;                  // .. 데미지
    public float rate;                  // .. 공격속도
    public BoxCollider meleeArea;       // .. 근접공격범위
    public TrailRenderer trailEffect;   // .. 공격효과

    public Transform bulletPos;         // .. 총알 위치
    public GameObject bullet;           // .. 총알
    public Transform bulletCasePos;     // .. 탄피 위치
    public GameObject bulletCase;       // .. 탄피

    public int maxAmmo;                 // .. 최대 총알
    public int curAmmo;                 // .. 현재 총알


    public void Use()
    {
        if(type == attackType.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if(type == attackType.Range && curAmmo > 0)
        {
            curAmmo--;
            StartCoroutine("Shot");
        }
        
    }

    // .. 근접 공격
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;               // .. 공격범위 활성화
        trailEffect.enabled = true;             // .. 공격효과 활성화

        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }

    // .. 원거리 공격
    IEnumerator Shot()
    {
        // .. 총알생성 및 발사
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;

        yield return null;

        // .. 탄피생성 및 배출 연출
        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = intantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);        // .. 탄피 회전
    }
}
