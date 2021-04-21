using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public float Speed = 3f;
    public float Jump = 100f;

    float moveInput;

    public bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    SpriteRenderer sprite;
    private Animator anim;
    Rigidbody2D rigid;

    Vector3 movement;
    bool isjump = false;

    bool CanMove = true;
    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
       
        moveInput = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("MoveLR", moveInput);
        if(isGrounded)
        {
            anim.SetBool("IsGround", true);
            anim.SetBool("IsJump", false);
            isjump = false;
        }
        if(!isGrounded)
        {
            anim.SetBool("IsGround", false);
            anim.SetBool("IsJump", true);
            isjump = false;
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rigid.AddForce(Vector2.up * Jump, ForceMode2D.Impulse);
            anim.SetBool("IsJump", true);
            isjump = true;
        }
        if (CanMove)
        {
            rigid.velocity = new Vector2(moveInput * Speed, rigid.velocity.y);
        }
        if(!isjump)
        {
            if (moveInput > 0)
            {
                LookRight();
                anim.SetBool("IsLookR", true);
            }
            if (moveInput < 0)
            {
                LookLeft();
                anim.SetBool("IsLookR", false);
            }
        }
    }

    void LookRight()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    void LookLeft()
    {
        transform.localScale = new Vector3(-1, 1, 1);
    }
}
