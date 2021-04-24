using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFlag : MonoBehaviour
{
    Vector3 pos;
    float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        Destroy(gameObject, 2f);
    }
}
