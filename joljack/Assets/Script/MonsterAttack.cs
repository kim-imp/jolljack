using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    Animator anim;

    private void OnCollisionEnter(Collision other)
    {
        
    }


    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag != "Player")
            return;

        if (other.gameObject.tag == "Player")
        {
            anim.SetBool("isAttack", true);
        }
    }
}
