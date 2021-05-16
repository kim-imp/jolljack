using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFInd : MonoBehaviour
{
    public bool isPlayerFind = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            isPlayerFind = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            isPlayerFind = false;
        }
    }
}
