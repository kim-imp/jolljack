using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRun : MonoBehaviour
{
    GameObject Slime;
    // Start is called before the first frame update
    void Start()
    {
        Slime = GameObject.Find(this.transform.parent.name.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Vector3 playerPos = collision.transform.position;

            if (!Slime.GetComponent<MonsterMoveL>().attacking)
            {
                if (playerPos.x > transform.position.x)
                {
                    Slime.GetComponent<MonsterMoveL>().RushLeft();
                    //GetComponent<MonsterMove>().RushRight();
                }
                if (playerPos.x < transform.position.x)
                {
                    print(this.transform.parent.name.ToString());
                    Slime.GetComponent<MonsterMoveL>().RushRight();
                    //GetComponent<MonsterMove>().RushLeft();
                }
            }
        }
    }
}
