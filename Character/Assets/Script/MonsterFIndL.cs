using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFIndL : MonoBehaviour
{
    public bool isPlayerFind = false;
     GameObject Slime;

    void Awake()
    {
        Slime = GameObject.Find(this.transform.parent.name.ToString());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerFind = true;
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
