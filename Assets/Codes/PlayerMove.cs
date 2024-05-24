using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("#Player Info")]
    public float speed;         // .. Player Speed
    public float rotateSpeed;   // .. Player Rotate Speed
    Vector3 moveVec;
    Vector3 dodgeVec;

    float hAxis;
    float vAxis;
    bool walkKeydown;          // .. Walk Key(Left Shift)
    bool jumpKeydown;          // .. Jump Key(Space Bar)

    [Header("#Player Status")]
    public bool isJump;        // .. Checking Player Jumping
    public bool isDodge;       // .. Checking Player Dodging

    bool isBorder;             // .. �浹 üũ�� ���� ����


    Rigidbody rigid;
    Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();
    }

    void FixedUpdate()
    {
        FreezeRotation();
        CantGoWall();
    }
    // .. Player Key Input
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        walkKeydown = Input.GetButton("Walk");             // .. left Shift Ű
        jumpKeydown = Input.GetButtonDown("Jump");
    }

    // .. Player Run & Walk
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;


        // .. ȸ���߿� �������� �Ұ�
        if (isDodge)
        {
            moveVec = dodgeVec;
        }

        // .. ����/�����߿� �̵��Ұ�
        if (GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().isSwap
            || !GameManager.Instance.player.GetComponent<PlayerAttack>().isFireReady
            || GameManager.Instance.player.GetComponent<PlayerAttack>().isReload)
            moveVec = Vector3.zero;

        // .. ���� �հ��� �� �ϰ�
        // .. �ȴ���(Shift)�̸� �̵��ӵ� ����
        if (!isBorder)
            transform.position += moveVec * speed * (walkKeydown ? 0.5f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", walkKeydown);
    }

    // .. �÷��̾� ���� ȸ��
    void Turn()
    {
        // .. �Է��� ������ ����
        if (moveVec == Vector3.zero)
            return;

        // .. ���¹������� �ڿ������� ȸ��
        Quaternion newRotation = Quaternion.LookRotation(moveVec);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
    }

    
    // .. Player Jump
    void Jump()
    {
        if(jumpKeydown && moveVec == Vector3.zero && !isJump && !isDodge && !GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().isSwap)
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    // .. Player Dodge
    void Dodge()
    {
        if (jumpKeydown && moveVec != Vector3.zero && !isDodge && !GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().isSwap)
        {
            dodgeVec = moveVec;
            speed *= 3/2f;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.7f);       // .. 0.7�� �Ŀ� DodgeOut
        }
    }

    // .. Player DodgeOut(reset speed)
    void DodgeOut()
    {
        speed *= 2/3f;
        isDodge = false;
    }


    // .. �������� ����
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    // .. �ܺ� �浹�� ���� ȸ�� ����
    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    // .. �� �ո� ����
    void CantGoWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 3, LayerMask.GetMask("Wall"));
    }
}
