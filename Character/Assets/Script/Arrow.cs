using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform Target;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;

    public Transform Projectile;
    private Transform myTransform;

    void Awake()
    {
        myTransform = transform;
    }

    void Start()
    {
        Target = GameObject.FindWithTag("Player").transform;
        StartCoroutine(SimulateProjectile());
    }



    public IEnumerator SimulateProjectile()
    {
        //yield return new WaitForSeconds(1.5f);

        // 투사체 이동
        Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);

        // 거리계산
        float target_Distance = Vector3.Distance(Projectile.position, Target.position);

        // 속도계산
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // 속도의XY값추출
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        //비행시간계산
        float flightDuration = target_Distance / Vx;

        // 발사체회전
        Vector3 tarpos = Target.transform.position - Projectile.transform.position;
        Projectile.rotation = Quaternion.LookRotation(tarpos).normalized;

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != "Enemy")
        {
            Destroy(this.gameObject);
            Debug.Log("없어짐");
        }
    }
}
