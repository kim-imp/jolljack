using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public float Speed = 3f;
    public float Jump = 100f;
    public float Fury = 0f;
    public int HP = 100;
    public float QCTime = 2f;
    public float WCTime = 7f;
    public float ECTime = 4f;
    public float NQCTime;
    public float NWCTime;
    public float NECTime;


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

    bool isjump = false;
    bool isLookR = true;
    bool isBerserk = false;
    public bool isQCool = false;
    public bool isWCool = false;
    public bool isECool = false;
    bool isCanAlive = true;
    bool isDie = false;

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
        // 바닥 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        // 이동
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

        // 공격
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

        // 스킬
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isQCool)
            {
                CanMove = false;
                if (isBerserk)
                    anim.Play("BCharge");
                if (!isBerserk)
                    anim.Play("Charge");

                StartCoroutine(QCoolTime(QCTime));
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!isWCool)
            {
                anim.Play("WheelWind");
                StartCoroutine(WCoolTime(WCTime));
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isECool)
            {
                CanMove = false;
                anim.Play("SwordFlag");
                StartCoroutine(ECoolTime(ECTime));
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isBerserk && Fury == 100)
            {
                CanMove = false;

                anim.Play("Berserk");
            }
        }

        // 분노(광폭화 자원) 관리
        if (Fury < 0)
        {
            Fury = 0;
            isBerserk = false;
        }
        else if (Fury > 0 && isBerserk == true)
        {
            Fury -= 10 * Time.deltaTime;
        }
        else if(Fury < 100 && isBerserk == false)
        {
            Fury += 10 * Time.deltaTime;
            if (Fury > 100)
                Fury = 100;
        }
        if(HP == 0)
        {

        }
    }


    // 분노 중
    public void Berserking()
    {
        Vector3 Ep = new Vector3(transform.position.x, transform.position.y + .5f);
        Instantiate(EBerserking, Ep, transform.rotation, gameObject.transform);
    }

    // 분노
    public void Berserk()
    {
        Vector3 Ep = new Vector3(transform.position.x, transform.position.y + .5f);
        Instantiate(EBerserk, Ep, transform.rotation, gameObject.transform);
        isBerserk = true;
    }

    // 공격시 이동
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

    // 콤보 공격
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
        attackRange.SetActive(false);
        attackRange2.SetActive(false);
        WheelRange.SetActive(false);
        BWheelRange.SetActive(false);
        comboStep = 0;
    }

    // 공격 범위
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

    // 나중에 스킬별로 둬서 데미지차이
    // 스킬 범위
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

    // 바라보는 방향
    void LookRight()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    void LookLeft()
    {
        transform.localScale = new Vector3(-1, 1, 1);
    }



    // Q 공격
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

    // W 공격
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

    // E 생성
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

    // 쿨타임
    IEnumerator QCoolTime(float cool)
    {
        isQCool = true;
        NQCTime = cool;
        while (NQCTime > 0.0f)
        {
            NQCTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        if (NQCTime <= 0)
            NQCTime = 0;
        isQCool = false;
    }
    IEnumerator WCoolTime(float cool)
    {
        isWCool = true;
        NWCTime = cool;
        while (NWCTime > 0.0f)
        {
            NWCTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        if (NWCTime <= 0)
            NWCTime = 0;
        isWCool = false;
    }
    IEnumerator ECoolTime(float cool)
    {
        isECool = true;
        NECTime = cool;
        while (NECTime > 0.0f)
        {
            NECTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        if (NECTime <= 0)
            NECTime = 0;
        isECool = false;
    }


    // HP관리 + HP0일때 한번 버티기
    public void HPMinus()
    {
        if (HP > 0)
        {
            HP -= 40;
            if (HP <= 0 && isCanAlive == true)
            {
                HP = 0;
                isCanAlive = false;
                Berserk();
                Berserking();
            }
            if (HP <= 0 && isCanAlive == false)
            {
                HP = -1;
            }
        }
        CanMove = false;
    }

   
    // 트리거 이것저것
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDie == false)
        {
            if (collision.tag == "Enemy_A")
            {
                if (HP > 0)
                {
                    anim.Play("Hit");
                    if (isLookR)
                    {
                        rigid.AddForce(Vector2.left * 4f, ForceMode2D.Impulse);
                    }
                    else if (!isLookR)
                    {
                        rigid.AddForce(Vector2.right * 4f, ForceMode2D.Impulse);
                    }
                }
                else if (HP <= 0)
                {
                    HP = 0;
                    anim.Play("Death");
                    isDie = true;
                    if (isLookR)
                    {
                        rigid.AddForce(Vector2.left * 4f, ForceMode2D.Impulse);
                    }
                    else if (!isLookR)
                    {
                        rigid.AddForce(Vector2.right * 4f, ForceMode2D.Impulse);
                    }
                }
            }
        }
    }
}
