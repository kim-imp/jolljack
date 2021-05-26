﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMoveL : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public float nextMove;
    public bool attacking = false;

    public Vector3 AttackPosition;
    public int HP;
    bool isDie = false;
    bool Hitting;


    public BoxCollider2D MonsterHitBox;
    public CircleCollider2D MonsterRun;

    public GameObject Arrow;
    GameObject SlimeFind;
    GameObject Hero;



    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine("Think");
        SlimeFind = GameObject.Find(this.transform.GetChild(1).name.ToString());
        Hero = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AttackPosition = GameObject.Find(this.transform.GetChild(2).name.ToString()).transform.position;
        print(HP);
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
        if (Hitting)
        {
            rigid.velocity = new Vector2(0, 0);
        }
        if (isDie)
        {
            StopCoroutine("Think");
            StopCoroutine("Turn");
            rigid.velocity = new Vector2(0, 0);
        }
        if (rayHit.collider == null)
        {
            StartCoroutine("turn");
            if (rayHit.distance < 0.5f)
                Debug.Log("경고!");
        }

        if (HP == 0)
        {
            Die();
        }
        else if (HP < 0)
        {
            HP = 0;
        }
        if (SlimeFind.GetComponent<MonsterFIndL>().isPlayerFind && !Hitting && !isDie)
        {
            rigid.velocity = new Vector2(nextMove * 1.5f, rigid.velocity.y);
            anim.SetFloat("WalkSpeed", nextMove * 1.5f);
            Vector3 playerPos = Hero.GetComponent<Hero>().transform.position;
            if (playerPos.x > transform.position.x)
            {
                if (attacking == true)
                {
                    nextMove = 0;
                    //spriteRenderer.flipX = true;
                    LookLeft();
                }
                else
                {
                    nextMove = -1f;
                    //spriteRenderer.flipX = nextMove == 1f;
                    LookRight();
                }
            }
            if (playerPos.x < transform.position.x)
            {
                if (attacking == true)
                {
                    nextMove = 0;
                    //spriteRenderer.flipX = false;
                    LookRight();
                }
                else
                {
                    nextMove = 1f;
                    //spriteRenderer.flipX = nextMove == 1f;
                    LookLeft();
                }
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

    IEnumerator Think()
    {
        print("Think");
        //다음활동설정
        nextMove = Random.Range(-1, 1);

        //애니메이션
        anim.SetFloat("WalkSpeed", nextMove);

        //애니메이션 방향전환
        if (nextMove >= 0)
            LookLeft();
        else if (nextMove <= 0)
            LookRight();

        //재귀
        float nextThinkTime = Random.Range(1f, 3f);
        Invoke("Think", nextThinkTime);
        yield return new WaitForSeconds(3f);
        StartCoroutine("Think");
    }

    IEnumerator turn()
    {
        nextMove = nextMove * -1f;
        //spriteRenderer.flipX = nextMove == 1f;
        if (nextMove >= 0)
            LookLeft();
        else if (nextMove <= 0)
            LookRight();

        yield return new WaitForSeconds(0.2f);
    }

    public void RushRight()
    {
        rigid.AddForce(Vector2.right * 5, ForceMode2D.Impulse);
        anim.SetFloat("WalkSpeed", 1);
    }

    public void RushLeft()
    {
        rigid.AddForce(Vector2.left * 5, ForceMode2D.Impulse);
        anim.SetFloat("WalkSpeed", -1);
        //transform.Translate(Vector2.left * Time.deltaTime * 2);
    }


    public void OnAttack()
    {
        if (!isDie)
        {
            nextMove = 0;
            //rigid.AddForce(playerP * 5, ForceMode2D.Impulse);
            attacking = true;
            anim.SetBool("isAttack", true);
            //AttackCol.SetActive(true);
            SlimeFind.GetComponent<MonsterFIndL>().AttackCoolNow = SlimeFind.GetComponent<MonsterFIndL>().AttackCool;
            //if(SlimeFind.GetComponent<MonsterFIndL>().AttackCoolNow > 0)
            //{
            //    SlimeFind.GetComponent<MonsterFIndL>().AttackCoolNow -= Time.deltaTime;
            //}
            //else if (SlimeFind.GetComponent<MonsterFIndL>().AttackCool <= 0)
            //{
            //    SlimeFind.GetComponent<MonsterFIndL>().AttackCoolNow = 0;
            //}
        }
    }

    public void Fire()
    {
        Instantiate(Arrow, AttackPosition, Arrow.transform.rotation);
    }


    public void OffAttack()
    {
        attacking = false;
        anim.SetBool("isAttack", false);

        //AttackCol.SetActive(false);
    }

    public void Die()
    {
        anim.SetBool("HittingDie", true);
        isDie = true;
        Debug.Log("Die");
        rigid.velocity = new Vector2(0, rigid.velocity.y);
        anim.SetTrigger("isDie");
        spriteRenderer.color = new Color(.8f, 0, 0, 0.4f);
        StopCoroutine("Think");
        StopCoroutine("Turn");
        MonsterHitBox.enabled = false;
        rigid.gravityScale = 0;
        StartCoroutine(AfterDie());
    }

    IEnumerator AfterDie()
    {
        yield return new WaitForSeconds(2.0f);
        GameObject.Destroy(this.gameObject);
    }


    void DeActive()
    {
        //GameObject.Destroy(this);
        //gameObject.SetActive(false);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StopCoroutine("Think");
        }
        if (collision.gameObject.tag == "Attack")
        {
            if (HP > 0)
                StartCoroutine("Hit");
            else if (HP <= 0)
                anim.SetBool("HittingDie", true);
        }
        else if (collision.gameObject.tag == "Wheel")
        {
            if (HP > 0)
                StartCoroutine("Hit");
            else if (HP <= 0)
                anim.SetBool("HittingDie", true);
        }
        else if (collision.gameObject.tag == "Flag")
        {
            if (HP > 0)
                StartCoroutine("Hit");
            else if (HP <= 0)
                anim.SetBool("HittingDie", true);
        }
    }

    IEnumerator Hit()
    {
        HP -= 1;
        if (HP == 0 || HP < 0)
        {
            anim.SetBool("HittingDie", true);
            yield return null;
        }
        else if(HP > 0)
        {
            Hitting = true;
            anim.Play("SlimeHit");
            rigid.velocity = new Vector2(0, 0);
            anim.SetFloat("WalkSpeed", 0);
            StopCoroutine("Think");
            yield return new WaitForSeconds(2f);
            Hitting = false;
            StartCoroutine("Think");
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Player")
        //{
        //    Vector3 playerPos = collision.transform.position;
        //    if (playerPos.x > transform.position.x)
        //    {
        //        if(attacking == true)
        //        {
        //            nextMove = 0;
        //            spriteRenderer.flipX = true;
        //        }
        //        else
        //        {
        //            nextMove = 1;
        //            spriteRenderer.flipX = nextMove == 1;
        //        }
        //    }
        //    if (playerPos.x < transform.position.x)
        //    {
        //        if (attacking == true)
        //        {
        //            nextMove = 0;
        //            spriteRenderer.flipX = false;
        //        }
        //        else
        //        {
        //            nextMove = -1;
        //            spriteRenderer.flipX = nextMove == 1;
        //        }
        //    }
        //}
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            //Debug.Log("out");
            StartCoroutine("Think");
    }
}
