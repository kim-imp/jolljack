using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkingOff : MonoBehaviour
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

    // 분노 애니메이션 시간 1초 + 9초 후에 꺼짐, 나중에 10초로 조정 후 애니메이션 앞쪽에 코드 갖다 박아도될듯
    IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(9f);
        Destroy(gameObject);
    }
}
