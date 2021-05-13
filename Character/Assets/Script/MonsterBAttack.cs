using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBAttack : MonoBehaviour
{
    GameObject Mon;
    GameObject arrow;

    void Awake()
    {
        Mon = GameObject.Find("Slime_B");
        arrow = GameObject.Find("SlimeAttack");
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Mon.GetComponent<MonsterMove>().OnAttack();
            arrow.GetComponent<Arrow>().StartCoroutine("SimulateProjectile");

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Mon.GetComponent<MonsterMove>().OffAttack();
            arrow.GetComponent<Arrow>().StopCoroutine("SimulateProjectile");
        }
    }
}
