using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public int nextMove;
    bool attacking = false;
    public int HP = 3;
    CircleCollider2D tracecol;
    


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        tracecol = GetComponent<CircleCollider2D>();
        StartCoroutine("Think");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.5f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platfrom"));
        if(rayHit.collider == null)
        {
            StartCoroutine("turn");
            if (rayHit.distance < 0.5f) 
                Debug.Log("경고!");
        }

        if(HP == 0)
        {
            Die();
        }
    }

    IEnumerator Think()
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
        yield return new WaitForSeconds(3f);
        StartCoroutine("Think");
    }

    IEnumerator turn()
    {
        nextMove = nextMove * -1;
        spriteRenderer.flipX = nextMove == 1;

        yield return new WaitForSeconds(0.2f);
    }

    public void RushRight()
    {
        rigid.AddForce(Vector2.right * 5, ForceMode2D.Impulse);  
    }

    public void RushLeft()
    {
        Debug.Log("LRush");
        transform.Translate(Vector2.left * Time.deltaTime * 2);
    }


    public void OnAttack()
    {
        nextMove = 0;
        //rigid.AddForce(playerP * 5, ForceMode2D.Impulse);
        attacking = true;
        anim.SetBool("isAttack", true);
    }

    public void OffAttack()
    {
        attacking = false;
        anim.SetBool("isAttack", false);
    }

    public void Die()
    {
        Debug.Log("Die");
        rigid.velocity = new Vector2(0, rigid.velocity.y);
        anim.SetTrigger("isDie");
        spriteRenderer.color = new Color(1, 0, 0, 0.4f);
        tracecol.enabled = false;
        StopCoroutine("Think");
        StopCoroutine("Turn");
        Invoke("DeActive", 2.5f);
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StopCoroutine("Think");
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 playerPos = collision.transform.position;
            if (playerPos.x > transform.position.x)
            {
                if(attacking == true)
                {
                    nextMove = 0;
                }
                else
                {
                    nextMove = 1;
                    spriteRenderer.flipX = nextMove == 1;
                }
            }
            if (playerPos.x < transform.position.x)
            {
                if (attacking == true)
                {
                    nextMove = 0;
                }
                else
                {
                    nextMove = -1;
                    spriteRenderer.flipX = nextMove == 1;
                }
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            //Debug.Log("out");
            StartCoroutine("Think");
    }
}
