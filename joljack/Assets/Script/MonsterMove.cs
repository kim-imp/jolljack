﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public int nextMove;
    static bool isTracing;
    GameObject traceTarget;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Think();

        Invoke("Think", 5);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isTracing == false)
        {
            rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        }

        if(isTracing == true)
        {
            Debug.Log("istracing");
            rigid.velocity = new Vector2(traceTarget.transform.position.x, rigid.velocity.y);
        }
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.5f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platfrom"));
        if(rayHit.collider == null)
        {
            turn();
            if (rayHit.distance < 0.5f) 
                Debug.Log("경고!");
        }
    }

    void Think()
    {
            //다음활동설정
            nextMove = Random.Range(-1, 2);

            //애니메이션
            anim.SetInteger("WalkSpeed", nextMove);

            //애니메이션 방향전환
            if (nextMove != 0)
                spriteRenderer.flipX = nextMove == 1;

            //재귀
            float nextThinkTime = Random.Range(2f, 5f);
            Invoke("Think", nextThinkTime);
    }

    void turn()
    {
        nextMove = nextMove * -1;
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke();
        Invoke("Think", 5);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("In");
            traceTarget = other.gameObject;
            isTracing = true;
            StopCoroutine("Think");

        }

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Inside");
            isTracing = true;
            anim.SetBool("isMoving", true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("out");
            isTracing = false;

            StartCoroutine("Think");
        }
    }
}
