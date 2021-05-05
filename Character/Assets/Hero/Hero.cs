﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public float Speed = 3f;
    public float Jump = 100f;
    public float Fury = 0f;
    public int HP = 3;

    public GameObject attackRange;
    public GameObject attackRange2;
    public GameObject SwordFlag;
    public GameObject BSwordFlag;
    public GameObject LSwordFlag;
    public GameObject LBSwordFlag;
    public GameObject WheelRange;
    public GameObject BWheelRange;
    public Transform FlagPoint;

    public GameObject Slash;
    public GameObject SlashR;
    public GameObject Slash2;
    public GameObject SlashR2;
    public GameObject Wheel;
    public GameObject BWheel;
    public GameObject EChargeR;
    public GameObject ECharge;
    public GameObject EBerserk;
    public GameObject EBerserking;


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
    bool isLookR = true;
    bool isBerserk = false;
    bool isQCool = false;
    bool isWCool = false;
    bool isECool = false;

    bool comboPossible;
    int comboStep;
    public bool CanMove = true;
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
        if (isGrounded)
        {
            anim.SetBool("IsGround", true);
            anim.SetBool("IsJump", false);
            isjump = false;
        }
        if (!isGrounded)
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
        if (!isjump)
        {
            if (moveInput > 0)
            {
                LookRight();
                isLookR = true;
                anim.SetBool("IsLookR", true);
            }
            if (moveInput < 0)
            {
                LookLeft();
                isLookR = false;
                anim.SetBool("IsLookR", false);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (comboStep == 0)
            {
                CanMove = false;
                if (isLookR)
                    anim.Play("AttackR");
                else if (!isLookR)
                    anim.Play("AttackL");
                comboStep = 1;
            }
            if (comboPossible)
            {
                comboPossible = false;
                comboStep += 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isQCool)
            {
                CanMove = false;
                if (isBerserk)
                    anim.Play("BCharge");
                if (!isBerserk)
                    anim.Play("Charge");

                StartCoroutine(QCoolTime(2f));
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!isWCool)
            {
                anim.Play("WheelWind");
                StartCoroutine(WCoolTime(7f));
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isECool)
            {
                CanMove = false;
                anim.Play("SwordFlag");
                StartCoroutine(ECoolTime(4f));
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isBerserk)
            {
                CanMove = false;
                anim.Play("Berserk");
            }
        }

        if (Fury < 0)
        {
            Fury = 0;
            isBerserk = false;
        }
        else if (Fury > 0)
        {
            Fury -= 10 * Time.deltaTime;
        }
        print(isBerserk);
    }

    public void Berserking()
    {
        Vector3 Ep = new Vector3(transform.position.x, transform.position.y + .5f);
        Instantiate(EBerserking, Ep, transform.rotation, gameObject.transform);
    }

    public void Berserk()
    {
        Vector3 Ep = new Vector3(transform.position.x, transform.position.y + .5f);
        Instantiate(EBerserk, Ep, transform.rotation, gameObject.transform);
        isBerserk = true;
        Fury = 100.0f;
    }

    public void ComboPossible()
    {
        comboPossible = true;
    }

    public void Combo()
    {
        if (comboStep == 2)
        {
            if (isLookR)
            {
                anim.Play("Attack2R");
                StartCoroutine(AttackRMove());
            }
            else if (!isLookR)
            {
                anim.Play("Attack2L");
                StartCoroutine(AttackLMove());
            }
        }
    }

    public void ComboReset()
    {
        comboPossible = false;
        CanMove = true;
        WheelRange.SetActive(false);
        BWheelRange.SetActive(false);
        comboStep = 0;
    }

    // 나중에 스킬별로 둬서 데미지차이
    public void AttackRangeOn()
    {
        attackRange.SetActive(true);
    }
    public void AttackRangeOff()
    {
        attackRange.SetActive(false);
    }
    public void Attack2RangeOn()
    {
        attackRange2.SetActive(true);
    }
    public void Attack2RangeOff()
    {
        attackRange2.SetActive(false);
    }

    void LookRight()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    void LookLeft()
    {
        transform.localScale = new Vector3(-1, 1, 1);
    }

    public void SpawnSwordFlag()
    {
        if (isLookR)
        {
            if (isBerserk)
                Instantiate(BSwordFlag, FlagPoint.position, FlagPoint.rotation);
            else if (!isBerserk)
                Instantiate(SwordFlag, FlagPoint.position, FlagPoint.rotation);
        }
        else if (!isLookR)
        {
            if (isBerserk)
                Instantiate(LBSwordFlag, FlagPoint.position, FlagPoint.rotation);
            else if (!isBerserk)
                Instantiate(LSwordFlag, FlagPoint.position, FlagPoint.rotation);
        }
    }

    public void Charge()
    {
        Vector3 Ep;
        if (isLookR)
        {
            Ep = new Vector3(transform.position.x, transform.position.y + 1);
            Instantiate(EChargeR, Ep, transform.rotation, gameObject.transform);
            rigid.AddForce(Vector2.right * 4f, ForceMode2D.Impulse);
            anim.SetBool("IsLookR", true);
        }
        else if (!isLookR)
        {
            Ep = new Vector3(transform.position.x, transform.position.y + 1);
            Instantiate(ECharge, Ep, transform.rotation, gameObject.transform);
            rigid.AddForce(Vector2.left * 4f, ForceMode2D.Impulse);
            anim.SetBool("IsLookR", false);
        }
    }
    public void BCharge()
    {
        Vector3 Ep;
        if (isLookR)
        {
            Ep = new Vector3(transform.position.x, transform.position.y + 1);
            Instantiate(EChargeR, Ep, transform.rotation, gameObject.transform);
            rigid.AddForce(Vector2.right * 5f, ForceMode2D.Impulse);
            anim.SetBool("IsLookR", true);
        }
        else if (!isLookR)
        {
            Ep = new Vector3(transform.position.x, transform.position.y + 1);
            Instantiate(ECharge, Ep, transform.rotation, gameObject.transform);
            rigid.AddForce(Vector2.left * 5f, ForceMode2D.Impulse);
            anim.SetBool("IsLookR", false);
        }
    }

    public void StartWheelWind()
    {
        if(isBerserk)
        {
            BWheelRange.SetActive(true);
            Instantiate(BWheel, transform.position, transform.rotation, gameObject.transform);
        }
        if (!isBerserk)
        {
            WheelRange.SetActive(true);
            Instantiate(Wheel, transform.position, transform.rotation, gameObject.transform);
        }
    }
    public void EndWheelWind()
    {
        if (isBerserk)
            BWheelRange.SetActive(false);
        if (!isBerserk)
            WheelRange.SetActive(false);
    }

    IEnumerator AttackRMove()
    {
        yield return new WaitForSeconds(0.2f);
        rigid.AddForce(Vector2.right * 4f, ForceMode2D.Impulse);
    }
    IEnumerator AttackLMove()
    {
        yield return new WaitForSeconds(0.2f);
        rigid.AddForce(Vector2.left * 4f, ForceMode2D.Impulse);
    }

    IEnumerator QCoolTime(float cool)
    {
        isQCool = true;
        while (cool > 1.0f)
        {
            cool -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        isQCool = false;
    }
    IEnumerator WCoolTime(float cool)
    {
        isWCool = true;
        while (cool > 1.0f)
        {
            cool -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        isWCool = false;
    }
    IEnumerator ECoolTime(float cool)
    {
        isECool = true;
        while (cool > 1.0f)
        {
            cool -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        isECool = false;
    }

    public void HPMinus()
    {
        HP--;
        CanMove = false;
    }

    public void SlashOn()
    {
        Vector3 EP;
        if (isLookR)
        {
            EP = new Vector3(transform.position.x + 0.5f, transform.position.y + 1);
            Instantiate(SlashR, EP, transform.rotation, gameObject.transform);
        }
        if (!isLookR)
        {
            EP = new Vector3(transform.position.x - 0.5f, transform.position.y + 1);
            Instantiate(Slash, EP, transform.rotation, gameObject.transform);
        }
    }
    public void SlashOn2()
    {
        Vector3 EP;
        if (isLookR)
        {
            EP = new Vector3(transform.position.x + 0.5f, transform.position.y + 1);
            Instantiate(SlashR2, EP, transform.rotation, gameObject.transform);
        }
        if (!isLookR)
        {
            EP = new Vector3(transform.position.x - 0.5f, transform.position.y + 1);
            Instantiate(Slash2, EP, transform.rotation, gameObject.transform);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            if(HP == 0)
            {
                anim.Play("Death");
                if (isLookR)
                {
                    rigid.AddForce(Vector2.left * 4f, ForceMode2D.Impulse);
                }
                else if (!isLookR)
                {
                    rigid.AddForce(Vector2.right * 4f, ForceMode2D.Impulse);
                }
            }
            else if(HP > 0)
            {
                anim.Play("Hit");
                if (isLookR)
                {
                    rigid.AddForce(Vector2.left * 4f, ForceMode2D.Impulse);
                }
                else if(!isLookR)
                {
                    rigid.AddForce(Vector2.right * 4f, ForceMode2D.Impulse);
                }
            }
        }
    }
}
