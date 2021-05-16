using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    //public bool isAttack = false;
    //public GameObject AttackCol;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.tag == "Player")
    //    {
    //        AttackCol.SetActive(true);
    //        isAttack = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if(collision.tag == "Player")
    //    {
    //        AttackCol.SetActive(false);
    //        isAttack = false;
    //    }
    //}
    GameObject Mon;

    void Awake()
    {
        Mon = GameObject.Find("Slime_A");
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Mon.GetComponent<MonsterMove>().OnAttack();

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Mon.GetComponent<MonsterMove>().OffAttack();
        }
    }
}
