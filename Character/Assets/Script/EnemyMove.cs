using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public int nextMove;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartMove();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platfrom"));
        if (rayHit.collider == null)
        {
            StartCoroutine("turn");
            if (rayHit.distance < 0.5f)
                Debug.Log("경고!");
        }
    }

    IEnumerable Think()
    {
        Debug.Log("생각중");
        //다음활동설정
        nextMove = Random.Range(-1, 2);

        //애니메이션
        anim.SetInteger("WalkSpeed", nextMove);

        //애니메이션 방향전환
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;

        yield return new WaitForSeconds(3f);
        StartCoroutine("Think");
    }

    IEnumerable turn()
    {
        nextMove = nextMove * -1;
        spriteRenderer.flipX = nextMove == 1;

        yield return new WaitForSeconds(0.2f);
    }
    public void StartMove()
    {
        StartCoroutine("Think");
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
        if(collision.gameObject.tag == "Player")
        {
            Vector3 playerPos = collision.transform.position;
            if (playerPos.x > transform.position.x)
            {
                nextMove = 1;
            }
            if (playerPos.x < transform.position.x)
            {
                nextMove = -1;
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player")
            //Debug.Log("out");
            StartCoroutine("Think");
    }
}
