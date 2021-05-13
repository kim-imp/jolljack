using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRush : MonoBehaviour
{
    GameObject Slime;

    void Awake()
    {
        Slime = GameObject.Find("Slime_A");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 playerPos = collision.transform.position;
            if (playerPos.x > transform.position.x)
            {
                Slime.GetComponent<MonsterMove>().RushRight();
            }
            if (playerPos.x < transform.position.x)
            {
                Slime.GetComponent<MonsterMove>().RushLeft();
            }
        }
    }
}
