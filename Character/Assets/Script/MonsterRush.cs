using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRush : MonoBehaviour
{
    GameObject Slime;

    void Awake()
    {
        Slime = GameObject.Find(this.transform.parent.name.ToString());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Vector3 playerPos = collision.transform.position;
            if(GetComponent<MonsterFInd>().isPlayerFind == true)
            {
                if (playerPos.x > transform.position.x)
                {
                    Slime.GetComponent<MonsterMove>().RushRight();
                    //GetComponent<MonsterMove>().RushRight();
                }
                if (playerPos.x < transform.position.x)
                {
                    print(this.transform.parent.name.ToString());
                    Slime.GetComponent<MonsterMove>().RushLeft();
                    //GetComponent<MonsterMove>().RushLeft();
                }
            }
        }
    }
}
