using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("#Player Info")]
    public float speed;         // .. Player Speed
    public float rotateSpeed;   // .. Player Rotate Speed
    public Camera followCamera;
    Vector3 moveVec;
    Vector3 dodgeVec;

    float hAxis;
    float vAxis;
    bool walkKeydown;          // .. Walk Key(Left Shift)
    bool jumpKeydown;          // .. Jump Key(Space Bar)

    [Header("#Player Status")]
    public bool isJump;        // .. Checking Player Jumping
    public bool isDodge;       // .. Checking Player Dodging

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

    // .. Player Key Input
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        walkKeydown = Input.GetButton("Walk");             // .. left Shift 키
        jumpKeydown = Input.GetButtonDown("Jump");
    }

    // .. Player Run & Walk
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        // .. 회피중에 방향제어 불가
        if(isDodge)
        {
            moveVec = dodgeVec;
        }

        // .. 스왑/공격중에 이동불가
        if (GameManager.Instance.player.GetComponent<PlayerWeaponSwap>().isSwap
            || !GameManager.Instance.player.GetComponent<PlayerAttack>().isFireReady
            || GameManager.Instance.player.GetComponent<PlayerAttack>().isReload)
            moveVec = Vector3.zero;

        // .. 걷는중(Shift)이면 이동속도 감소
        transform.position += moveVec * speed * (walkKeydown ? 0.5f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", walkKeydown);
    }

    // .. Player Rotate
    void Turn()
    {
        // .. 입력이 없으면 리턴
        if (moveVec == Vector3.zero)
            return;

        // .. 보는방향으로 자연스럽게 회전
        Quaternion newRotation = Quaternion.LookRotation(moveVec);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotateSpeed * Time.deltaTime);

        /*
        // .. 마우스에 의한 회전
        if (GameManager.Instance.player.GetComponent<PlayerAttack>().fireKeydown)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
        */
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
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.7f);       // .. 0.7초 후에 DodgeOut
        }
    }

    // .. Player DodgeOut(reset speed)
    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    
    void OnCollisionEnter(Collision collision)
    {
        // .. 연속점프 방지
        if(collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
}
