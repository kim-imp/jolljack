using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
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
     if(collision.gameObject.tag == "Player")
        {
            Mon.GetComponent<MonsterMove>().OffAttack();
        }
    }
}
