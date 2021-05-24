using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFIndL : MonoBehaviour
{
    public bool isPlayerFind = false;
    GameObject Slime;
    public float AttackCool;
    public float AttackCoolNow;

    void Awake()
    {
        Slime = GameObject.Find(this.transform.parent.name.ToString());
        AttackCool = 2.0f;
        AttackCoolNow = 0;
    }

    private void Update()
    {
        print(AttackCoolNow);
        if(AttackCoolNow <= 0)
        {
            AttackCoolNow = 0;
        }
        else if(AttackCoolNow > 0)
        {
            AttackCoolNow -= Time.deltaTime;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerFind = true;
            Vector3 playerPos = collision.transform.position;
            if(GetComponent<MonsterFIndL>().isPlayerFind == true)
            {
                if (playerPos.x > transform.position.x)
                {
                    if(AttackCoolNow == 0)
                    {
                        Slime.GetComponent<MonsterMoveL>().OnAttack();
                    }
                    else
                    {
                        Slime.GetComponent<MonsterMoveL>().RushLeft();
                    }
                    //GetComponent<MonsterMove>().RushRight();
                }
                if (playerPos.x < transform.position.x)
                {
                    if (AttackCoolNow == 0)
                    {
                        Slime.GetComponent<MonsterMoveL>().OnAttack();
                    }
                    else
                    {
                        Slime.GetComponent<MonsterMoveL>().RushRight();
                    }
                    print(this.transform.parent.name.ToString());
                    //GetComponent<MonsterMove>().RushLeft();
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerFind = false;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.tag == "Player")
    //    {
    //        isPlayerFind = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if(collision.tag == "Player")
    //    {
    //        isPlayerFind = false;
    //    }
    //}
}
