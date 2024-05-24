using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum attackType { Melee, Range };
    public attackType type;
    public int damage;                  // .. ������
    public float rate;                  // .. ���ݼӵ�
    public BoxCollider meleeArea;       // .. �������ݹ���
    public TrailRenderer trailEffect;   // .. ����ȿ��

    public Transform bulletPos;         // .. �Ѿ� ��ġ
    public GameObject bullet;           // .. �Ѿ�
    public Transform bulletCasePos;     // .. ź�� ��ġ
    public GameObject bulletCase;       // .. ź��

    public int maxAmmo;                 // .. �ִ� �Ѿ�
    public int curAmmo;                 // .. ���� �Ѿ�


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

    // .. ���� ����
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;               // .. ���ݹ��� Ȱ��ȭ
        trailEffect.enabled = true;             // .. ����ȿ�� Ȱ��ȭ

        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }

    // .. ���Ÿ� ����
    IEnumerator Shot()
    {
        // .. �Ѿ˻��� �� �߻�
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;

        yield return null;

        // .. ź�ǻ��� �� ���� ����
        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = intantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);        // .. ź�� ȸ��
    }
}
