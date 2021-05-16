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

    public GameObject AttackCol;
    public int HP = 3;
    


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine("Think");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.5f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
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
        nextMove = Random.Range(-1, 1);

        //애니메이션
        anim.SetInteger("WalkSpeed", nextMove);

        //애니메이션 방향전환
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;

        //재귀
        float nextThinkTime = Random.Range(1f, 3f);
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
        anim.SetInteger("WalkSpeed", 1);
    }

    public void RushLeft()
    {
        rigid.AddForce(Vector2.left * 5, ForceMode2D.Impulse);
        anim.SetInteger("WalkSpeed", -1);
        //transform.Translate(Vector2.left * Time.deltaTime * 2);
        Debug.Log(1);
    }


    public void OnAttack()
    {
        nextMove = 0;
        //rigid.AddForce(playerP * 5, ForceMode2D.Impulse);
        attacking = true;
        anim.SetBool("isAttack", true);
        AttackCol.SetActive(true);
    }

    public void OffAttack()
    {
        attacking = false;
        anim.SetBool("isAttack", false);

        AttackCol.SetActive(false);
    }

    public void Die()
    {
        Debug.Log("Die");
        rigid.velocity = new Vector2(0, rigid.velocity.y);
        anim.SetTrigger("isDie");
        spriteRenderer.color = new Color(1, 0, 0, 0.4f);
        StopCoroutine("Think");
        StopCoroutine("Turn");
    }

    void DeActive()
    {
        GameObject.Destroy(this);
        //gameObject.SetActive(false);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StopCoroutine("Think");
        }
        else if (collision.gameObject.tag == "Attack")
        {
            HP -= 3;
        }
        else if (collision.gameObject.tag == "Wheel")
        {
            HP -= 3;
        }
        else if (collision.gameObject.tag == "Flag")
        {
            HP -= 3;
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
                    spriteRenderer.flipX = true;
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
                    spriteRenderer.flipX = false;
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
