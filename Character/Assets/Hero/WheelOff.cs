using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelOff : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TurnOff());
    }

    // Update is called once per frame
    void Update()
    {

    }

    // W스킬 공격 범위 끄기
    IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}
